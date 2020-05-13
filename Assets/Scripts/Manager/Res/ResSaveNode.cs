using NLog;
using UnityEngine;

using Object = UnityEngine.Object;

public class ResSaveNode : NodeObject
{

    protected static bool BeShowLog = true;

    /// <summary>
    /// 存储节点中的资源是从哪里加载的
    /// resources,steamingAssets,persistent
    /// </summary>
    public ResEnum.ResLoadURL ResLoadURL;
    
    // 资源id(资源相对路径)
    public string ResID;

    //资源卸载类型
    public ResEnum.ResUnloadTypeEnum UnloadType;

    public int RefCount; 
       

    public bool BeDependRes;

    public string[] DependResArray;

    public AssetBundle TAssetBundle;


    /// <summary>
    /// 种类assetObj和assetsObjs可以在ResNode生成时直接加载
    /// 但是永远不使用，目的是在bundle加载后就加载出所有Asset，而不是等待用户自己使用时加载
    /// 这样会卡顿一下(跟从Resource.Load后instance类似)
    /// 
    /// 这两个加载完后，用户再使用时，直接用TAssetBunlde.Load<T>(string name)就可以了
    /// </summary>
    private Object assetObj;
    
    private Object[] assetsObjs;

    public void ReduceRefCount()
    {
        RefCount--;
        LogMgr.I("LoadMgr", "ReduceRefCount", "资源引用减少 resid:" + ResID + " refCount:" + RefCount, BeShowLog);
    }

    /// <summary>
    /// 增加引用计数
    /// 引用增加位置，1回调，2内存中有这个资源并且直接返回了
    /// </summary>
    public void AddRefCount()
    {
        RefCount++;
        LogMgr.I("LoadMgr", "AddRefCount", "资源引用增加 resid:" + ResID + " refCount:" + RefCount, BeShowLog);
    }

    private void AddDependResRefCount()
    {
        if (null == DependResArray) return;

        int size = DependResArray.Length;

        for (int i = 0; i < size; i++)
        {
            ResSaveNode node = null;
            bool b = LoadMgr.ResSaveNodeDic.TryGetValue(DependResArray[i], out node);
            if (b)
            {
                node.AddRefCount();
            }
        }

    }

    /// <summary>
    /// 从ResNode bundle中获取指定名称指定类型的asset
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="resNode"></param>
    /// <returns></returns>
    public T GetAsset<T>(string name) where T : UnityEngine.Object
    {
        T t = TAssetBundle.LoadAsset<T>(name);
        AddRefCount();
        AddDependResRefCount();
        return t;
    }

    public T GetMainAsset<T>() where T : UnityEngine.Object
    {
        T t = assetObj as T;
        AddRefCount();
        AddDependResRefCount();
        return t;
    }

    public Object GetMainAsset()
    {       
        AddRefCount();
        AddDependResRefCount();
        return assetObj;
    }

    public Object[] GetObjs()
    {
        AddRefCount();
        AddDependResRefCount();
        return assetsObjs;
    }

    public void SetObjs(Object[] objs)
    {
        assetsObjs = objs;
    }

    public void SetObj(Object obj)
    {
        assetObj = obj;
    }
}
