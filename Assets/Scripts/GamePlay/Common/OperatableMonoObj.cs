using UnityEngine;
using System.Collections;
/// <summary>
/// 可被鼠标选中mono物体
/// </summary>
public class OperatableMonoObj : MonoBehaviour {
	
	public virtual void OnFocus(){}
	public virtual void OnLostFocus(){}
	public virtual void OnSel(){}
	public virtual void OnDeSel(){}
	/// <summary>
	/// click处理
	/// 这个不一定需要实现，看需求
	/// 比如npc商店等，sel时就可以直接打开window
	/// </summary>
	public virtual void OnClick(){}

}
