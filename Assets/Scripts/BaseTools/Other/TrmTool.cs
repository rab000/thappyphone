using UnityEngine;

public class TrmTool{

	public static void ResetTrm(Transform trm)
	{
		trm.localPosition = Vector3.zero;
		trm.localEulerAngles = Vector3.zero;
		trm.localScale = Vector3.one;
	}
}
