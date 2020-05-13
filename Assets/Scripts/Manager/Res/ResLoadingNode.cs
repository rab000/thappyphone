using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;
using NLog;
/// <summary>
/// 资源（加载中）节点
/// </summary>
public class ResLoadingNode{

    private bool BeShowLog =true;

    public string ResID;

    public ResEnum.ResLoadURL ResLoadURL;

	public enum ResLoadStateEnum{
		Null,//未启动
		Done,//加载成功
		Error,//加载失败
	}
	public ResLoadStateEnum LoadState;//异步加载状态

    //资源加载状态
    public ResEnum.ResLoadingStepEnum LoadStep;

    //资源卸载类型
    public ResEnum.ResUnloadTypeEnum UnloadType;

    /// <summary>
    /// 资源本身是其他资源的依赖资源
    /// </summary>
    public bool BeDependRes;

    /// <summary>
    /// 加载中引用次数
    /// 比如一个被依赖的资源进入了加载队列，但是还没加载完的情况下，又被其他依赖这个资源的资源多次引用
    /// 这时就要在加载中增加引用次数，等到加载完成，把这个引用次数补充到ResNode中
    /// </summary>
    //public int RefCountInLoading;

    //public ResEnum.RES_PACK_TYPE PackType;//资源打包类型，单独资源，多资源，依赖资源

    //加载优先级
    public byte Priority;

    //用来存储依赖资源
    public string[] DependResArray;

	protected byte[] bytes;

	public Action<ResLoadingNode> CallbackSuccessAction;//www加载成功回调

	private string wwwError;

	public Action<string> CallbackFaileAction;//www加载失败回调

	public List<Listener<string,ResSaveNode>> CallBackList;

	public ResLoadingNode()
    {
		CallBackList = new List<Listener<string, ResSaveNode>>();
	}

	public void SetCallback(Action<ResLoadingNode> onSuccess,Action<string> onFaile = null)
    {
		LoadState = ResLoadStateEnum.Null;
		this.CallbackSuccessAction = onSuccess;
		this.CallbackFaileAction = onFaile;
	}

	/// <summary>
	/// www成功回调
	/// </summary>
	public void CallbackSuccess()
    {
		CallbackSuccessAction(this);
	}

	/// <summary>
	/// www失败回调
	/// </summary>
	public void CallbackFaile()
    {
		CallbackFaileAction(wwwError);
	}


	/// <summary>
	/// 启动节点资源加载
	/// </summary>
	public void Load()
    {
		string url = ResEnum.GetUrlByResID(this.ResID);//这里的url获取是完整地址，根据ResID是能从总资源表中找到ResName的

        switch (ResLoadURL)
        {
            case ResEnum.ResLoadURL.streamingAssets:
                ThreadManager.GetIns().StartCoroutine(CoroutineLoadStreamingAsset(url));
                break;
            case ResEnum.ResLoadURL.persistent:
                ThreadManager.GetIns().StartCoroutine(CoroutineLoadPersistent(url));
                break;
        }
		        
	}

    IEnumerator CoroutineLoadStreamingAsset(string url)
    {

        LogMgr.I("ResNode", "CoroutineLoadStreamingAsset", "url:" + url, BeShowLog);

		WWW www = new WWW(url);
		yield return www;

		if (www.error != null) {
			LogMgr.E ("ResNode", "CoroutineLoadStreamingAsset", "www.error=" + www.error, BeShowLog);
            LoadState = ResLoadStateEnum.Error;
            wwwError = www.error;
			yield break;
		}
		else
		{
			LogMgr.I ("ResNode", "CoroutineLoadStreamingAsset", "www success resid:"+ResID, BeShowLog);
		}
        //Debug.LogError("开始解析");
        DecodeAsset2ResNode(www.assetBundle);
        //Debug.LogError("加载状态修改为完成");
        LoadState = ResLoadStateEnum.Done;
        www.Dispose();
        www = null;
    }

    IEnumerator CoroutineLoadPersistent(string url){

		LogMgr.I ("ResNode", "CoroutineLoadPersistent", "url:"+url,BeShowLog);

        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(url);

        yield return request;
        
        //加载不到也会走这里，但是progress = 0并且request.assetBundle = null
        if (request.progress == 1 && null != request.assetBundle)
        {
            LogMgr.I("ResNode", "Load", "www success resid:" + ResID, BeShowLog);
            DecodeAsset2ResNode(request.assetBundle);
            LoadState = ResLoadStateEnum.Done;
           
        }
        else
        {
            LogMgr.E("ResNode", "Load", "www.error=" + wwwError, BeShowLog);
            LoadState = ResLoadStateEnum.Error;
            wwwError = string.Format("progress:{0}", request.progress);
            
        }

        //测试下加载失败的处理
        /**
		www = new WWW(url);
		yield return www;

		if (www.error != null) {
			LogMgr.E ("ResNode", "Load", "www.error=" + www.error, BeShowLog);
			WwwState = WWWState.Error;
			wwwError = www.error;
			yield break;
		}
		else
		{
			LogMgr.I ("ResNode", "Load", "www success resid:"+ResID, BeShowLog);
		}
		WwwState = WWWState.Done;
        **/

        //这里面既不返回成功，也不返回失败，只保留状态，让调用者去调用成功失败的回调,目的是由外部控制同时加载数量
        //WWWCallBack();
    }

	/// <summary>
	/// 从www获取bytes
	/// </summary>
	/// <returns>The bytes.</returns>
	public byte[] GetBytes(){
        //if(null!=www){
        //	bytes = www.bytes;
        //	return bytes;
        //}
        //else{
        //	Debug.LogError("BaseNode.GetBytes:www为null");
        //	return null;
        //}

        //TODO 这里后面从streamingAsstet转存资源到沙盒中时要用

        return null;
	}

