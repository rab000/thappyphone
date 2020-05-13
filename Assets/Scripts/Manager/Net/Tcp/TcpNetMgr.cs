using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Collections;
using System.Text;
using NLog;

/// <summary>
/// TCP网络管理
/// 
/// 
/// </summary>
public class TcpNetMgr : AbsNetMgr{

	private bool BeShowLog = true;

	#region var
	/// <summary>
	/// socket消息头常亮
	/// </summary>
	public const int HEAD_MSG = 127;

	private Socket _Socket = null;
	private NetworkStream _NetworkStream;
	private Stream _Stream = null;
	public NetBackMgr _NetBackMgr;

	/// <summary>
	/// socket是否已连接
	/// </summary>
	private bool BeSocketConnect = false;

	#endregion

	#region life

	private static TcpNetMgr Ins = null;

	public static TcpNetMgr GetInst ()
	{
		if (null == Ins)
			Ins = new TcpNetMgr ();
		return Ins;
	}

	public TcpNetMgr ()
	{
		if (null == _NetBackMgr) {
			_NetBackMgr = NetBackMgr.GetInst ();
			_NetBackMgr.RegistNetBack ();
		}
	}

	// 连接
	public bool VConnect (string ip, int port){
		LogMgr.I("TcpSocketMgr","Connect","IP = " + ip + "/n" + " Port = " + port,BeShowLog);
		try {
			if (BeConnect()) {
				//如果当前有连接，就先关闭当期连接，在开启新的连接
				System.Threading.Thread.Sleep (10);
				this.Closed ("ReConnectServer");
				System.Threading.Thread.Sleep (200);
			}
			IPEndPoint remoteServer = new IPEndPoint (IPAddress.Parse (ip), port);
			_Socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			_Socket.NoDelay = true;
			BeSocketConnect = false;
			_Socket.BeginConnect (remoteServer, OnConnect, _Socket);
		} catch (System.Exception ex) {
			// 将断开连接消息压到解码队列
			//QueueDisconnect();
			this.Closed (ex.Message);
			Debug.LogError ("网络链接异常：" + ex.Message);

			Cmd4Rec cmd = new Cmd4Rec ();
			cmd.MsgID = HEAD_MSG;
			cmd.Content = string.Format ("error {0}", ex.Message);
			AbsNetMgr.Cmd4RecQueue.Enqueue (cmd);
			//NetWorkMessageResp.AddCommandSys (cmd);
			return false;
		}
		return true;
	}


	// 断开连接
	public bool VDisconnect (){
		if (null != _Socket) {
			// 将断开连接消息压到解码队列
			if (BeSocketConnect) {
				Closed ("QueueDisconnect");
				BeSocketConnect = false;
			}
			VRelease ();
			return true;
		} else {
			return false;
		}
	}

	private void Closed (String msg){

		if (_Socket == null) {
			LogMgr.E("TcpSocketMgr","Closed","_Socket==null closeSocket失败",BeShowLog);
			return;
		}
		//断线后重置

		//Debug.LogError("断开Socket连接时设置sendLoginMessage=" + sendLoginMessage.ToString());
		lock (this) {
			BeMsgSending = false;
			SendMsgQueue.Clear ();
		}
		BeSocketConnect = false;
		if (_Socket != null && _Socket.Connected) {

			try {

				_Socket.Shutdown (SocketShutdown.Both);
			} catch (Exception ex) {
				Debug.Log ("网络关闭：" + ex.ToString ());
			}
			try {
				_Socket.Close ();
				_Socket = null;
				Debug.Log ("关闭socket");
			} catch (Exception ex) {

				Debug.Log ("断开连接" + ex.ToString ());
				//SystemMessageManager.GetInst().Msg("与服务器断开连接 " + ex.ToString());

			}
		}

	}


	public void VRelease (){
		RecMsgCallback = null;
	}

