using UnityEngine;
using n.tools;
using System;
using System.Collections.Generic;
using NLog;

/// <summary>
/// 资源加载器
/// 负责资源导入，解析，资源节点保存，资源释放，延迟释放
/// </summary>
public class LoadMgr{

	private static bool BeShowLog = true;
	
	#region 内存表
	/// <summary>
	/// 加载中资源存储表
	/// 重要Dic用来存储下载中，加载中，加载后的ResNode，包括从本地和服务器动态下载的资源
	/// 一些没必要存的东西，加载到后可以手动从这个表里清理掉，比如下载后的bytes,因为
	/// </summary>
	public static Dictionary<string,ResLoadingNode> ResLoadingNodeDic = new Dictionary<string,ResLoadingNode>();

	/// <summary>
	/// 加载中空闲资源节点池
	/// </summary>
	public static ClassPool<ResLoadingNode> ResLoadingNodePool = new ClassPool<ResLoadingNode>();

    /// <summary>
    /// 已加载资源存储表
    /// </summary>
    public static Dictionary<string, ResSaveNode> ResSaveNodeDic = new Dictionary<string, ResSaveNode>();

    /// <summary>
    /// 已加载资源空闲节点池
    /// </summary>
    public static ClassPool<ResSaveNode> ResSaveNodePool = new ClassPool<ResSaveNode>();

    /// <summary>
    /// 延迟释放列表
    /// </summary>
    public static List<ResSaveNode> DelayUnloadResList = new List<ResSaveNode>();

    #endregion


    #region 流程表
    /// <summary>
    /// 最大同时载入数
    /// </summary>
    private const int MAX_LOAD_NUM = 10;

	/// <summary>
	/// 等待加载优先级队列
	/// </summary>
	private PriorityQueue<ResLoadingNode> WaitLoadPriorityQue = new PriorityQueue<ResLoadingNode>(1024,new BaseNodeCom());

	/// <summary>
	/// 加载中列表
	/// </summary>
	private List<ResLoadingNode> LoadingList = new List<ResLoadingNode>();

	/// <summary>
	/// 加载完列表
	/// </summary>
	private List<ResLoadingNode> LoadOverList = new List<ResLoadingNode>();

	/// <summary>
	/// 加载失败列表
	/// </summary>
	private List<ResLoadingNode> LoadErrorList = new List<ResLoadingNode>();
    #endregion


    #region 外部方法

    public void LoadLocalAsset(string id, Listener<string, ResSaveNode> callback, byte priority = 0, bool bDependRes = false, ResEnum.ResUnloadTypeEnum unloadType = ResEnum.ResUnloadTypeEnum.RefCount)
    {
        ResSaveNode saveNode = ResSaveNodePool.Get();
        saveNode.ResID = id;
        saveNode.ResLoadURL = ResEnum.ResLoadURL.resources;
        saveNode.SetObj(Resources.Load(id));
        ResSaveNodeDic.Add(id,saveNode);
        callback?.Invoke(id,saveNode);
        //这里用完没回池
    }

