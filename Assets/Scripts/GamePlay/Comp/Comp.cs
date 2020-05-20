using UnityEngine;
using System.Collections;
/// <summary>
/// Nafio Comp.
/// </summary>
public class Comp:EventDispather{

	public Comp(AbsSpr sprite){

		this.sprite = sprite;
	}

	public AbsSpr sprite;
	

	//AbsRootComp _rootComp;
	//public AbsRootComp rootComp{
	//	get{
	//		if(null == _rootComp){
	//			_rootComp = sprite.rootComp;
	//		}
	//		return _rootComp;
	//	}
	//}

	//TrmComp _trmComp;
	//public TrmComp trmComp{
	//	get{
	//		if(null == _trmComp){
	//			_trmComp = sprite.trmComp;
	//		}
	//		return _trmComp;
	//	}
	//}
		
}
