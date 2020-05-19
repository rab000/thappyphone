using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SimpleResMgr : SingletonBehaviour<SimpleResMgr>
{

    public void LoadResXml(string url)
    {
        var ct = StartCoroutine(LoadXml(url));
    }

    IEnumerator LoadXml(string url) 
    {

        //https://forum.unity.com/threads/how-to-get-assetbundlemanifest.495494/

        //UnityWebRequest request = UnityWebRequest.Get(url);
        //yield return request.SendWebRequest();
        Debug.Log("url:"+url);

        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url);
        
        //UnityWebRequest request = new UnityWebRequest(url);
        //request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.isHttpError || request.isNetworkError)
        {
            Debug.LogError("FileHelper.go() www.error=" + request.error + " url:" + url + " isHttpError:" + request.isHttpError + " isNetworkError:" + request.isNetworkError);
            yield break;
        }
        else
        {

            AssetBundle manifestBundle = DownloadHandlerAssetBundle.GetContent(request);

            AssetBundleManifest manifest = manifestBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");


            //AssetBundleManifest xml = (AssetBundleManifest)www.assetBundle.LoadAsset("assetbundlemanifest");//这个名称永远固定，无论bundle名是什么，asset名都是这个


            //AssetBundle ab11 = DownloadHandlerAssetBundle.GetContent(request);




            //AssetBundle ab = (request.downloadHandler as DownloadHandlerAssetBundle).assetBundle;

            //AssetBundleManifest xml = (AssetBundleManifest)ab.LoadAsset("assetbundlemanifest");
          



            string[] resNames = manifest.GetAllAssetBundles();

            for (int i = 0; i < resNames.Length; i++)
            {
                Debug.Log("i:" + i + " name:" + resNames[i]);
            }

        }
    }

    public void LoadBundle() 
    {
        
    }

    IEnumerator wait()
    {
        string path = Application.streamingAssetsPath + "/test.ab";//ab包位置//test.ab是我ab包的名字
        WWW www = new WWW(path);
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

}
