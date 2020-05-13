using UnityEngine;
using System.Collections;
/// <summary>
/// 直线运动
/// 
/// 讨论:
/// 当开始点和结束点之间有坡度时，直线运动怎么处理
/// 因为有贴地组件，当角色沿直线运动时，如果角色出现悬空，状态会被切换到悬空落地状态，运动实际会终止
/// 
/// 不过不是主角，是ai的话，没有贴地组件，直线运动时就要自行判断贴地了
/// 这种情况最好是不让ai发出直线技能，但具体什么条件ai不允许发出直线身体位移技能呢
/// 
/// //可以考虑有多种判断碰撞的方式，比如角色技能移动中连续判断，不使用动画某个点判断的方式
/// </summary>
public class LineMotion : AbsMotion {

	private AnimationCurve XZPosCurve;

	public LineMotion(){
		MotionType = AbsMotion.MOTION_TYPE_LINE;
	}

	public override void StartMove(Vector3 start,Vector3 end,float speed){
		base.StartMove (start,end,speed);
	}

	protected override void CalculateCurPos (float time){
		float per = XZPosCurve.Evaluate(time);
		CurPos = Vector3.Lerp (StartPos,EndPos,per);
	}

}