	//TODO 这里需要通过调用interface中的VTick来调用，这段是用来处理本地收到的cmd(首先收到byte[],然后转换成cmd存到队列里)队列中的cmd
	// tick
	public void VTick (uint nLimitedCount){
		if (null != RecMsgCallback) {
			for (int i = 0; i < nLimitedCount; ++i) {
				if (AbsNetMgr.Cmd4RecQueue.Count > 0) {
					Cmd4Rec msg = AbsNetMgr.Cmd4RecQueue.Dequeue ();
					RecMsgCallback (msg);
				} else {
					break;
				}
			}
		}
	}

	/// <summary>
	/// 是否正在连接状态
	/// </summary>
	/// <returns></returns>
	public bool BeConnect (){
		if (_Socket == null)
			return false;
		return _Socket.Connected;
	}

	#endregion


	#region callback




	#endregion


	#region receive
	/// <summary>
	/// 用来记录_BinaryReader.BaseStream 已经读取到了什么位置
	/// </summary>
	private int tempBinaryReaderPos = 0;
	/// <summary>
	/// 每处理一条消息EventID++
	/// 预计调试时用，暂时没具体使用
	/// </summary>
	public static int EventID = 0;
	/// <summary>
	/// 记录下接收到的总byte长度
	/// </summary>
	private double RevMsgTotalSize = 0;
	/// <summary>
	/// 记录每次从Stream中读到NetBuffer的byte数
	/// </summary>
	public int BytesRecOnce;
	/// <summary>
	/// 当前数据NetBuffer(就是NetBuffer1或者NetBuffer2)中上一次数据处理没处理完的，剩余的byte数
	/// </summary>
	public int BytesNumWaiting4Process = 0;

	/// <summary>
	/// netBuffer容量，一次最多能从网络流中读取的容量
	/// </summary>
	public const int NET_BUFFER_SIZE = 102400;

	/// <summary>
	/// 每次从(网络流)Stream中读取的流数据时
	/// 先对netbuffer清零
	/// 然后把读到的数据直接存到这个buffer中
	/// </summary>
	public byte[] NetBuffer = new byte[NET_BUFFER_SIZE];

	/// <summary>
	/// 用于处理数据的双缓存
	/// 具体流程
	/// 1 从网络流中读取数据到NetBuffer
	/// 2 把NetBuffer中的数据转存到 处理数据的缓存CurNetBuffer(NetBuffer1)
	/// 3 处理CurNetBuffer中的数据，如果发现CurNetBuffer中不足一条数据，就把剩余数据放到NextNetBuffer(NetBuffer2)中
	/// 4 翻转CurNetBuffer与NextNetBuffer，就是切换NetBuffer1，NetBuffer2代表的含义
	/// 5 当下一次向数据缓存CurNetBuffer存数据时，因为CurNetBuffer(NetBuffer2)中还有一部分数据，
	/// 把新读入的数据放到上一次剩余数据的后面，然后在进行数据解析处理
	/// 
	/// </summary>
	public byte[] NetBuffer1 = new byte[NET_BUFFER_SIZE];
	public byte[] NetBuffer2 = new byte[NET_BUFFER_SIZE];


	private Listener<Cmd4Rec> RecMsgCallback = null;


	public void SetRevMsgCallback (Listener<Cmd4Rec> callback){
		RecMsgCallback = callback;
	}

	/// <summary>
	/// 连接成功异步回调
	/// </summary>
	/// <param name="ar">Ar.</param>
	private void OnConnect (System.IAsyncResult ar){
		try {
			if (null == _Socket) {
				LogMgr.E("TcpSocketMgr","OnConnect","OnConnet后socket=null",BeShowLog);
				Debug.LogError ("null == m_client");
				return;
			}

			// 连接成功
			_Socket.EndConnect (ar);

			BeSocketConnect = true;

			_NetworkStream = new NetworkStream (_Socket, false);

			_Stream = _NetworkStream;

			SetupRecieveCallback ();

		} catch (System.Exception ex) {
			LogMgr.E("Socket Error Happend[HandleConnectCallback]%:" + ex.Message);

			// 将断开连接消息压到解码队列
			this.Closed (ex.Message);
		} finally {
			if (!_Socket.Connected) {
				////####之后添加
				//XCKuaFuMessage.AutoLogin msg = new XCKuaFuMessage.AutoLogin();
				//msg.cmd = FID.CC_KUAFU_AUTOLOGIN;
				//msg.bIsReady = true;
				//NetWorkMessageResp.cmdlist.Enqueue(msg);
			}
		}
	}


