using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
/// <summary>
/// 目前设计用来存放可复用常用类型
/// </summary>
public class Utils {

	public static StringBuilder SB = new StringBuilder();

	public static void ClearSB()
	{
		SB.Remove (0, SB.Length);
	}

}
