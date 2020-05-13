using UnityEngine;
using System.Collections;
using NLog;
/// <summary>
/// Abs motion.
/// 输入开始，结束地点，运动速度
/// 目前的运动是初始构造，应该有不足的地方，以后随时考虑补充，暂时这样，没更好的办法
/// 
/// </summary>
public class AbsMotion {
	private bool BeShowLog = true;
	/// <summary>
	/// 运动中物体所在位置
	/// </summary>
	public Vector3 CurPos = new Vector3();

	protected Vector3 StartPos;

	protected Vector3 EndPos;

	protected float Speed;

	protected float MotionTotalTime;

	protected Timer _Timer;

	public bool BeStop = true;

	//运动类型
	protected byte MotionType;
	public const byte MOTION_TYPE_LINE = 0;
	public const byte MOTION_TYPE_CURVE = 1;

	public virtual void StartMove(Vector3 start,Vector3 end,float speed){
		StartPos = start;
		EndPos = end;
		Speed = speed;
		_Timer = TimerMgr.TimerPool.Get();

		float dis = Vector3.Distance(StartPos,EndPos);
		float lastTime = dis / Speed;
        LogMgr.I ("AbsMotion","StartMove","运动需要时间:"+lastTime,BeShowLog);
		_Timer.Start (lastTime);
		BeStop = false;
	}

//	public virtual void Clear(){
//		_Timer = null;
//	}

	/// <summary>
	/// 更新时返回当前位置
	/// </summary>
	/// <returns>The update.</returns>
	public void tUpdate(){
		
		float per = _Timer.GetLastTimePerventage();

		//计算位置
		CalculateCurPos(per);

		if (_Timer.IsOK ()) {
			Stop ();
		}
	}

	public void Stop(){
		_Timer.Stop();
        TimerMgr.TimerPool.Put (_Timer);
		_Timer = null;
		BeStop = true;
	}

	/// <summary>
	/// 计算运动物体当前位置
	/// </summary>
	/// <param name="time">0-1之间的浮点数</param>
	protected virtual void CalculateCurPos(float time){}

	public  Vector3 GetCurPos(){return CurPos;}

}