	private void SetupRecieveCallback (){

		if (_Socket == null || !_Socket.Connected) {
			this.Closed ("SetupRecieveCallback");
			return;
		}
		try {
			for (int i = 0; i < NetBuffer.Length; i++) {
				NetBuffer [i] = 0;
			}
			if (NetBuffer.Length > BytesNumWaiting4Process) {
				//ninfo 这里能读到的最大数是netbuffer.Length，之所以写netbuffer.Length - refreadpos是怕读多了，超出缓冲能处理的范围
				//ninfo stream从网络一次最大读取netbuffer.length，stream中存储的来自网络的量可能比这个length要大，但大多情况是比这个小
				//ninfo 后面自所以处理很复杂，原因就在于stream.BeginRead时网络stream中不一定有多少数据
				_Stream.BeginRead (NetBuffer, 0, NetBuffer.Length - BytesNumWaiting4Process, new AsyncCallback (OnRecievedData), null);
			} else{
				LogMgr.E("[网络线程重大bug]数据流溢出！");
			}
		} catch (Exception ex) {
			LogMgr.E("数据读取回掉异常：" + ex.Message);
		}
	}
		
	private void OnRecievedData (IAsyncResult ar){
		if (_Socket == null || !_Socket.Connected) {
			//已经彻底断开了，关了吧
			this.Closed ("OnRecievedData");
			return;
		}
		try {
			//ninfo 这句返回的是stream真实（向netbuffer中）读了多少数据
			BytesRecOnce = _Stream.EndRead (ar);
			if (BytesRecOnce > 0) {
				if (BytesRecOnce > int.MaxValue)
					Debug.LogError ("消息太大了！！！nBytesRec=" + BytesRecOnce);
				

				CheckBuffer (BytesRecOnce);
				RevMsgTotalSize += BytesRecOnce;
				Debug.LogError ("已接收数据：" + RevMsgTotalSize / 1024 + "kb，合计：" + (RevMsgTotalSize + sendMsgLen) / 1024);

				byte[] curbuffer = GetCurrentBuffer ();
				byte[] nextbuffer = GetNextBuffer ();
				for (int i = 0; i < nextbuffer.Length; i++) {
					nextbuffer [i] = 0;
				}
				int effectiveSizeInBuffer = BytesNumWaiting4Process + BytesRecOnce;
				if (effectiveSizeInBuffer >= NET_BUFFER_SIZE) {//清空，从头来
					Debug.LogError ("[严重bug]已经越界了！refreadpos=" + BytesNumWaiting4Process + " nBytesRec=" + BytesRecOnce + " nBufferCount=" + NET_BUFFER_SIZE);
					BytesNumWaiting4Process = 0;
					effectiveSizeInBuffer = BytesRecOnce;
				}
				if (effectiveSizeInBuffer < NET_BUFFER_SIZE) {
					for (int i = 0; i < BytesRecOnce; i++) {
						curbuffer [i + BytesNumWaiting4Process] = NetBuffer [i];
					}

					for (int i = BytesNumWaiting4Process + BytesRecOnce; i < NetBuffer1.Length; i++) {
						curbuffer [i] = 0;
					}

					BytesNumWaiting4Process = 0;

					//ninfo 注意这里effectiveSizeInBuffer不是buf真实size，而是有效数据size
					ProcNetMessage(curbuffer, effectiveSizeInBuffer);
					//ninfo 这里说下两个buffer的故事，buf1，buf2，当buf1是当前，buf2就是下一个，然后翻转后，buf2就成了当前，这时buf1(使用前先清空)就是下一个
					bFlipBuffer  = !bFlipBuffer ;
				} else {
					Debug.LogError ("[严重bug]抛弃该消息，消息长度过长：" + BytesRecOnce + " nBufferCount=" + NET_BUFFER_SIZE);
				}

			} else {
				//this.Closed("Receive");
				Debug.LogError ("recive failed!");
			}
		} catch (Exception ex) {
			
			Debug.LogError ("数据读取异常：" + ex.Message + " == " + ex.ToString ());

		}

		SetupRecieveCallback ();
	}

