using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
using ZLib;
using System.Reflection;

/// <summary>
/// 
/// 把从网络得到的原始二进制消息封装成cmd，并放入等待处理cmdQuene，等待处理
/// 
/// 具体参考demo类NetDouyiBack的写法
/// </summary>
public abstract class AbsNetBack{

	private NetBackMgr _NetBackMgr = null;

	public BinaryReader _BinaryReader;

	public AbsNetBack (){
		_NetBackMgr = NetBackMgr.GetInst ();
		RegistMsgID();
	}

	/// <summary>
	/// 注册监听回调方法
	/// </summary>
	public virtual int[] RegistMsg (){
		return new int[] { };
	}

	/// <summary>
	/// 注册监听回调方法
	/// </summary>
	private void RegistMsgID (){
		int[] msgs = RegistMsg ();
		Type t = GetType ();
		int id;
		for (int i = 0; i < msgs.Length; i++) {
			id = msgs [i];   
			MethodInfo info = t.GetMethod ("Recevie" + id);
			if (info == null) {
				Debug.LogError ("注册的消息号：" + id + "没有对应的处理方法:Recevie" + id + "();类名：" + t.ToString ());
				continue;
			}
			_NetBackMgr.RegistMsg (id, this);
		}
	}

	/// <summary>
	/// 注册消息
	/// </summary>
	/// <param name="cmd"></param>
	public void RegisterCommand (Cmd4Rec cmd){
		_NetBackMgr.RegisterCommand (cmd);
	}

}

