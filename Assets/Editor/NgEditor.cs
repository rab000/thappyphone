using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
/// <summary>
/// 记录下小问题，这里面写死了要查找lua，table，res/map,res/re路径
/// 如果没有这些路径怎么办
/// </summary>
public class NgEditor : MonoBehaviour
{
    
    private static readonly Dictionary<string, string> ResInfoDic = new Dictionary<string, string>();

    [MenuItem("Tools/resInfoTable")]
    public static void GenerateResInfo()
    {
        ResInfoDic.Clear();

        //加入table res描述,手动生成信息
        //遍历查找一个文件夹下所有的文件，挨个计算hashcode，存到表中
        string pathTableFolder = Application.streamingAssetsPath + "/table";
        bool b = FileHelper.BeFolderExist(pathTableFolder);
        if (b)
        {
            ProcessTable(pathTableFolder);
        }
        else
        {
            Debug.LogError("NgEditor.GenerateResInfo 表目录:"+ pathTableFolder+"不存在，需要确认是否缺少目录");
        }

        //加入lua res描述,手动生成信息
        string pathLuaFolder = Application.streamingAssetsPath + "/lua";
        b = FileHelper.BeFolderExist(pathLuaFolder);
        if (b)
        {
            ProcessTable(pathLuaFolder);
        }
        else
        {
            Debug.LogError("NgEditor.GenerateResInfo 表目录:" + pathLuaFolder + "不存在，需要确认是否缺少目录");
        }


        //生成art res描述，从xml读取信息
        Action<Dictionary<string,string>> collectXmlInfoAction = (xmlDic) =>
        {

            foreach(var p in xmlDic) 
            {
                ResInfoDic.Add(p.Key,p.Value);
            }

        };

        CallbackMgr mgr = new CallbackMgr(()=> {

            Debug.Log("资源xml转到ResInfoDic完成！");

            IoBuffer buffer = new IoBuffer(102400);

            buffer.PutInt(ResInfoDic.Count);

            foreach (var p1 in ResInfoDic)
            {
                Debug.Log("最终存到表中的值------p.key:" + p1.Key + " p.value:" + p1.Value);
                buffer.PutString(p1.Key);
                buffer.PutString(p1.Value);
            }

            byte[] bs = buffer.ToArray();

            string savePath = Application.streamingAssetsPath + "/resInfoList.bytes";

            FileHelper.WriteBytes2File_Create(savePath,bs);

            AssetDatabase.Refresh();

        });

        string pathMe = Application.streamingAssetsPath + "/res/me/me";
        string pathMeRele = "/res/me";
        b = FileHelper.BeFolderExist(pathMe);
        if (b)
        {
            LoadXmlAction _actionMe = new LoadXmlAction(pathMe, pathMeRele, collectXmlInfoAction);
            mgr.Add(_actionMe);
        }
        else
        {
            Debug.LogError("NgEditor.GenerateResInfo 资源目录:" + pathMe + "不存在，需要确认是否缺少目录");
        }

        string pathRe = Application.streamingAssetsPath + "/res/re/re";
        string pathReRele = "/res/re";
        b = FileHelper.BeFolderExist(pathRe);
        if (b)
        {
            LoadXmlAction _actionRe = new LoadXmlAction(pathRe, pathReRele, collectXmlInfoAction);
            mgr.Add(_actionRe);
        }
        else
        {
            Debug.LogError("NgEditor.GenerateResInfo 资源目录:" + pathRe + "不存在，需要确认是否缺少目录");
        }

        mgr.Start();

    }

    private static void ProcessTable(string path)
    {
        string[] talbePaths = FindAllFileURLs(path);
        int len = talbePaths.Length;
        Debug.Log("NEditor.ProcessTable 开始处理文件夹 path:"+path+"下资源 资源数:"+len);
        for (int i = 0; i < len; i++)
        {
            string absPath = talbePaths[i];
            string hash = FileHash.MD5File(absPath);
            int subLen = absPath.Length - Application.streamingAssetsPath.Length;
            string relePath = absPath.Substring(Application.streamingAssetsPath.Length, subLen);
            Debug.Log("NEditor.ProcessTable  处理单个资源 i--->" + i + " absPath:" + absPath + " hash:" + hash + " relePath:" + relePath);

            ResInfoDic.Add(relePath, hash);
        }
    }

    /// <summary>
    /// 找文件夹下所有文件
    /// </summary>
    /// <param name="folderPath"></param>
    /// <returns></returns>
    public static string[] FindAllFileURLs(string folderPath)
    {

        string[] fileURLs = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);//注意这里要排除meta文件

        List<string> filsList = new List<string>();
        for (int i = 0; i < fileURLs.Length; i++)
        {
            //NDebug.i("查找目录"+folderPath+"下文件"+i+"->"+fileURLs[i],true,"EditorHelper.FindAllFiles");

            //避免找到无用的.meta文件
            if (fileURLs[i].Contains(".meta")) continue;

            filsList.Add(fileURLs[i]);

        }
        return filsList.ToArray();
    }


}

/// <summary>
/// 活动:用于处理资源xml信息收集
/// </summary>
public class LoadXmlAction : BaseCallback
{
    public string XmlPath;

    //me资源从xml读取的路径是相对与me路径的
    //解压streamingAsstePath资源需要的是相对StreamingAsset目录的路径，所以这里需要加个相对目录
    //eg:没加这个路径前
    //file:///C:/root/work/gitspace/ng/Assets/StreamingAssetsmap/mapname_t1/building.n
    //其中map/mapname_t1/building.n是具体编辑器生成bundle的xml里的内容，这个无法改变
    //加了路径/res/me后
    //file:///C:/root/work/gitspace/ng/Assets/StreamingAssets/res/me/map/mapname_t1/building.n
    //这个相对目录就是
    public string RelePath;

    public Action<Dictionary<string,string>> collectXml;

    public LoadXmlAction(string xmlPath,string relePath, Action<Dictionary<string,string>> collect)
    {
        XmlPath = xmlPath;
        RelePath = relePath;
        collectXml = collect;
    }

    public override void Start()
    {
        CollectResXmlInfo.CreateSelf(XmlPath, RelePath,ProcessXml);
    }

    private void ProcessXml(Dictionary<string,string> xml)
    {
        collectXml?.Invoke(xml);
        End();
    }

}