	/// <summary>
	/// 从www获取asset资源到ResNode
	/// </summary>
	/// <returns>The main asset.</returns>
	public void DecodeAsset2ResNode(AssetBundle bundle){

        ResSaveNode resNode = LoadMgr.ResSaveNodePool.Get();
        resNode.ResID = ResID;
        resNode.TAssetBundle = bundle;
        UnityEngine.Object[] objs = bundle.LoadAllAssets();
        resNode.SetObjs(objs);
        resNode.SetObj(objs[0]);

        //if (BeDependRes) resNode.AddRefCount();
        resNode.BeDependRes = BeDependRes;
        resNode.ResLoadURL = ResLoadURL;
        resNode.UnloadType = this.UnloadType;
        //resNode.PackType = this.PackType;       
        resNode.DependResArray = this.DependResArray;

        if (!LoadMgr.ResSaveNodeDic.ContainsKey(ResID))
        {
            LogMgr.I("BundleResNode", "DecodeAsset2ResNode","资源ResID:"+ ResID+ "加入ResSaveNodeDic",BeShowLog);
            LoadMgr.ResSaveNodeDic.Add(ResID, resNode);
        }
        else
        {
            LogMgr.I("BundleResNode", "DecodeAsset2ResNode", "LoadMgr.TResNodeDic 中存在resID:" + ResID + " 重复添加",BeShowLog);
        }

        //TODO 这里要考虑下ResLoadingNode本身用完就要回池，这个应该在LoadMgr的update中做

   //     if (null!=www){           
   //         assetBundle = www.assetBundle;
			//assetsObjs = www.assetBundle.LoadAllAssets();
			//assetObj = assetsObjs[0];

			//			if(PackType==ResEnum.RES_PACK_TYPE.single){
			//				assetBundle = www.assetBundle;
			//				//nafio info 5.4后新版打包不在使用mainAsset,单独打包就认为是第一个资源
			//				//assetObj = assetBundle.mainAsset;
			//				//Debug.Log("ResNode.Decode ===============>len:"+assetBundle.GetAllAssetNames().Length+" firstName:"+assetBundle.GetAllAssetNames()[0]);
			//				//string[] ss = assetBundle.GetAllAssetNames();
			//				//assetObj = assetBundle.LoadAllAssets()[0];
			//
			//				assetObj = assetBundle.LoadAllAssets()[0];
			//
			//				//nafio info 这里解析完后面会立刻卸载bundle和www，升级unity2017后，不能立刻做卸载，否则保存，比如在这句后面随便加一句log都不会出错
			//				//Debug.Log("ResNode.Decode ===============>endFuck");//TODO 发现神奇的问题，这里需要一个延迟，否则就出bug,问题是为什么需要延迟
			//
			//			}else if(PackType==ResEnum.RES_PACK_TYPE.multi){
			//				//TODO 这里以后考虑下异步怎么处理,这里以后要考虑下怎么避免载入同样资源报错，
			//				//错误复现，复制一个bundle改名，然后同时加载两个相同内容的bundle
			//				assetBundle = www.assetBundle;
			//				assetsObjs = www.assetBundle.LoadAllAssets();
			//			}else if(PackType == ResEnum.RES_PACK_TYPE.depend){
			//				//TODO 对于依赖资源的处理是保留assetbundle不释放，做过实验后可以考虑跟多资源一样，保存assets，这个能否这么做要看只保存asset不保留assetbundle时依赖资源是否会丢失
			//				assetBundle = www.assetBundle;
			//				assetObj = assetBundle.LoadAllAssets()[0];
			//			}

			//return obj;
		//}
		//else{
		//	Debug.LogError("BaseNode.GetMainAsset:www为null");
		//}
	}
	
	/// <summary>
	/// 释放assetbundle
	/// </summary>
	//public void UnLoadAssetbundle(bool b=false){
	//	if(null!=assetBundle){
	//		assetBundle.Unload(b);
	//	}else{
	//		LogMgr.I("ResNode","UnLoadAssetbundle","assetBundle已经为null，不需要再次Unload",BeShowLog);
	//	}
	//}

	/// <summary>
	/// 清空ResNode
	/// </summary>
	public void Clear(){
		LogMgr.I ("ResNode","Clear","清理ResNode ResID:"+ResID,BeShowLog);

		bytes = null;
		ResID = "null";
        BeDependRes = false;
        //RefCountInLoading = 0;
        LoadState = ResLoadStateEnum.Null;
		LoadStep = ResEnum.ResLoadingStepEnum.Null;
		Priority = ResEnum.Load_Priority_Normarl;
		if (null != DependResArray) 
		{
			Array.Clear(DependResArray,0,DependResArray.Length);
			DependResArray = null;
		}
		//RefCount = 0;
		CallbackSuccessAction = null;
		wwwError = null;
		CallbackFaileAction = null;

		CallBackList.Clear();

	}

	public void CallBack(){

		int size = CallBackList.Count;

        //foreach (var p in LoadMgr.ResSaveNodeDic)
        //{
        //    Debug.LogError("出错前表中所有元素:" + p.Key);
        //}

        //Debug.LogError("要查找的元素为:"+ ResID);
        ResSaveNode resNode = LoadMgr.ResSaveNodeDic[ResID];

        for (int i=0;i<size;i++)
        {
			//NaTodo 后面考虑把这个判断去掉，这个判断目前针对依赖资源
			if (null == CallBackList[i])
            {              
                //LoadMgr.AddRefCount(resNode);
				continue;
			}

			CallBackList[i](ResID, resNode);

            //LoadMgr.AddRefCount(resNode);
		}
	}	

}