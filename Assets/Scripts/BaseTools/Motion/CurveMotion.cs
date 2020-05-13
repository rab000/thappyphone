using UnityEngine;
using System.Collections;
/// <summary>
/// 曲线运动
/// </summary>
public class CurveMotion : AbsMotion {

	private AnimationCurve YPosCurve;

	private AnimationCurve XZPosCurve;

	public CurveMotion(){
		
		MotionType = AbsMotion.MOTION_TYPE_CURVE;

		//抛物线
		YPosCurve = new AnimationCurve();
		YPosCurve.AddKey(0,0);
		YPosCurve.AddKey(0.5f,1f);
		YPosCurve.AddKey(1f,0f);

		//从0-1开始上升快，越到最后上升越慢，最终达到1的位置
		XZPosCurve = new AnimationCurve();
		XZPosCurve.AddKey(0,0);
		XZPosCurve.AddKey(0.5f,0.7f);
		XZPosCurve.AddKey(1,1);
	}

	protected override void CalculateCurPos (float time){

		float per = XZPosCurve.Evaluate(time);

		CurPos = Vector3.Lerp (StartPos,EndPos,per);

		//NAFIO 注意：这里得到的y是0-1的值，还需要乘角色跳起最大高度才行，这里默认最大高度时1，所以就不乘了
		float y = YPosCurve.Evaluate (time);

		//NAFIO 注意：曲线运动得y值变化是在开始点和结束点基础上变化的，想象下开始点和结束点之间存在坡度的情况会更容易明白这个问题
		CurPos.y = CurPos.y + y;
	}

}