	//ninfo 目测这段就是一段普通检测，应该是debug用的
	//这段逻辑的具体作用就是1找到消息头标志，2找到消息长度，3掠过这条消息的所有数据，4检测后面的数据是否是消息头标志，不是就报错
	//如果不报错就什么都不做，综上这段逻辑用处不大，应该用于debug	
	//ninfo 这里blen是一次接收到的size
	private void CheckBuffer (int bytesRecOnce){

		//by nafio 暂时屏蔽这段，需要再说
		if (true)
			return;

		MemoryStream ts = new MemoryStream (NetBuffer);
		BinaryReader br = new BinaryReader (ts, Encoding.UTF8);
		while (br.BaseStream.Position < bytesRecOnce) {
			int headmsg = BinaryHelper.ReadShort (br);
			if (headmsg != HEAD_MSG) {
				continue;//ninfo 难道会有第一个不是消息头的情况，从而不停continue下去？？
			}

			int msglen = BinaryHelper.ReadInt (br);
			int function_id = BinaryHelper.ReadInt (br);
			if (msglen > bytesRecOnce) {
				br.BaseStream.Position -= 10;
				break;
			}
			//ninfo 把整个数据段(除了前面读的10个)都掠过
			for (int i = 10; i < msglen; i++) {
				br.ReadByte ();
			}
			//ninfo 这段代码逗比啊，continue就从while开始执行，不满足while条件，从而跳出while，直接break不好么
			if (br.BaseStream.Position >= bytesRecOnce) {
				continue;
			}

			int hm = BinaryHelper.ReadShort (br);
			if (hm != HEAD_MSG) {
				Debug.LogError ("消息长度错误:" + function_id + " msglen=" + msglen + " blen=" + bytesRecOnce);
			}
			br.BaseStream.Position -= 2;
		}
	}

	//ninfo 注意这里nEnd不是buf真实size，而是buf中有效数据size

