using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using NLog;

public class SimpleResMgr : SingletonBehaviour<SimpleResMgr>
{

    private bool BeShowLog = true;

    private AssetBundleManifest manifest;

    public void LoadResXml(string url) 
    {
        switch (GameConfig.ResRootPath) 
        {
            case GameConfig.ResRootPathEnum.stream:
                LoadStreamResXml(url);
                break;
            case GameConfig.ResRootPathEnum.persist:
                LoadPersistResXml(url);
                break;
        }
    }
    public void LoadRes(string url, Action<AssetBundle> callback = null, Action<string> errorBack = null) 
    {
        switch (GameConfig.ResRootPath)
        {
            case GameConfig.ResRootPathEnum.stream:
                LoadStreamRes(url, callback, errorBack);
                break;
            case GameConfig.ResRootPathEnum.persist:
                LoadPersistRes(url, callback, errorBack);
                break;
        }
    }
    public void LoadScn(string scnName, string url, Action<float> progressCB = null, Action<AssetBundle> callback = null, Action<string> errorBack = null) 
    {
        switch (GameConfig.ResRootPath)
        {
            case GameConfig.ResRootPathEnum.stream:
                LoadStreamScn(scnName,url,progressCB,callback,errorBack);
                break;
            case GameConfig.ResRootPathEnum.persist:
                LoadPersistScn(scnName, url, progressCB, callback, errorBack);
                break;
        }
    }

    #region stream
    private void LoadStreamResXml(string url)
    {
        //var ct = StartCoroutine(LoadXml(url));

        Action<AssetBundle> callback =(bundle)=>
        {
            manifest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

            //获取到的name是 xxx.ab 这种带后缀的bundle文件名
            //string[] resNames = manifest.GetAllAssetBundles();
            //for (int i = 0; i < resNames.Length; i++)
            //{
            //    Debug.Log("i:" + i + " name:" + resNames[i]);
            //}

        };

        Action<string> errorBack = (errorInfo) =>
        {
            LogMgr.E("SimpleResMgr", "LoadXml", "errorInfo:" + errorInfo,BeShowLog);
        };

        StartCoroutine(LoadStreamBundle(url, callback, errorBack));

    }

    private void LoadStreamRes(string url,Action<AssetBundle> callback = null, Action<string> errorBack = null) 
    {
        StartCoroutine(LoadStreamBundle(url,callback,errorBack));
    }

    private void LoadStreamScn(string scnName,string url, Action<float> progressCB=null,Action<AssetBundle> callback=null, Action<string> errorBack = null) 
    {
        StartCoroutine(LoadStreamScnE(scnName,url, progressCB, callback, errorBack));        
    }

    private IEnumerator LoadStreamBundle(string url, Action<AssetBundle> callback = null, Action<string> errorBack = null)
    {

        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url);

        yield return request.SendWebRequest();

        if (request.isHttpError || request.isNetworkError)
        {
            Debug.LogError("FileHelper.go() www.error=" + request.error + " url:" + url + " isHttpError:" + request.isHttpError + " isNetworkError:" + request.isNetworkError);

            errorBack?.Invoke(request.error);
            yield break;
        }
        else
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
            callback?.Invoke(bundle);
        }

    }

    IEnumerator LoadStreamScnE(string scnName, string url, Action<float> progressCB = null, Action<AssetBundle> callback = null, Action<string> errorBack = null)
    {

        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url);

        yield return request.SendWebRequest();

        if (request.isHttpError || request.isNetworkError)
        {

            LogMgr.E("SimpleResMgr", "LoadScnE", "error:" + request.error + "  url:" + url + " isHttpError:" + request.isHttpError + " isNetworkError:" + request.isNetworkError, BeShowLog);

            errorBack?.Invoke(request.error);

            yield break;
        }
        else
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);

            //string[] strs = bundle.GetAllScenePaths();
            //foreach (string str in strs) 
            //{
            //    Debug.Log(str);
            //}

            AsyncOperation async = SceneManager.LoadSceneAsync(scnName);
            async.allowSceneActivation = false;
            while (async.progress < 0.9f)
            {
                //Debug.Log("场景进度  " + async.progress);
                progressCB?.Invoke(async.progress);
                yield return null;
            }

            callback?.Invoke(bundle);

            async.allowSceneActivation = true;
            yield break;


        }


        WWW www = new WWW(url);
        yield return www;
        if (string.IsNullOrEmpty(www.error))
        {
            //获取到ab包资源
            AssetBundle bundle = www.assetBundle;
            //输出所有资源地址
            string[] strs = bundle.GetAllScenePaths();
            foreach (string str in strs) Debug.Log(str);
            //一步读取场景
            AsyncOperation async = SceneManager.LoadSceneAsync("Test");//不需要带后缀//这里Test是我的场景名字
            async.allowSceneActivation = false;
            while (async.progress < 0.9f)
            {
                Debug.Log("场景进度  " + async.progress);
                yield return null;
            }
            async.allowSceneActivation = true;
            yield break;
        }
        Debug.Log("读取错误  " + www.error);

    }

    #endregion


    #region persist

    private void LoadPersistResXml(string url) 
    {
        
    }

    private void LoadPersistRes(string url, Action<AssetBundle> callback = null, Action<string> errorBack = null) 
    {
        
    }

    private void LoadPersistScn(string scnName, string url, Action<float> progressCB = null, Action<AssetBundle> callback = null, Action<string> errorBack = null) 
    {
        
    }

    #endregion

}