    /// <summary>
    /// 载入Assetbundle类型资源
    /// </summary>
    /// <param name="resid">Identifier.</param>
    /// <param name="callback">Callback.</param>
    /// <param name="priority">Priority.</param>
    public void LoadAssetBundle(string resid, Listener<string, ResSaveNode> callback,byte priority = ResEnum.Load_Priority_Normarl,bool bDependRes = false,ResEnum.ResUnloadTypeEnum unloadType = ResEnum.ResUnloadTypeEnum.RefCount){

        //查找是否已经加载过
        ResSaveNode resNode = null;
        if (ResSaveNodeDic.ContainsKey(resid))
        {
            resNode = ResSaveNodeDic[resid];
            LogMgr.I("LoadMgr", "LoadAssetBundle", "当前TResNodeDic中包含ID为" + resid + "的资源，不进行重复加载，直接返回", BeShowLog);
            callback?.Invoke(resid, resNode);
            //依赖资源加载时就加引用计数，其他资源用户使用时才加引用计数，使用时再增加依赖资源引用
            //if(bDependRes) resNode.AddRefCount();
            return;
        }

        //判断是否在加载中
        ResLoadingNode resLoadingNode = null;
		if (ResLoadingNodeDic.ContainsKey (resid))
		{

			LogMgr.I("LoadMgr","LoadAssetBundle","当前ResNodeDic中包含ID为" + resid + "的资源，不进行重复加载，直接返回",BeShowLog);

			resLoadingNode = ResLoadingNodeDic [resid];

			if (null == resLoadingNode)
				LogMgr.E ("LoadMgr","LoadAssetBundle","abNode=null!",BeShowLog);

			switch (resLoadingNode.LoadStep)
            {			
			case ResEnum.ResLoadingStepEnum.Wait:
			case ResEnum.ResLoadingStepEnum.Loading:
                resLoadingNode.CallBackList.Add (callback);
				return;
			default:				
               LogMgr.E("LoadMgr", "LoadAssetBundle", "nodeState:" + resLoadingNode.LoadStep, BeShowLog);
               break;
			}
		}
		else
		{
			LogMgr.I("LoadMgr","LoadAssetBundle","当前ResNodeDic中不包含ID为" + resid + "的资源，准备开始一次新的加载",BeShowLog);
		}

        //开始加载
		//获取资源信息（从资源总表获取）
		ResInfoFromXml resInfo = ResMgr.GetIns().GetResInfo(resid);

        if (resInfo == null)
        {
			LogMgr.E ("LoadMgr","LoadAssetBundle","资源索引表中不存在ID为" + resid + "的资源，停止加载",BeShowLog);			
			return;
		}

        //这里存在的意义可能是为了保留tUpdate中最后处理loadOverNode的流程不被破坏 
		//www回调函数，www解析函数，主要任务时解析资源，回调不在这里调用，在循环中回调
		Action<ResLoadingNode> loadSuccess = (node)=>{
			LogMgr.I("LoadMgr","LoadAssetBundle","返回wwwOk id:"+node.ResID,BeShowLog);
            //node.DecodeAsset2ResNode();//解析出asset或assets保存到ResNode中
            //nafio info 特殊说明，升级到unity2017后，assetbundle.loadAsset后不能立刻unloadBoundle，否则异常，所以这里用协程做了延迟处理
            //ThreadManager.StartCoroutine(AfterLoad(node));
            AfterLoad(node);

        };

		Action<string> loadFaile = (s)=>{
			LogMgr.I("LoadMgr","LoadAssetBundle","返回wwwOk error:"+s,BeShowLog);
		};
			

		resLoadingNode = ResLoadingNodePool.Get();

        //如果有依赖，优先先加载依赖资源，具体方法就是依赖资源优先级高，普通资源优先级低，所以依赖资源一定会在普通资源之前加载
        if (null != resInfo.DependResIDList && resInfo.DependResIDList.Count > 0)
        {
            LogMgr.I("LoadMgr", "LoadAssetBundle", "开始加载资源resid:" + resid + "的依赖资源", BeShowLog);
            int dependResNum = resInfo.DependResIDList.Count;
            for (int i = 0; i < dependResNum; i++)
            {
                string _dependResID = resInfo.DependResIDList[i];
                //这里意思是被依赖资源优先级永远高于依赖资源(可能存在多级依赖)
                byte _priorty = (byte)(priority + 1);
                LoadAssetBundle(_dependResID, null, _priorty, true);//之所以为null是因为，依赖资源不需要回调，加载完安静躺那就ok
            }

            //添加AB节点加载(依赖资源)信息
            resLoadingNode.DependResArray = new string[resInfo.DependResIDList.Count];

            resInfo.DependResIDList.CopyTo(resLoadingNode.DependResArray);
        }

        //填充AB节点加载信息
        resLoadingNode.CallBackList.Add(callback);
		resLoadingNode.SetCallback(loadSuccess,loadFaile);
        //Debug.LogError("加入资源到加载中节点时的resid:"+ resid);
		resLoadingNode.ResID = resid;
        //resLoadingNode.RefCountInLoading = 0;
        resLoadingNode.BeDependRes = bDependRes;
        resLoadingNode.UnloadType = unloadType;
        resLoadingNode.ResLoadURL = ResMgr.ResLoadPos;
        //resLoadingNode.PackType = resInfo.ResPackType;
        resLoadingNode.LoadStep = ResEnum.ResLoadingStepEnum.Wait;
		resLoadingNode.Priority = priority;//设置优先级，可以解决依赖资源问题

		//加入等待队列
		WaitLoadPriorityQue.Push(resLoadingNode);

		ResLoadingNodeDic.Add(resid,resLoadingNode);
	}

