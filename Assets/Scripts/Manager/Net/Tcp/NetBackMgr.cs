using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using UnityEngine;
using ZLib;

/// <summary>
/// 未经解析的二进制msg
/// </summary>
public class Cache4RecMsg  {
	public int id;
	public byte[] content;
}

/// <summary>
/// 接收的消息处理都注册在这里
/// 
/// 消息接收过程说明:
/// 1 缓存网络数据
/// 得到bytes，把bytes缓存到Cache4RecMsg(包含msgid，byte[])，并存放到NetBackMgr.receiveQueue中
/// 
/// 2 从缓存网络数据解析Cmd并保存(通过Tick.Update查询并执行) 
/// 在Tick循环中通过NetBackMgr.ReceiveMessages解析NetBackMgr.receiveQueue，
/// 具体的解析过程是，通过NetBackMgr.MainAnalyzer中存储的各个具体AbsNetBack.Receivexxx方法来生成具体Cmd4Rec，并把Cmd4Rec存入到AbsNetMgr.Cmd4RecQueue
/// 
/// 3 从Cmd队列中取Cmd，执行收到消息后的逻辑处理
/// 在TcpNetWorkMgr.VTick中，逐个取出AbsNetMgr.Cmd4RecQueue中的cmd，通过TcpNetMgr.RecMsgCallback这个委托，把当前要处理的cmd发送给关注这个委托的观察者
/// 观察者通过类似switch(cmd.id)的形式，逐级发给各个子逻辑
/// 
/// </summary>
public class NetBackMgr{

	#region var

	/// <summary>
	/// 
	/// NetBack解析方法dic
	/// 
	/// NINFO注意:这里存的内容并不是所有的具体AbsNetBack，而是所有AbsNetBack包含的所有消息解析方法(AbsNetBack.RecieveXXXX)
	/// 
	/// eg:以具体AbsNetBack  NetDouYiBack为例
	/// NetDouYiBack向MainAnalyzer存放的所有数据为
	/// 18002  NetDouYiBack
	/// 18004  NetDouYiBack
	/// 18006  NetDouYiBack
	/// 18008  NetDouYiBack
	/// 18010  NetDouYiBack
	/// 
	/// </summary>
	public Dictionary<int, AbsNetBack> NetBackAnalyzerDic = new Dictionary<int, AbsNetBack> ();

	public static SafeQueue<Cache4RecMsg> Cache4RecMsgQueue = new SafeQueue<Cache4RecMsg>();

	#endregion

	#region 单例
	private static NetBackMgr Ins = null;

	public static NetBackMgr GetInst (){
		if (Ins == null) {
			Ins = new NetBackMgr ();

			//TODO Tick目前并没有注册到主工程，以后用到Net模块要记得处理这步，直接把Tick的主循环放到GameMgr中貌似就可以
			Tick.AddUpdate (NetBackMgr.AnalysisImportantCache4RecMsg);
			//轮询接收消息
			Tick.AddUpdate (NetBackMgr.AnalysisCache4RecMsg);
		}
		return Ins;
	}
	#endregion

	#region regist
	/// <summary>
	/// 注册所有NetBack
	/// </summary>
	public void RegistNetBack (){
		//new NetLoginBack();
		//new NetMountBack();
		//new NetDropItemBack();
		new NetDouYiBack();
	}

	/// <summary>
	/// 注册要接收的消息
	/// 具体的一个Netback可以含有有多条消息，Netback初始化时，要把自身所有消息注册到这里
	/// 
	/// </summary>
	/// <param name="msgID">Message I.</param>
	/// <param name="back">Back.</param>
	public void RegistMsg (int msgID, AbsNetBack back){
		if (NetBackAnalyzerDic.ContainsKey (msgID)) {
			NetBackAnalyzerDic [msgID] = back;
		} else {
			NetBackAnalyzerDic.Add (msgID, back);
		}
	}

	/// <summary>
	/// 注册Cmd4Rec到AbsNetMgr.Cmd4RecQueue
	/// AbsNetMgr.Cmd4RecQueue队列在主循环解析时就会执行具体消息返回逻辑
	/// </summary>
	/// <param name="cmd">Cmd.</param>
	public void RegisterCommand (Cmd4Rec cmd){
		if (cmd == null)return;
		AbsNetMgr.Cmd4RecQueue.Enqueue(cmd);
	}

	#endregion




	#region important msg

