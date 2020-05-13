using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class DownloadNode {

    public string Url;

    public string SavePath;

    TDownloadHandler _TDownloadHandler;

    UnityWebRequest _UnityWebRequest;

    public Action OnSuccess;

    public Action<string> OnFaile;

    public Action<int> OnGetFileSize;

    public Action<float> OnProgress;

    public void StartDownload()
    {
        _TDownloadHandler = new TDownloadHandler(SavePath);
        _UnityWebRequest = UnityWebRequest.Get(Url);
        _UnityWebRequest.chunkedTransfer = true;//支持分块传输
        _UnityWebRequest.disposeDownloadHandlerOnDispose = true;//设置为true时，如果UnityWebRequest执行dispose，则downloader自动dispose
        _UnityWebRequest.SetRequestHeader("Range", "bytes=" + _TDownloadHandler.DownedLength + "-"); //断点续传设置读取文件数据流开始索引，成功会返回206
        _UnityWebRequest.downloadHandler = _TDownloadHandler;

        //request.timeout = 20;//20秒超时时间，这个暂时不知道怎么模拟超时情况
        //request.useHttpContinue = true; //默认就是true
        //request.Send(); //协程操作，可以在协程中调用等待,这个方法过时了，用下面方法替代
        _UnityWebRequest.SendWebRequest();

        //可以用yield的方式处理，但是需要全部文件下载完毕才执行yield后面的内容，所以这里不适合用yield
        //yield return request.Sendwebrequest();
        //if (request.ishttperror || request.isnetworkerror)
        //{
        //    debug.logerror("下载网络发生异常 ishttperror:" + request.ishttperror + " networkerror:" + request.isnetworkerror + " error:" + request.error);
        //}

        _TDownloadHandler.RegisteReceiveTotalLengthBack(OnGetFileSize);
        _TDownloadHandler.RegisteProgressBack(OnProgress);

    }

    public void StopDownload()
    {
        _TDownloadHandler.OnDispose();//释放文件操作的资源
        _UnityWebRequest.Abort();//中止下载
        _UnityWebRequest.Dispose();//释放
        _TDownloadHandler = null;
        _UnityWebRequest = null;
    }

    public void Dispose()
    {

        _TDownloadHandler.OnDispose();

        _UnityWebRequest.Dispose();
    }

    public bool IsDone()
    {
        return _UnityWebRequest.isDone;
    }

    public bool IsHttpError()
    {

        return _UnityWebRequest.isHttpError;
    }

    public bool IsNetworkError()
    {
        return _UnityWebRequest.isNetworkError;
    }

    public long GetResponseCode()
    {
        return _UnityWebRequest.responseCode;
    }

    public string GetError()
    {
        return _UnityWebRequest.error;
    }


}