	private void AfterLoad(ResLoadingNode node)
	{
		node.LoadStep = ResEnum.ResLoadingStepEnum.Loadover;//已经在node索引表中了就不需要再加入了，只需要改状态
		node.CallBack();//回调给调用者去解析具体asset
	}

    public void LoadBytes(string id, Action<byte[]> callback, Action<string> OnError)
    {
        string _path = ResEnum.GetUrlByResID(id);
        
        FileHelper.GetIns().ReadBytesFromApkFile(_path, callback, OnError);
    }

    /// <summary>
    /// 中断加载
    /// </summary>
    /// <param name="resID">Res I.</param>
    public void InterraptLoad(string resID){
		ResLoadingNode assetBundleNode = ResLoadingNodeDic[resID];
		switch(assetBundleNode.LoadStep){
		case ResEnum.ResLoadingStepEnum.Wait:

			assetBundleNode.LoadStep = ResEnum.ResLoadingStepEnum.Interrapt;
			ResLoadingNodeDic.Remove(assetBundleNode.ResID);
			
			break;
		case ResEnum.ResLoadingStepEnum.Loading:
			LoadingList.Remove(assetBundleNode);
			ResLoadingNodeDic.Remove(assetBundleNode.ResID);
			//assetBundleNode.ReduceRefCount();
			
			
			break;
		case ResEnum.ResLoadingStepEnum.Loadover:
			LoadOverList.Remove(assetBundleNode);
			ResLoadingNodeDic.Remove(assetBundleNode.ResID);
			//assetBundleNode.ReduceRefCount();
			
			break;
		}
	}
		
	#endregion

	#region 加载流程
	public void tUpdate(){


		ResLoadingNode _node = null;
		
		//处理待加载表
		if(WaitLoadPriorityQue.Count>0 && LoadingList.Count < MAX_LOAD_NUM){
			_node = WaitLoadPriorityQue.Pop();
			if(_node.LoadStep == ResEnum.ResLoadingStepEnum.Interrapt){
				//_node.ReduceRefCount();
				
			}else{
				_node.LoadStep = ResEnum.ResLoadingStepEnum.Loading;
				LoadingList.Add(_node);
				_node.Load();
			}
		}
		
		//处理加载中表
		int i = 0;
		int size = LoadingList.Count;
		for(i=0;i<size;i++){
			_node = LoadingList[i];
			switch(_node.LoadState){
			case ResLoadingNode.ResLoadStateEnum.Error:
				if (_node.LoadStep == ResEnum.ResLoadingStepEnum.Loading) {
					_node.LoadStep = ResEnum.ResLoadingStepEnum.Error;
					LogMgr.E ("LoadMgr", "tUpdate", "res" + _node.ResID + " wwwStateError", BeShowLog);
					LoadErrorList.Add(_node);
				}

				break;
			case ResLoadingNode.ResLoadStateEnum.Done:
				if (_node.LoadStep == ResEnum.ResLoadingStepEnum.Loading) {
					LogMgr.I ("LoadMgr", "tUpdate", "res" + _node.ResID + " wwwStateDone", BeShowLog);
					_node.LoadStep = ResEnum.ResLoadingStepEnum.Loadover;
					LoadOverList.Add(_node);
				}
				break;
			}
		}

        //加载失败处理，返回加载失败

		//检测加载完表
		size = LoadOverList.Count;
		for(i=0;i<size;i++)
        {
			_node = LoadOverList[i];
			LoadingList.Remove(_node);//清理加载中列表
            //把TResNode回调给用户
           
			_node.CallbackSuccess();           
            LoadMgr.ResLoadingNodeDic.Remove(_node.ResID);
            _node.Clear(); //clear的时机不对，上面CallbackSuccess中有携程              
            ResLoadingNodePool.Put(_node);
            
        }

        //加载完表已经处理完，全部节点出表
        if (size > 0)
        {
            LoadOverList.Clear();
        }
        
	}
    #endregion