	//重要消息数组（重要消息立即执行，不队列）
	private static readonly int[] importantMessageList = new int[] {
		//主角同步相关
		11106,
		11012,
		11112
		//切场景相关
		//XXXXXX  //客户端发送请求切换场景后，如有需要则服务器返回确认提示.以后如果有这个消息则请添加到此处.
		//10010,//服务器返回地图ID、地图地址和主角坐标
		//11208,//服务器切换视野成功消息 客户端发送请求视野11011之后的返回消息
			
		//主角同步相关
		//11112//同步角色位置(目前只应用于主角)
	};

	/// <summary>
	/// 需要把前序消息全部立即执行的消息 !!!这个消息也必须在importantMessageList里面
	/// </summary>
	private static readonly int[] isNeedExecuteAllList = new int[] {

	};




	/// <summary>
	/// 存Action到队列中..
	/// </summary>
	private static SafeQueue<Action> queue = new SafeQueue<Action> ();

	private static bool IsNeedImneduately = false;

	//检测是否是重要消息
	private static bool isImportantMessage (int __msgCode){
		return (Array.IndexOf (importantMessageList, __msgCode) != -1);
	}

	/// <summary>
	/// 检查一个消息是否是需要前序消息都执行完的消息 !!!!!!!这个消息必须也是重要消息
	/// </summary>
	/// <param name="__msgCode"></param>
	/// <returns></returns>
	private static bool isNeedExecuteAll (int __msgCode){
		return (Array.IndexOf (isNeedExecuteAllList, __msgCode) != -1);
	}

	/// <summary>
	/// 检测是否是重要消息
	/// 如果是重要消息，则不进队列，优先执行
	/// 如果是特别重要消息（别入切场景消息），则一次性执行掉所有队列中的消息
	/// </summary>
	/// <param name="__msgCode">消息号</param>
	/// <param name="__data">消息数据</param>
	/// <returns>如果是重要消息，则返回true, 否则返回false</returns>
	public static bool CheckImportantMsg (int __msgCode, byte[] __data){
		//如果是重要消息则立即执行
		if (isImportantMessage (__msgCode)) {
			if (isNeedExecuteAll (__msgCode)) {
				IsNeedImneduately = true;
			}
			//异步线程,存起来主线程处理
			Action work = () => AnalysisCache4RecMsg (__msgCode, __data);

			queue.Enqueue (work);
			//立即执行
			return true;
		}
		return false;
	}

	#endregion


	#region analysis Cache4RecMsg

	/// <summary>
	/// 解析重要消息
	/// </summary>
	public static void AnalysisImportantCache4RecMsg (){
		//需要立即执行的情况
		if (IsNeedImneduately) {
			while (Cache4RecMsgQueue.Count > 0) {
				//获取第一个消息
				Cache4RecMsg message = Cache4RecMsgQueue.Dequeue ();
				//处理消息
				AnalysisCache4RecMsg (message.id, message.content);
			}

			IsNeedImneduately = false;
		}
		//处理异步线程的work
		while (queue.Count > 0)
			queue.Dequeue () ();

	}

	/// <summary>
	/// 轮询接收消息
	/// </summary>
	public static void AnalysisCache4RecMsg(){
		float time = Time.time;

		while (Cache4RecMsgQueue.Count > 0) {
			//获取第一个消息
			Cache4RecMsg message = Cache4RecMsgQueue.Dequeue ();
			//处理消息
			AnalysisCache4RecMsg (message.id, message.content);

			//时间判断（消息处理要均匀的分布在刷帧中）
			if (Time.time - time >= 5)
				break;
		}

	}
		
	/// <summary>
	/// 收到消息
	/// </summary>
	/// <param name="__msgCode">消息号</param>
	/// <param name="__msgContent">消息体</param>
	private static void AnalysisCache4RecMsg (int __msgCode, byte[] __msgContent){

		if (NetBackMgr.GetInst ().NetBackAnalyzerDic.ContainsKey (__msgCode)) {
			AbsNetBack back = NetBackMgr.GetInst ().NetBackAnalyzerDic [__msgCode];

			MemoryStream tstream = new MemoryStream (__msgContent);
			back._BinaryReader = new BinaryReader (tstream, Encoding.UTF8);
			Type t = back.GetType ();
			MethodInfo info = t.GetMethod ("Recevie" + __msgCode);
			info.Invoke (back, null);
			back._BinaryReader.Close ();
			tstream.Close ();
			tstream.Dispose ();
		} else {
			Debug.LogError ("无法解析Socket包：" + __msgCode);
		}
	}

	#endregion 
}
	

