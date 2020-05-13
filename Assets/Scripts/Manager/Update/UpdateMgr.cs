using System;
using UnityEngine;
using System.Collections.Generic;
using NLog;

public class UpdateMgr : SingletonBehaviour<UpdateMgr>
{
    private static bool BeShowLog = true;
  
    #region ref

    private CopyRes _CopyRes;
    private CopyRes TCopyRes
    {
        get
        {
            if (null == _CopyRes) _CopyRes = gameObject.GetComponent<CopyRes>();
            return _CopyRes;
        }
    }

    private UnCompress _UnCompress;
    private UnCompress TUnCompress
    {
        get
        {
            if (null == _UnCompress) _UnCompress = gameObject.GetComponent<UnCompress>();
            return _UnCompress;
        }
    }

    private DownloadRes _DownloadRes;
    private DownloadRes TDownloadRes
    {
        get
        {
            if (null == _DownloadRes)            
                _DownloadRes = gameObject.GetComponent<DownloadRes>();
            
            return _DownloadRes;
        }
    }

    #endregion ref

    #region var

    public static string PersistentResVer;

    public static string StreamResVer;

    public static string RemoteResVer;
    
    /// <summary>
    /// res cdn url
    /// 这里在进行更新前要设置下,注意这里还需要加平台前缀比如/Android/
    /// </summary>
    public static string RemoteCdnUrl = "http://127.0.0.1:8090";

    private static Listener OnUpdateComplete;

    #endregion var

    #region public

    /// <summary>
    /// 先初始化再启动更新检测
    /// </summary>
    /// <param name="cdnUrl"></param>
    public static void Init(string cdnUrl,Listener onUpdateComplete)
    {
        LogMgr.I("UpdateMgr","Init"," 设置下载cdn地址为:"+ cdnUrl, BeShowLog);
        RemoteCdnUrl = cdnUrl;
        OnUpdateComplete = onUpdateComplete;
    }

    /// <summary>
    /// 启动版本资源检测
    /// </summary>
    public void StartCheckVer()
    {
        SetState(CheckVerState.unCompress);
    }

    public void Clear()
    {
        Dispose();        
    }
       
    #endregion public

    #region fsm

    public enum CheckVerState
    {       
        unCompress,//解压包内资源到沙盒
        downLoad,//下载热更资源
        copyRess,//用新资源覆盖旧资源
        updateComplete,//清理临时资源阶段
        nil,
    }

    private CheckVerState curState = CheckVerState.nil;

    private CheckVerState preState = CheckVerState.nil;

    public void SetState(CheckVerState state)
    {
        if (state == curState) return;

        LogMgr.I("UpdateMgr","SetState","切换状态到:" + state,BeShowLog);

        preState = curState;
        StateExit(preState);
        curState = state;

        switch (curState)
        {
            case CheckVerState.unCompress:               
                ntools.Messenger.Broadcast<float>("updateLoadingProgress", 0f);
                ntools.Messenger.Broadcast<string>("updateLoadingText", "解压资源");
                TUnCompress.SetState(UnCompress.UnCompressState.checkPersistResVer);
                
                break;
            case CheckVerState.downLoad:
                ntools.Messenger.Broadcast<float>("updateLoadingProgress", 0f);             
                TDownloadRes.SetState(DownloadRes.DownloadResState.checkRemoteResVer);
                break;
            case CheckVerState.copyRess:                
                ntools.Messenger.Broadcast<string>("updateLoadingText", "复制资源");
                ntools.Messenger.Broadcast<float>("updateLoadingProgress", 0f);                
                TCopyRes.tStart();
                break;
            case CheckVerState.updateComplete:
                Clear();
                ntools.Messenger.Broadcast<string>("updateLoadingText", "资源更新完毕");
                OnUpdateComplete?.Invoke();
                break;
        }
    }

    public bool IsCurState(CheckVerState state)
    {
        if (curState == state) return true;
        else return false;
    }

    void StateExit(CheckVerState preState)
    {
       
    }

    #endregion


}
