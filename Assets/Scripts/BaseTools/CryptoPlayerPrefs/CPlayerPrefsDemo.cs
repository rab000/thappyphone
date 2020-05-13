using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerPrefsDemo : MonoBehaviour {

	public static void SaveUserDataByPrefs(Class4Save userData)
	{
		PlayerPrefsExtend.Save("loginData", userData);
	}

	public static Class4Save LoadUserDataByPrefs()
	{
		return PlayerPrefsExtend.GetValue<Class4Save>("loginData");
	}


	public static void ClearUserDataByPrefs()
	{

		PlayerPrefs.SetInt("loading_mail", 0);
        //清理方式是新建一个新类，实际是清理了数据，而没有清理内存
		Class4Save o = new Class4Save();
		PlayerPrefsExtend.Clear("loginData", o);
	}
}
