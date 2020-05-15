using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CollectResXmlInfo : MonoBehaviour
{
    public static GameObject go;
    public static WWW www;
    private static string XmlPath;
    //看LoadXmlAction中的注释
    private static string RelePath;
    private static Action<Dictionary<string,string>> ProcessXmlAction;

    public static void CreateSelf(string path,string relePath,Action<Dictionary<string,string>> processXml = null)
    {

        XmlPath = path;
        RelePath = relePath;
        ProcessXmlAction = processXml;
        go = new GameObject("CollectResXmlInfo");
        CollectResXmlInfo collect =  go.AddComponent<CollectResXmlInfo>();
        collect.StartLoad();
    }

    public void StartLoad()
    {
        Debug.LogError("1启动----->path:" + XmlPath);
        StartCoroutine(LoadXml());
    }

    IEnumerator LoadXml()
    {
        
        //callback action 有些问题，这里相当于过早回调，导致下一次加载启动了，然后又执行了go.destroy把第二次加载的www干掉了，导致加载停止

        www = new WWW(XmlPath);

        yield return www;

        Debug.LogError("www返回：" + XmlPath);

        if (null != www.error) Debug.Log("加载xmlBundle www error:" + www.error);

        string[] names = www.assetBundle.GetAllAssetNames();

        //for (int i = 0; i < names.Length; i++)
        //{
        //    Debug.Log("---[" + i + "]------>" + names[i]);
        //}

        Dictionary<string, string> tempDic = new Dictionary<string, string>();

        AssetBundleManifest xml = (AssetBundleManifest)www.assetBundle.LoadAsset("assetbundlemanifest");//这个名称永远固定，无论bundle名是什么，asset名都是这个

        string[] bundleNames = xml.GetAllAssetBundles();

        for (int i = 0; i < bundleNames.Length; i++)
        {
            string hash = xml.GetAssetBundleHash(bundleNames[i]).ToString();
            

            string path = RelePath + "/" + bundleNames[i];
            //Debug.LogError("---i:" + i + " relepath:" + path + " hash:" + hash);
            tempDic.Add(path, hash);
        }

        //ninfo 编辑器下用完不关下回就不能二次载入
        www.assetBundle.Unload(true);
        www.Dispose();
        www = null;
        DestroyImmediate(go);
        go = null;


        ProcessXmlAction(tempDic);
    }
   
}
