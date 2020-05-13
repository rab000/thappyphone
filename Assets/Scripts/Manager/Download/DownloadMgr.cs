using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System;

/// <summary>
/// 下载管理器
/// 1 支持断点续传并返回进度
/// 2 支持多文件并行下载
/// </summary>
public class DownloadMgr : MonoBehaviour {

	#region mono
	private static DownloadMgr Ins;

	private static GameObject SelfGo;

	public static DownloadMgr GetIns()
	{
		return Ins;
	}

	public static void Init()
	{
		SelfGo = new GameObject ("Download");
		SelfGo.AddComponent<DownloadMgr>();
	}

	public static void Dispose()
	{
		if (null != SelfGo) 
		{
			GameObject.Destroy (SelfGo);
			SelfGo = null;
		}
	}

	void Awake()
	{
		Ins = this;
	}

	void OnDestroy()
	{
		Ins = null;
	}

	void Update()
	{
		//释放下载完成的操作

		int count = DownloadNodeList.Count;

		if (count == 0)
			return;

		for (int i = 0; i < count; i++)
		{
			DownloadNode node = DownloadNodeList[i];

            if (node.IsDone()) 
			{
                //正常下载成功code=206,文件找不到code = 404,服务器没开找不到路径code=0
				
                long responseCode = node.GetResponseCode();

                Debug.Log("DownloadMgr.Update 下载结束responseCode:" + responseCode);

                if (responseCode == 206|| responseCode == 200)//本地nginx返回200
                {
                    if(null!= node.OnSuccess) node.OnSuccess();
                }
                else
                {
                    if (null != node.OnFaile)
                        node.OnFaile("isHttpError" + node.IsHttpError()+" isNetworkError:"+node.IsNetworkError()+" error:"+node.GetError()+ " responseCode:"+node.GetResponseCode());
                }

                string _url = node.Url;

                node.Dispose();

                RemoveDownloadNode(_url);

			}
		}

	}

	void OnApplicationQuit()
	{
		//释放下载完成的操作
		int count = DownloadNodeList.Count;

		for (int i = 0; i < count; i++) 
		{
            DownloadNodeList[i].Dispose();
        }

        RemoveAllDownloadNode();
	}

    #endregion

    private Dictionary<string, DownloadNode> DownloadNodeDic = new Dictionary<string, DownloadNode>();

    private List<DownloadNode> DownloadNodeList = new List<DownloadNode>();

    private void AddDownloadNode(string url, DownloadNode node)
    {
        DownloadNodeDic.Add(url, node);
        DownloadNodeList.Add(node);
    }
    private void RemoveDownloadNode(string url)
    {
        if (DownloadNodeDic.ContainsKey(url))
        {
            var webRequest = DownloadNodeDic[url];
            DownloadNodeList.Remove(webRequest);
            DownloadNodeDic.Remove(url);
        }
    }

    private void RemoveAllDownloadNode()
    {
        DownloadNodeList.Clear();
        DownloadNodeDic.Clear();
    }

    public void StartDownload(string url, string savePath,Action onSuccess=null,Action<string> onFaile=null,Action<int> onGetFileSize=null,Action<float> onProgress=null)
    {
        if (DownloadNodeDic.ContainsKey(url))
        {
            Debug.Log("下载列表已经存在路径=>" + url+" 不再重复下载");
            if (null != onFaile) onFaile("duplicate files");
            return;
        }

        DownloadNode node = new DownloadNode();
        node.Url = url;
        node.SavePath = savePath;
        node.OnSuccess = onSuccess;
        node.OnFaile = onFaile;
        node.OnGetFileSize = onGetFileSize;
        node.OnProgress = onProgress;
        AddDownloadNode(url, node);
        node.StartDownload();
    }

    public void StopDownload(string url)
    {
        DownloadNode node = null;
        if (!DownloadNodeDic.TryGetValue(url, out node))
        {
            Debug.Log("不存在下载的请求=>" + url);
            return;
        }

        RemoveDownloadNode(url);

        node.StopDownload();

    }


   



}
