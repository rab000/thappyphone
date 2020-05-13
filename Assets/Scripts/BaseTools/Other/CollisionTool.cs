using UnityEngine;
using System.Collections;
/// <summary>
/// 碰撞检测工具
/// </summary>
public class CollisionTool{

	/// <summary>
	/// 2D平行于轴的Rect与point的碰撞检测
	/// 这个目前用于远程子弹的碰撞检测，效率最高
	/// 配合场景分区，角色视野，阵营判定，达到最大效率
	/// </summary>
	/// <returns><c>true</c>, if d rect2 d was point2ed, <c>false</c> otherwise.</returns>
	/// <param name="px">Px.</param>
	/// <param name="py">Py.</param>
	/// <param name="rectX">Rect x.</param>
	/// <param name="rectY">Rect y.</param>
	/// <param name="rectW">Rect w.</param>
	/// <param name="rectH">Rect h.</param>
	public static bool Point2D_BaseRect2D(float px,float py,float rectX,float rectY,float rectW,float rectH)
	{
		if (px < rectX)return false;
		if (py < rectY)return false;
		if (px > rectX + rectW)return false;
		if (py > rectY + rectH)return false;
		return true;
	}

	/// <summary>
	/// 可旋转Rect2D判定
	/// </summary>
	/// <returns><c>true</c>, if d rect2 d was point2ed, <c>false</c> otherwise.</returns>
	public static bool Point2D_Rect2D(){
		return false;
	}

	/// <summary>
	/// 2D扇形与2D点碰撞检测
	/// (x－a)²+(y－b)²=r²
	/// </summary>
	/// <returns><c>true</c>, if d sector2 d was point2ed, <c>false</c> otherwise.</returns>
	/// <param name="targetV">目标点坐标</param>
	/// <param name="sectorV">扇形中心点坐标</param>
	/// <param name="sectorDir">扇形中心线朝向</param>
	/// <param name="sectorAngle">扇形张开角度(1/2,比如张开是60度，这里就是30)</param>
	/// <param name="sectorR">扇形半径</param>
	public static bool Point2D_Sector2D(Vector2 targetV,Vector2 sectorV,Vector2 sectorDir,int sectorAngle,float sectorR)
	{
		//判定距离,以点targetV.x画一条垂直于x轴的线，如果跟圆方程有两个解就是圆内部，反之圆外部
		//根据圆方程，r2-x2如果大于等于0说明y有两个值，反之没值，点不在圆内
		float x2 = sectorV.x - targetV.x;
		x2 = x2 * x2;
		float r2 = sectorR * sectorR;
		if (r2 - x2 < 0)return false;
			
//		float dis = Vector2.Distance(targetV,sectorV);
//		if (dis > sectorR * sectorR)
//			return false;

		//判定角度
		Vector2 vSector2Target = targetV - sectorV;
		vSector2Target.Normalize();
		float n = Vector2.Dot(vSector2Target,sectorDir);
		if (n > MathsTool.Cos (sectorAngle))
			return false;
		
		return true;
	}

	/// <summary>
	/// 2D圆形区域与2D点碰撞
	/// </summary>
	/// <returns><c>true</c>, if d circle2 d was point2ed, <c>false</c> otherwise.</returns>
	/// <param name="targetV">Target v.</param>
	/// <param name="sectorV">Sector v.</param>
	/// <param name="sectorR">Sector r.</param>
	public static bool Point2D_Circle2D(Vector2 targetV,Vector2 sectorV,float sectorR)
	{
		float x2 = sectorV.x - targetV.x;
		x2 = x2 * x2;
		float r2 = sectorR * sectorR;
		if (r2 - x2 < 0)return false;

		return true;

//		float dis = Vector2.Distance(targetV,sectorV);
//		if (dis > sectorR * sectorR)
//			return false;
//		return true;
	}



}