	//TODO 下一步，这个可以考虑移动到TcpNetMgr中
	// 处理网络消息
	public void ProcNetMessage (byte[] stream, int nEnd){

		int EffectiveByteNumInBuffer = nEnd;
		MemoryStream tstream = new MemoryStream (stream);
		BinaryReader _BinaryReader = new BinaryReader (tstream, Encoding.UTF8);

		tempBinaryReaderPos = 0;
		while (_BinaryReader.BaseStream.Position < EffectiveByteNumInBuffer) {
			//ninfo 因为一次解析至少要解析
			//判断剩下的字节够不够4个，如果小于4个就交给下一波来处理就行了
			long leftbytes = EffectiveByteNumInBuffer - _BinaryReader.BaseStream.Position;
			if (leftbytes < 6) {
				//如果小于4个字节，那么交给下个BUFFER
				byte[] thisbuffer = TcpNetMgr.GetInst().GetCurrentBuffer();
				byte[] nextbuffer = TcpNetMgr.GetInst().GetNextBuffer();
				for (int i = 0; i < leftbytes; i++) {
					nextbuffer [i] = thisbuffer [_BinaryReader.BaseStream.Position + i];
				}
				TcpNetMgr.GetInst().BytesNumWaiting4Process = (int)leftbytes;
				return;
			}

			int headmsg = BinaryHelper.ReadShort (_BinaryReader);
			if (TcpNetMgr.HEAD_MSG != headmsg) {
				Debug.Log ("严重错误");
				Debug.LogError ("message is woring");
				return;
			}
			int msglen = BinaryHelper.ReadInt (_BinaryReader);
			if (msglen <= 6) {

				//ninfo 解释下这里，首先按常理msglen不可能<=6 消息头+msglen两项就是6个字节
				//遇到<=6的情况，说明之前判断到的消息头headmsg=BEGINTAG_MSG实际是假的消息头，是脏数据
				//所以才掠过错误数据，找下一个（疑是）消息头
				//肯定是出错了,跳过这个消息，挪到下一个消息
				while (BinaryHelper.ReadShort(_BinaryReader) != TcpNetMgr.HEAD_MSG) {
					;
				}
				_BinaryReader.BaseStream.Position -= 2;
				tempBinaryReaderPos = (int)_BinaryReader.BaseStream.Position;
				continue;
			}

			leftbytes = EffectiveByteNumInBuffer - _BinaryReader.BaseStream.Position;
			//ninfo msglen-6就是一条消息出去消息头和msglen的有效消息内容的长度
			//这里的判断含义就是，剩余数据不够一条消息的有效数据长度
			if (leftbytes < msglen - 6) {
				byte[] thisbuffer = TcpNetMgr.GetInst().GetCurrentBuffer ();
				byte[] nextbuffer = TcpNetMgr.GetInst().GetNextBuffer ();
				for (int i = 0; i < leftbytes + 6; i++) {
					nextbuffer [i] = thisbuffer [_BinaryReader.BaseStream.Position + i - 6];
				}
				TcpNetMgr.GetInst().BytesNumWaiting4Process = (int)leftbytes + 6;
				return;
			}


			int function_id = BinaryHelper.ReadInt (_BinaryReader);

			EventID++;

			bool analyzeError = false;
			try {

				if (NetBackMgr.GetInst().NetBackAnalyzerDic.ContainsKey (function_id)) {
					Debug.LogError ("收到消息Messge ID = " + function_id);
					//MainAnalyzer[function_id]();
					Byte[] content = _BinaryReader.ReadBytes (msglen - 10);
					//ReceivedMessage(function_id, content);

					if (!NetBackMgr.CheckImportantMsg (function_id, content)) {
						Cache4RecMsg recevieMessage = new Cache4RecMsg ();
						recevieMessage.id = function_id;
						recevieMessage.content = content;
						NetBackMgr.Cache4RecMsgQueue.Enqueue (recevieMessage);
					}
				} else {
					Debug.Log ("Not Found Messge ID = " + function_id + ",msglen:" + msglen);
					for (int i = 10; i < msglen; i++)
						BinaryHelper.ReadByte (_BinaryReader);
				}
			} catch (Exception e) {
				analyzeError = true;
				//                    Debug.LogError("-------------CommandSys = " + function_id + " Read Error Start-------------");
				Debug.LogException (e);
				//                    Debug.LogError("-------------CommandSys = " + function_id + " Read Error End-------------");
			} finally {
				tempBinaryReaderPos += msglen;
				if (_BinaryReader.BaseStream.Position != tempBinaryReaderPos) {
					analyzeError = true;
					string ms = "消息处理函数读取数据错误!" + function_id.ToString ();
					Debug.LogError (ms);
					_BinaryReader.BaseStream.Position = tempBinaryReaderPos;
				}
			}//finally结束
		}//while结束
	}//函数结尾

	public bool bFlipBuffer  = true;
	public byte[] GetCurrentBuffer (){return bFlipBuffer  ? NetBuffer1 : NetBuffer2;}
	public byte[] GetNextBuffer (){return bFlipBuffer  ? NetBuffer2 : NetBuffer1;}

