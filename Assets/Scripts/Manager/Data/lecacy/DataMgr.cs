using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 统一管理数据表
/// by nafio 18.12.06
/// </summary>
public class DataMgr {

	private static bool BeShowLog = false;

	public const string DEFAULT_SUBKEY = "unique";

	private static Dictionary<string,Dictionary<string,BaseData>> DatasDic = new Dictionary<string,Dictionary<string,BaseData>>();


	public static bool BeDataExist(string key,string subkey){

		if (DatasDic.ContainsKey (key)) {
			if (DatasDic [key].ContainsKey (subkey)) {
				return true;
			}
		}

		return false;

	}

	public static T GetData<T>(string key,string subkey=DEFAULT_SUBKEY)  where T : class
	{
		if (DatasDic.ContainsKey (key))
		{
			if (DatasDic [key].ContainsKey (subkey)) 
			{
				return (T)Convert.ChangeType(DatasDic[key][subkey], typeof(T));
			}

			Debug.LogError ("DataMgr.GetData cant find data subkey:"+key+" subkey:"+subkey);
		}

		Debug.LogError ("DataMgr.GetData cant find data key:"+key); 

		return null;

	}

	public static bool GetData<T>(string key,string subkey,out T t)  where T : class
	{
		if (DatasDic.ContainsKey (key))
		{
			if (DatasDic [key].ContainsKey (subkey)) 
			{

				t = (T)Convert.ChangeType(DatasDic[key][subkey], typeof(T));

				return true;
			}

			if(BeShowLog)Debug.LogError ("DataMgr.GetData cant find data subkey:"+key+" subkey:"+subkey);
		}

		if(BeShowLog)Debug.LogError ("DataMgr.GetData cant find data key:"+key+" subkey:"+subkey); 

		t = null;

		return false;

	}


	public static void PutData(string key,BaseData data,string subkey = DEFAULT_SUBKEY)
	{
		if (DatasDic.ContainsKey (key)) 
		{
			if (!DatasDic [key].ContainsKey (subkey)) 
			{
				DatasDic [key].Add (subkey, data);
			}
			else 
			{
				Debug.LogError ("DataMgr.PutData cant put data  key:"+key+" subkey:"+subkey+" exist!!!!!"); 
			}
		}
		else
		{
			
			DatasDic.Add (key,new Dictionary<string,BaseData>());

			DatasDic [key].Add (subkey, data);

		}

	}

	public static void RemoveData(string key,string subkey)
	{
		if (!BeDataExist (key, subkey))
		{
			Debug.LogError ("DataMgr.RemoveData removeData Faile!!!  key:"+key+" subkey:"+subkey+" not exist!!!!!"); 

			return;
		}
			
		//nafio info 这里只清除subkey key因为一直要用，没有清理需求
		DatasDic [key].Remove (subkey);

	}


	public static void Init()
	{
		//nafio info 这个用来临时记录，用户操作数据，与新生成块数据，发送给服务器后就暂时失效了
		//这个暂时要手动设置下roomid再发送
		//new RoomUpdateData ("room_update", "4send");

		//nafio info 这个用来临时记录，用户操作后，新地图的数据，发送给服务器后就暂时失效了
		//new MapData ("map","new");


	}




}
