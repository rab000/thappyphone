using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using NLog;
/// <summary>
/// 资源管理器
/// 负责管理资源版本更新，配表更新，首次进游戏资源下载
/// 资源下载，资源加载 启动www并返回
/// 资源解析www->bytes object
/// 资源内存存储，资源卸载，资源空闲对象池
/// 资源存储
/// 这个类主要负责调度，尽量减少具体功能，降低复杂度
/// 依赖类说明:1优先级队列，2文件保存功能
/// 
/// 为什么要使用引用计数
/// 首先这个情况应该发生在多个位置共用一个资源
/// 比如abcde位置
/// a位置不用了，不能卸载这个资源
/// b位置不用了，也不能卸载，所以要引用计数
/// 当没有资源引用时(既引用数为0时)
/// 
/// </summary>
public class ResMgr : SingletonBehaviour<ResMgr> {

	bool BeShowLog = true;

	#region 子功能类

	public LoadMgr loadMgr;

    #endregion

    #region new public

    public static ResEnum.ResLoadURL ResLoadPos = ResEnum.ResLoadURL.streamingAssets;

    #endregion

    #region bundle

    /// <summary>
    /// 资源加载时经常会遇到如下情况
    /// 比如技能加载，一个技能需要加载anim，bullet特效，eff特效3个资源(而且是不同类型资源，需要不同函数解析加载回的资源)
    /// 加载者需要知道技能加载完成的具体时机，但每个资源并非立刻加载，
    /// 这就需要等待计数机制，等待所有资源加载完成，通知加载者全部资源加载完成
    /// 
    /// 简单说就是，同时加载多资源，又需要知道加载完成时机的情况就用这个方法
    /// 
    /// 同时ResMgr只提供加载功能，具体资源使用，绑定需要调用者来实现，所以对于每个加载回来的资源需要不同的(调用者)回调函数进行资源绑定
    /// 
    /// </summary>
    /// <param name="resIDS">要加载的全部resID，可以是完全不同的资源类型</param>
    /// <param name="callback">每个resID对应的解析回调函数</param>
    /// <param name="allResLoadOver">全部资源加载完成后的回调</param>
    public void LoadAssetBundleGroup(string[] resIDS, Listener<string, ResSaveNode>[] callbacks,Listener allResLoadOver,byte priority = 0, bool bDependRes = false, ResEnum.ResUnloadTypeEnum unloadType = ResEnum.ResUnloadTypeEnum.RefCount)
    {

		if (BeShowLog)
		{
			for (int i = 0; i < resIDS.Length; i++) 
			{
				LogMgr.I ("ResMgr", "LoadAssetBundleGroup", "start load " +i+"->"+ resIDS[i], BeShowLog);
			}
		}
			
		int size = resIDS.Length;

		//对组里的每个资源加载成功回调做拦截，统计完成数量，然后调用callbacks由具体调用者完成资源绑定，一旦全部资源加载完，发送全部加载完回调
		int num = size;
        Listener<string, ResSaveNode> oneResLoadCallback = (resID,resNode)=>
		{
			Loom.QueueOnMainThread(()=>{
				num--;
				int j =0;
				for(j=0;j<size;j++){
					if(resID==resIDS[j])break;
				}
				callbacks[j](resID, resNode);
				if(num<=0)
				{
					allResLoadOver();
					LogMgr.I("ResMgr", "LoadAssetBundleGroup", "AssetBundleGroup LoadOver resID0 is " + resIDS[0], BeShowLog);
				}
			}); 
		};

        switch (ResLoadPos)
        {
            case ResEnum.ResLoadURL.resources:
                for (int i = 0; i < size; i++)
                {
                    loadMgr.LoadLocalAsset(resIDS[i], oneResLoadCallback);
                }
                break;
            case ResEnum.ResLoadURL.streamingAssets:
            case ResEnum.ResLoadURL.persistent:
                for (int i = 0; i < size; i++)
                {
                    loadMgr.LoadAssetBundle(resIDS[i], oneResLoadCallback, priority,bDependRes,unloadType);
                }
                break;
        }

    }

	/// <summary>
	/// 资源加载统一接口
	/// </summary>
	/// <param name="resID">资源ID</param>
	/// <param name="callback">回调</param>
	public void LoadBundle(string resid, Listener<string, ResSaveNode> callback, byte priority = 0, bool bDependRes = false, ResEnum.ResUnloadTypeEnum unloadType = ResEnum.ResUnloadTypeEnum.RefCount)
    {
		LogMgr.I ("ResMgr", "LoadAssetBundle", "start load " + resid, BeShowLog);

        switch (ResLoadPos)
        {
            case ResEnum.ResLoadURL.resources:
                loadMgr.LoadLocalAsset(resid, callback);
                break;
            case ResEnum.ResLoadURL.streamingAssets:
            case ResEnum.ResLoadURL.persistent:
                loadMgr.LoadAssetBundle(resid, callback, priority, bDependRes, unloadType);
                break;
        }

        
	}
	
