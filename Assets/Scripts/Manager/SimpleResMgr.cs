using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleResMgr : SingletonBehaviour<SimpleResMgr>
{
    public void LoadResXml() 
    {
        
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
