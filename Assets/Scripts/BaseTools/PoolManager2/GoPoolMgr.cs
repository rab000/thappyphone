using NLog;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// go对象池
/// 暂时不加入定时减少和最大对象数的概念
/// </summary>
public class GoPoolMgr : SingletonBehaviour<GoPoolMgr> {

    private bool BeShowLog = false;

    //池中物体dic，string 池名称，List<Transform>具体go.transform
    private Dictionary<string,List<Transform>> poolGoDic = new Dictionary<string, List<Transform>> ();
   
    //存储不同池的prefab
    private Dictionary<string, Object> poolDic = new Dictionary<string, Object>();

    /// <summary>
    /// 创建池
    /// </summary>
    /// <param name="name"></param>
    /// <param name="prefab">使用者把要入池的prefab传进来，无论从哪个位置载入，池不负责载入，保持功能单一 </param>
    public void CreatePool(string name, Object prefab)
    {
        if (poolDic.ContainsKey(name))
        {
            LogMgr.I("GoPoolMgr", "CreatePool", "创建go池失败，重复创建 poolName:" + name, BeShowLog);
            return;
        }

        LogMgr.I("GoPoolMgr", "CreatePool", "创建go池成功 poolName:" + name, BeShowLog);
        poolDic.Add(name, prefab);
  
    }

	public void Put(string name,Transform trm)
	{

        LogMgr.I("GoPoolMgr","Put", "poolName:" + name + " trmName:" + trm.name,BeShowLog);

		trm.SetParent (transform);

		if (trm.gameObject.activeSelf)
			trm.gameObject.SetActive (false);

		if (!poolGoDic.ContainsKey (name))
			poolGoDic.Add (name,new List<Transform>());
		
		poolGoDic [name].Add (trm);


	}

	public Transform Get(string name)
	{

        Transform trm = null;

		if (poolGoDic.ContainsKey (name)) 
		{
            //池中有空闲对象的情况
			int num = poolGoDic [name].Count;
			if ( num > 0) 
			{
				trm = poolGoDic [name] [num - 1];
				trm.SetParent (null);
				poolGoDic [name].RemoveAt (num-1);                
                LogMgr.I("GoPoolMgr", "Get", "从池中取已存节点 poolName:" + name + " trmName:" + trm.name, BeShowLog);
                return trm;
			}
		}

        //池中没有空闲对象，准备创建新对象
        if (poolDic.ContainsKey(name))
        {         
            var go = Instantiate(poolDic[name]) as GameObject;
            trm = go.transform;
            LogMgr.I("GoPoolMgr", "Get", "从池中取新节点 poolName:" + name + " trmName:" + trm.name, BeShowLog);
            return trm;
        }

        LogMgr.E("GoPoolMgr", "Get", "取对象失败,无法创建没注册构造器的对象 poolName:" + name + " trmName:" + trm.name, BeShowLog);
        return null;

	}

    //清理池
    public void ClearAll()
    {
        int i = 0;

        int size = 0;

        foreach (var p in poolGoDic)
        {
            size = p.Value.Count;

            for (i = 0; i < size; i++)
            {
                Destroy(p.Value[i].gameObject);
            }
        }

        poolGoDic.Clear();

        poolDic.Clear();

        //ResMgr.GC();

        Resources.UnloadUnusedAssets();
        System.GC.Collect();
    }

}