    /// <summary>
    /// 中断加载
    /// </summary>
    /// <param name="resID">Res I.</param>
    public void InterraptLoad(string resID){
		if (LoadMgr.ResLoadingNodeDic.ContainsKey (resID)) 
		{
			LogMgr.I ("ResMgr", "InterraptLoad", "resID:" + resID + "中断加载", BeShowLog);
			loadMgr.InterraptLoad(resID);
		}
	}

	/// <summary>
	/// 按资源ID卸载资源
	/// </summary>
	/// <param name="resID">Res I.</param>
	public void UnLoad(string resID){
		LogMgr.I ("ResMgr", "UnLoad", "UnloadRes resID:" + resID, BeShowLog);
        ResSaveNode resNode = null;
        bool b = LoadMgr.ResSaveNodeDic.TryGetValue(resID, out resNode);
        if (b) LoadMgr.UnLoad(resNode);
        else
        {
            LogMgr.E("ResMgr","UnLoad", "LoadMgr.TResNodeDic找不到ResID:"+ resID+"卸载失败", BeShowLog);
        }
	}

    #endregion

    #region 辅助方法
    public void LoadBytes(string resID, Action<byte[]> callback, Action<string> OnError)
    {
        loadMgr.LoadBytes(resID, callback, OnError);
    }

    //直接加载某个asset资源
    public void LoadAsset(string xmlPath, Listener<string, ResSaveNode> callback)
    {
        if (LoadMgr.ResSaveNodeDic.ContainsKey(xmlPath))
        {
            if (null != callback)
                callback("t", LoadMgr.ResSaveNodeDic[xmlPath]);
            return;
        }

        StartCoroutine(LoadSingleRes(xmlPath, callback));
    }

    IEnumerator LoadSingleRes(string path, Listener<string, ResSaveNode> callback)
    {
        WWW www = new WWW(path);
        yield return www;

        if (null != www.error) Debug.Log("加载资源 www error:" + www.error);

        ResSaveNode resNode = LoadMgr.ResSaveNodePool.Get();
        resNode.ResID = path;
        
        resNode.TAssetBundle = www.assetBundle;
        UnityEngine.Object[] objs = resNode.TAssetBundle.LoadAllAssets();
        resNode.SetObjs(objs);
        resNode.SetObj(objs[0]);
        //resNode.assetsObjs = resNode.TAssetBundle.LoadAllAssets();
        //resNode.assetObj = resNode.assetsObjs[0];

        LoadMgr.ResSaveNodeDic.Add(resNode.ResID,resNode);

        if (null != callback)
            callback("t", resNode);

    }
    #endregion

    #region Res管理对内方法

    //xxx nres 需要改位置

    /// <summary>
    /// 资源信息索引总表
    /// </summary>
    private Dictionary<string,ResInfoFromXml> ResInfoIndexDic = new Dictionary<string,ResInfoFromXml>();

	public Dictionary<string,ResInfoFromXml> GetAllResInfo(){
		return ResInfoIndexDic;
	}

