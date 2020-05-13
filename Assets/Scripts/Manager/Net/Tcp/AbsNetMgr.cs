using System.Collections.Generic;
public abstract class AbsNetMgr{
	// 连接
	public virtual bool VConnect (string ip, int port){return false;}
	// 断开连接
	public virtual bool VDisconnect (){return false;}
	// 发送消息
	public virtual bool VSendMsg (Cmd4Send iSendMessage){return false;}
	// tick
	public virtual void VTick (uint nLimitedCount){}
	// 释放
	public virtual void VRelease (){}

	public virtual void SetRevMsgCallback (Listener<Cmd4Rec> handleMessage){}

	/// <summary>
	/// 待处理的Cmd4Rec命令
	/// </summary>
	public static Queue<Cmd4Rec> Cmd4RecQueue = new Queue<Cmd4Rec> ();


}
