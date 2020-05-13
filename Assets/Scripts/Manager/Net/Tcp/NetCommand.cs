using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;

/// <summary>
/// 用于接收的command
/// </summary>
public class Cmd4Rec{
	
	public int MsgID;
	public string Content;
	public DateTime _DataTime;
	public bool isLocked = false;
	public Cmd4Rec (){
		_DataTime = DateTime.Now;
	}
	/**
     * 再执行完函数之后，执行声音处理
    */
	public virtual void ProcSound (){
		//MusicManager.GetInst().PlaySoundWidthMessage(cmd);
	}
	public virtual void BeginExture (){
		if (fBeginFrame < 0) {
			fBeginFrame = Time.renderedFrameCount;
		}
	}
	/**
      * 判断是否超时
     */
	public int fDelayFrame = 1000;
	public int fBeginFrame = -1;

	public virtual bool bTimeOut (){
		if (fBeginFrame > 0) {
			if (Time.renderedFrameCount - fBeginFrame > fDelayFrame) {
				Debug.LogError ("消息" + GetType ().ToString () + " id =" + MsgID.ToString () + "执行超时!");
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// 处理消息返回数据
	/// </summary>
	public virtual void DataProcessing (){

	}

}


/// <summary>
/// 用于发送的command
/// </summary>
public class Cmd4Send{
	private Stream _stream = new MemoryStream ();
	private BinaryWriter _BinaryWriter;
	public Cmd4Send (){
		_BinaryWriter = new BinaryWriter (_stream, Encoding.UTF8);
		Add (TcpNetMgr.HEAD_MSG);	//写入消息头
		int shortlen = 10;
		Add (shortlen);					//写入消息长度，回头再改
	}
	public void Add (string field){BinaryHelper.WriteString (_BinaryWriter,field);}
	public void Add (short field){BinaryHelper.WriteShort (_BinaryWriter,field);}
	public void Add (byte field){BinaryHelper.WriteByte (_BinaryWriter,field);}
	public void Add (sbyte field){BinaryHelper.WriteSByte (_BinaryWriter,field);}
	public void Add (int field){BinaryHelper.WriteInt (_BinaryWriter,field);}
	public void Add (long field){BinaryHelper.WriteLong (_BinaryWriter,field);}
	public void Add (bool field){BinaryHelper.WriteBool (_BinaryWriter,field);}
	public void Add (float field){BinaryHelper.WriteFloat (_BinaryWriter,field);}
	private void CompLen (){
		int pl = (int)_stream.Length;
		_BinaryWriter.Seek (2, SeekOrigin.Begin);//(a,2,2);
		Add (pl);
		_BinaryWriter.Seek (pl, SeekOrigin.Begin);
	}
	public Stream GetBuff (){
		CompLen ();
		return _stream;
	}

}