	#endregion

	#region send
	/// <summary>
	/// 是否消息正在发送中
	/// </summary>
	private bool BeMsgSending = false;
	/// <summary>
	/// 待发送消息队列
	/// 队列里存的是byte[] 类型的待发送消息数据
	/// </summary>
	private Queue SendMsgQueue = new Queue ();
	// 发送消息
	public bool VSendMsg (Cmd4Send iSendMessage){

		Stream s = iSendMessage.GetBuff ();
		s.Seek (0, SeekOrigin.Begin);

		int len = (int)s.Length;
		byte[] content = new byte[len];
		s.Seek (0, SeekOrigin.Begin);
		s.Read (content, 0, len);
		return Send (content);		
	}
		
	private bool Send (byte[] buff){
		if (!BeConnect ()) {
			Debug.Log ("Must be connected to Send a message");
			//已经断了，不用再关一次了吧？xuquansheng
			//this.Closed("send");

			return false;
		}
		lock (this) {
			try {

				if (BeMsgSending) {
					SendMsgQueue.Enqueue (buff);
				} else {
					BeMsgSending = true;
					try {
						sendMsgLen += buff.Length;
						Debug.LogError ("已发送数据：" + sendMsgLen / 1024 + "kb，合计：" + (RevMsgTotalSize + sendMsgLen) / 1024);
						_Stream.BeginWrite (buff, 0, buff.Length, new AsyncCallback (OnSend), null);
					} catch (Exception ex) {
						//14-01-02 jmn 掉线严重，去掉close，验证，添加catch信息
						//添加上传服务器信息					
						////14-01-02 jmn 添加解析消息ID号
						MemoryStream ts = new MemoryStream (buff);
						BinaryReader br = new BinaryReader (ts, Encoding.UTF8);
						BinaryHelper.ReadShort (br);
						BinaryHelper.ReadShort (br);
						int function_id = BinaryHelper.ReadInt (br);
						string msg = ">>>send exception, msg id:";
						msg += function_id;
						msg += ";ex: ";
						msg += ex.Message;

						////####
						//User32API.BugReport(msg, "jmn");
						//TTrace.p(">>>send exception:", msg);
					}
				}
			} catch (Exception) {
				return false;
			}
		}
		return true;

	}

	private double sendMsgLen = 0;
	private void OnSend (IAsyncResult ar){
		lock (this) {
			try {
				_Stream.EndWrite (ar);
				if (SendMsgQueue.Count > 0) {
					byte[] bData = (byte[])SendMsgQueue.Dequeue ();
					sendMsgLen += bData.Length;
					LogMgr.I("已发送数据：" + sendMsgLen / 1024 + "kb，合计：" + (RevMsgTotalSize + sendMsgLen) / 1024);
					_Stream.BeginWrite (bData, 0, bData.Length, new AsyncCallback (OnSend), null);
				} else {
					BeMsgSending = false;
				}
			} catch (Exception ex) {
				//14-01-09 jmn 掉线严重，去掉close，验证，添加catch信息
				//添加上传服务器信息					
				string msg = ">>>Onsend exception:";
				msg += ex.Message;
				////####
				//User32API.BugReport(msg, "jmn");
				//TTrace.p(">>>Onsend exception:", msg);
			}
		}
	}

	#endregion



}


//public class PingIP{
//	public static bool Ping (string ip){
//		using (var myPing = new System.Net.NetworkInformation.Ping ()) {
//			PingOptions op = new PingOptions ();
//			op.DontFragment = true;
//			string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
//			byte[] buff = ASCIIEncoding.ASCII.GetBytes (data);
//			PingReply PR = myPing.Send (ip, 200, buff, op);
//			if (PR.Status == IPStatus.Success) {
//				((IDisposable)myPing).Dispose ();
//				return true;
//			}
//			((IDisposable)myPing).Dispose ();
//			return false;
//		}
//	}
//}