	public ResInfoFromXml GetResInfo(string resID){
		if (ResInfoIndexDic.ContainsKey (resID)) {
			return ResInfoIndexDic [resID];
		}
		return null;
	}
    //NTODO 下一步,这里没有载入技能表数据
	/// <summary>
	/// 载入资源总表
	/// </summary>
	private void LoadResTable(){

//		IoBuffer buffer = new IoBuffer ();
//		Action<byte[]> OnReadResInfoTable = (b) =>{
//
//			do{
//				if(null == b){
//					Debug.LogError("VerMgr.ProcessReadLocalResInfoTable获取本地资源总表为null");
//					break;
//				}
//
//				buffer.Clear();
//				buffer.PutBytes(b);
//				int num = buffer.GetInt();
//				//Log.v("VerMgr.ProcessReadLocalResInfoTable:读取到的总表 资源条数"+num);
//				ResInfo resInfo = null;
//				for(int i=0;i<num;i++){
//					resInfo = new ResInfo();
//					resInfo.ResID = buffer.GetString();
//					resInfo.ResUnloadType = (ResEnum.RES_UNLOAD_TYPE)buffer.GetByte();//TODO 这个信息总表可能不知道，可能是物品表控制
//					buffer.GetByte();//nafio todo 这里记得去掉
//					//resInfo.ResPackType = (ResEnum.RES_PACK_TYPE)buffer.GetByte();
//					resInfo.ResName = buffer.GetString();
//					//LogMgr.E("=============VerMgr.ProcessReadLocalResInfoTable:读取到的总表 resid["+i+"]="+resInfo.ResID+" 资源名:"+resInfo.ResName);
//					//TODO 这里随时添加其他可能的信息
//
//					int dependNum = buffer.GetInt();
//					if(dependNum>0){
//						resInfo.DependResIDList = new List<string>();
//						for(int j=0;j<num;j++){
//							string _dependResID = buffer.GetString();
//							resInfo.DependResIDList.Add(_dependResID);
//						}
//					}
//					//LogMgr.E("-------------VerMgr.ProcessReadLocalResInfoTable:读取到的总表 resid["+i+"]="+resInfo.ResID+" 资源名:"+resInfo.ResName);
//					ResInfoIndexDic.Add(resInfo.ResID,resInfo);
//				}
//
//
//			}while(false);
//		};
//
//		string url = "";
//		//xxx nres 需要改名称,暫時不改
//		url = ResEnum.StreamingAssetPath+ResEnum.TABLE+ResEnum.SYSTEM+"/apkresinfo.bytes";
//		FileHelper.GetIns().ReadBytesFromFile(url,OnReadResInfoTable,null);

		string meXmlPath = ResEnum.StreamingAssetPath+"/res/me/me";
		StartCoroutine (LoadXmls(meXmlPath,"me",()=>{
			string reXmlPath = ResEnum.StreamingAssetPath+"/res/re/re";
			StartCoroutine (LoadXmls(reXmlPath,"re",null));
		}));

	}

	//resType就是me，re等,加这个原因，看me.doc
	IEnumerator LoadXmls(string xmlPath,string resType,Listener callback)
	{
		WWW www = new WWW(xmlPath);
		yield return www;
		if(null != www.error)Debug.Log("加载xmlBundle www error:"+www.error);
		//这个名称永远固定，无论bundle名是什么，asset名都是这个
		AssetBundleManifest xml = (AssetBundleManifest)www.assetBundle.LoadAsset("assetbundlemanifest");
		string[] ss = xml.GetAllAssetBundlesWithVariant ();

		for (int i = 0; i < ss.Length; i++) 
		{
			//eg:i:0 assetName:map/mapname_t1/building.n
			//if(BeShowLog)Debug.LogError ("ResMgr.LoadXmls  i:"+i+" assetName:"+ss[i]);
			ResInfoFromXml resInfo = new ResInfoFromXml();
			
			Utils.SB.Append ("res/");
			Utils.SB.Append (resType);//这里要加me/,原因看me.doc,读取出来的路径是map/mapname_t1/building.n，添加完变成正确路径me/map/mapname_t1/building.n
			Utils.SB.Append ("/");
			Utils.SB.Append (ss [i]);
			resInfo.ResID = Utils.SB.ToString ();
            Utils.ClearSB();

            string[] dirDepend = xml.GetDirectDependencies (ss[i]);
			if (dirDepend.Length > 0) 
			{
				resInfo.DependResIDList = new List<string> ();
				resInfo.DependResIDList.AddRange (dirDepend);
			}
			ResInfoIndexDic.Add(resInfo.ResID,resInfo);
		}

		if (null != callback)
			callback ();
	}

	public void Init(){
		ResEnum.InitResConfig();//配置路径信息
		loadMgr = new LoadMgr();
		LoadResTable();
	}
	#endregion

	#region 资源管理主循环
	
	public void tUpdate ()
    {		
		if(null!=loadMgr)loadMgr.tUpdate();
	}
	#endregion
	
	#region 资源相关的工具方法

	/// <summary>
	/// 从Object[]中获取目标名称，类型的object
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="type">Type.</param>
	/// <param name="objs">Objs.</param>
	public static UnityEngine.Object GetTargetAsset(string name,System.Type type,UnityEngine.Object[] objs){
		int size = objs.Length;
		for(int i = 0; i < size; i++){

			//Debug.Log("TName----------------:"+objs[i].name+"   typ:"+objs[i].GetType());
			if(objs[i].name.Equals(name) && objs[i].GetType().Equals(type)){
				return objs[i];
			}
		}
		
		LogMgr.E("没有找到目标obj name:"+name+" type:"+type);
		return null;
	}

    public static void GC()
    {
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
    }

	#endregion

}

