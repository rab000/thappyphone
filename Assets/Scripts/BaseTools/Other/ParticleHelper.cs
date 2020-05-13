using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHelper : MonoBehaviour {

	[SerializeField]private bool AutoDestroy = true;

	private ParticleSystem[] ps;

	private ParticleSystem[] PS{
		set{ 
			ps = value;
		}
		get{ 
			if(null==ps)ps = GetComponentsInChildren<ParticleSystem> ();
			return ps;
		}
	}

	private bool BePlayering = true;

	public void Play()
	{
		for (int i = 0; i < PS.Length; i++) 
		{
			PS [i].Play ();
		}

		BePlayering = true;

	}

	public void Stop()
	{
		for (int i = 0; i < PS.Length; i++) 
		{
			PS [i].Stop ();
		}
		BePlayering = false;

		RemoveEff (gameObject);

		//Debug.LogError ("特效name:"+transform.name+" Stop回池！");
		GoPoolMgr.GetIns().Put (transform.name, transform);
	}

	void Update()
	{

		if (!BePlayering)
			return;

		bool allStopped = true;

		int num = PS.Length;

		for (int i = 0;i<num;i++)
		{
			if (!PS[i].isStopped)
			{
				//Debug.LogError (PS[i].gameObject.name+"----原因不能回池");
				allStopped = false;
			}
		}

		if (allStopped) 
		{
			BePlayering = false;
			//Debug.LogError ("特效name:"+transform.name+" 自销毁回池！");
			GoPoolMgr.GetIns().Put (transform.name, transform);
		}
			
	}

	private static List<GameObject> ParticleGoList = new List<GameObject>();
	public static void AddEff(GameObject effGo)
	{
		if (!ParticleGoList.Contains (effGo)) 
		{
			ParticleGoList.Add (effGo);
		}

	}

	public static void RemoveEff(GameObject effGo)
	{
		if (!ParticleGoList.Contains (effGo)) 
		{
			ParticleGoList.Remove (effGo);
		}
	}

	public static List<GameObject> GetEffList()
	{
		return ParticleGoList;
	}

}