    #region operate ResNode

    /// <summary>
    /// 引用卸载
    /// </summary>
    /// <param name="resNode"></param>
    public static void UnLoad(ResSaveNode resNode)
    {
        ResEnum.ResLoadURL resLoadUrl = resNode.ResLoadURL;
       
        if (resNode.RefCount <= 0)
        {
            LogMgr.E("LoadMgr","UnLoad","resNode.RefCount:"+ resNode.RefCount+" unload faile"+" resID:"+ resNode.ResID, true);
            return;
        }

        resNode.ReduceRefCount();

        if (resNode.RefCount > 0)
        {
            LogMgr.I("LoadMgr", "UnLoad", "减少资源resID:" + resNode.ResID+"引用数后refCount:"+ resNode.RefCount, BeShowLog);
            return;
        }

        LogMgr.I("LoadMgr", "UnLoad", "引用计数为0准备卸载资源 resID：" + resNode.ResID+" unloadType:"+ resNode.UnloadType, BeShowLog);
        switch (resNode.UnloadType)
        {
            case ResEnum.ResUnloadTypeEnum.RefCount:                
                RealUnLoad(resNode);
                break;
            case ResEnum.ResUnloadTypeEnum.Delay:
                //NTODO 这里需要加个时间，并且在update中处理延迟释放列表的时间
                DelayUnloadResList.Add(resNode);
                break;
            case ResEnum.ResUnloadTypeEnum.Never:
                //不做操作，等待用户手动卸载，或者LoadMgr统一卸载资源时释放
                break;
        }

    }

    /// <summary>
    /// 真实卸载资源
    /// </summary>
    /// <param name="resNode"></param>
    public static void RealUnLoad(ResSaveNode resNode)
    {

        if (resNode.ResLoadURL == ResEnum.ResLoadURL.resources)
        {
            //这种方式只能卸载非gameObject资源(只能是image，sound等真实资源)
            //这里暂时无法判断asset具体类型，所以不添加下面的语句
            //而且Resources导入的资源只能使用Resources.UnloadUnusedAssets才能卸载干净
            //Resources.UnloadAsset(resNode.assetObj);卸载不干净
            ClearResNode(resNode);
            return;
        }

        LogMgr.I("LoadMgr", "RealUnLoad","卸载卸载bundle id："+resNode.ResID,BeShowLog);
        resNode.TAssetBundle.Unload(true);

        if (null != resNode.DependResArray)
        {
            int len = resNode.DependResArray.Length;
            
            for (int i = 0; i < len; i++)
            {
                ResSaveNode rnode = null;
                bool b = ResSaveNodeDic.TryGetValue(resNode.DependResArray[i],out rnode);
                if (b) UnLoad(rnode);
                else
                {
                    LogMgr.E("LoadMgr", "RealUnLoad","卸载资源:"+resNode.ResID+"的依赖资源:"+ resNode.DependResArray[i]+"失败",true);
                }
            }
        }

        ClearResNode(resNode);

    }

    private static void ClearResNode(ResSaveNode resNode)
    {
        ResSaveNodeDic.Remove(resNode.ResID);

        resNode.ResID = "nil";

        //resNode.PackType = ResEnum.RES_PACK_TYPE.Single;

        resNode.ResLoadURL = ResEnum.ResLoadURL.persistent;

        resNode.UnloadType = ResEnum.ResUnloadTypeEnum.RefCount;

        resNode.RefCount = 0;

        resNode.BeDependRes = false;

        resNode.DependResArray = null;

        resNode.TAssetBundle = null;

        resNode.SetObj(null);

        resNode.SetObjs(null);
        
        ResSaveNodePool.Put(resNode);
    }

    

    #endregion

}

/// <summary>
/// 等待加载优先级队列排序类
/// 数值大的排在栈顶
/// </summary>
public class BaseNodeCom:IComparer<ResLoadingNode>{
	public int Compare(ResLoadingNode a,ResLoadingNode b){
		if(a.Priority<b.Priority)
			return 1;
		else if(a.Priority==b.Priority)
			return 0;
		else {
			return -1;
		}
	}
}

