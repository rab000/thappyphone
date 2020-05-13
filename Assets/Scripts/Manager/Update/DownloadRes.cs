using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntools;
using NLog;
/// <summary>
/// 下载热更资源
/// </summary>
public class DownloadRes : MonoBehaviour
{
    bool BeShowLog = true;

    #region var

    /// <summary>
    /// 下载资源缓存相对路径
    /// </summary>
    private const string DownloadCachePath = "/downloadCache";

    private const string DownloadOverFileName = "downloadOverFlag.txt";

    private Dictionary<string, string> PersisResInfoDic = new Dictionary<string, string>();

    private Dictionary<string, string> RemoteResInfoDic = new Dictionary<string, string>();

    /// <summary>
    /// 待下载资源字典
    /// 用于临时存储比较远程和本地resInfoList后得到的待下载资源
    /// 为了方便过滤掉断点续传已经下载完的资源，做了这个dic
    /// 当过滤完成后，转存到DownloadResInfoQueue中，准备进行下载
    /// </summary>
    private Dictionary<string, string> DownloadResInfoDic = new Dictionary<string, string>();
    /// <summary>
    /// 待下载资源队列
    /// </summary>
    private Queue<string> DownloadResInfoQueue = new Queue<string>();

    /// <summary>
    /// 待下载文件总数
    /// </summary>
    private float DownloadFileNum = 0;

    #endregion var

    #region fsm

    public enum DownloadResState
    {
        checkRemoteResVer,//拉取并查看远程resVer       
        compareResVerPersist_Remote,//比较沙盒和远程resVer
        checkPersistDownloadingRes,//检测本地(沙盒中)是否有上次未更新完的资源
        pullRemoteResInfoList,//拉取remote resInfoList
        readPersistResInfoList,//获取沙盒resInfoList
        compareResInfoListPersist_Remote,//对比沙盒与远程resInfoList，生成待更新资源表

        checkBreakpoint,//检查是否是断电续传
        filterUpdateResInfoList,//过滤掉之前已经下载过的资源，断点续传没下载过的资源
        saveRemoteResVer2Downloading,//将远程resVer存到下载中目录

        startDownload,//下载资源       
        saveResInfoList2DownloadingFoler,//将remote最新的resInfoList保存到下载中目录，等待移动到沙盒位置
        generateDownloadOverFlagFile,//生成下载完成标识文件
        downloadComplete,//下载资源完毕
        nil
    }

    private DownloadResState curState = DownloadResState.nil;

    private DownloadResState preState = DownloadResState.nil;

    public void SetState(DownloadResState state)
    {
        if (state == curState) return;

        LogMgr.I("DownloadRes", "SetState", "SetState切换状态到:" + state, BeShowLog);

        preState = curState;
        StateExit(preState);
        curState = state;

        switch (curState)
        {
            case DownloadResState.checkRemoteResVer:
                CheckRemoteResVer();                
                break;
            case DownloadResState.compareResVerPersist_Remote:
                CompareResVerPersist_Remote();
                break;
            case DownloadResState.checkPersistDownloadingRes:
                CheckPersistDownloadingRes();
                break;           
            case DownloadResState.pullRemoteResInfoList:
                PullRemoteResInfoList(); 
                break;
            case DownloadResState.readPersistResInfoList:
                ReadPersistResInfoList();
                break;
            case DownloadResState.compareResInfoListPersist_Remote:
                CompareResInfoListPersist_Remote();
                break;
            case DownloadResState.checkBreakpoint:
                CheckBreakpoint();
                break;
            case DownloadResState.filterUpdateResInfoList:
                FilterUpdateResInfoList();
                break;
            case DownloadResState.saveRemoteResVer2Downloading:
                SaveRemoteResVer2Downloading();
                break;
            case DownloadResState.startDownload:
                StartDownload();
                break;           
            case DownloadResState.saveResInfoList2DownloadingFoler:
                SaveResInfoList2DownloadingFoler();
                break;
            case DownloadResState.generateDownloadOverFlagFile:
                GenerateDownloadOverFlagFile();
                break;
            case DownloadResState.downloadComplete:
                Clear();
                //这里的100即可是版本检测的100也可是下载的100
                Messenger.Broadcast<float>("updateLoadingProgress", 1f);                
                UpdateMgr.GetIns().SetState(UpdateMgr.CheckVerState.copyRess);
                break;
        }
    }

    private void StateExit(DownloadResState state)
    {
        switch (state)
        {
            case DownloadResState.checkRemoteResVer:                
                Messenger.Broadcast<string>("updateLoadingText", "检查资源更新");
                Messenger.Broadcast<float>("updateLoadingProgress", 0.10f);                
                break;
            case DownloadResState.compareResVerPersist_Remote:
                Messenger.Broadcast<float>("updateLoadingProgress", 0.20f);
                
                break;
            case DownloadResState.checkPersistDownloadingRes:
               
                Messenger.Broadcast<float>("updateLoadingProgress", 0.30f);
                
                break;
            case DownloadResState.pullRemoteResInfoList:                             
                Messenger.Broadcast<float>("updateLoadingProgress", 0.40f);
                break;
            case DownloadResState.readPersistResInfoList:
                Messenger.Broadcast<float>("updateLoadingProgress", 0.50f);
                break;
            case DownloadResState.compareResInfoListPersist_Remote:
                Messenger.Broadcast<float>("updateLoadingProgress", 0.60f);
                break;
            case DownloadResState.checkBreakpoint:
                Messenger.Broadcast<float>("updateLoadingProgress", 0.70f);
                break;
            case DownloadResState.filterUpdateResInfoList:
                Messenger.Broadcast<float>("updateLoadingProgress", 0.80f);
                break;
            case DownloadResState.saveRemoteResVer2Downloading:
                Messenger.Broadcast<float>("updateLoadingProgress", 1f);
                break;

            case DownloadResState.startDownload:
                Messenger.Broadcast<string>("updateLoadingText", "下载中");
                Messenger.Broadcast<float>("updateLoadingProgress", 0f);
                break;
            case DownloadResState.saveResInfoList2DownloadingFoler:
               
                Messenger.Broadcast<float>("updateLoadingProgress", 0.90f);
               
                break;
            case DownloadResState.generateDownloadOverFlagFile:
                
                Messenger.Broadcast<float>("updateLoadingProgress", 0.95f);
                
                break;
            case DownloadResState.downloadComplete:               
               
                break;
        }
    }

    #endregion fsm

    #region func
    public void Clear()
    {
        PersisResInfoDic.Clear();

        RemoteResInfoDic.Clear();

        DownloadResInfoDic.Clear();

        DownloadResInfoQueue.Clear();

        DownloadFileNum = 0;
    }

    /// <summary>
    /// 检查远程resVer，并存储下来
    /// </summary>
    private void CheckRemoteResVer()
    {
        //从remote读取resVer
        Utils.SB.Append(UpdateMgr.RemoteCdnUrl);
        Utils.SB.Append("/");
        Utils.SB.Append(UnityUtil.CurPlatform);
        Utils.SB.Append("/resVer.txt");
        string remoteResVerPath = Utils.SB.ToString();
        Utils.ClearSB();
        Action<byte[]> onSuccess = (bs) =>
        {
            UpdateMgr.RemoteResVer = TypeConvertUtils.Bytes2String(bs);
            LogMgr.I("DownloadRes", "CheckRemoteResVer", "获取remote resVer成功 resVer;" + UpdateMgr.RemoteResVer,BeShowLog);
            SetState(DownloadResState.checkPersistDownloadingRes);
        };

        Action<string> onFaile = (error) =>
        {
            LogMgr.E("DownloadRes", "CheckRemoteResVer", "下载流程中断，获取remote resVer失败;" + remoteResVerPath, BeShowLog);            
        };

        LogMgr.I("DownloadRes", "CheckRemoteResVer","开始获取remote resVer url;"+ remoteResVerPath,BeShowLog);

        FileHelper.GetIns().ReadBytesFromApkFile(remoteResVerPath, onSuccess, onFaile);

    }

    /// <summary>
    /// 检测本地(沙盒中)是否有上次未更新完的资源
    /// </summary>
    private void CheckPersistDownloadingRes()
    {
        Utils.SB.Append(UnityUtil.PersistentPath);
        Utils.SB.Append(DownloadCachePath);
        string downloadingFolderPath = Utils.SB.ToString();
        //下载中目录如果不存在就创建
        FileHelper.CreateFolder(downloadingFolderPath);
        Utils.ClearSB();

        Utils.SB.Append(UnityUtil.PersistentPath);
        Utils.SB.Append(DownloadCachePath);
        Utils.SB.Append("/resVer.txt");
        string downloadingResver = Utils.SB.ToString();
        Utils.ClearSB();
        bool bDownloadingResVerExist = FileHelper.BeFileExist(downloadingResver);

        LogMgr.I("DownloadRes", "CheckPersistDownloadingRes", "开始检测是否存在更新一半的资源", BeShowLog);

        //本地有未更新完版本
        if (bDownloadingResVerExist)
        {            
            string downloadingResVer = FileHelper.ReadStringFromFile(downloadingResver);
            int result = VersionUtils.CompareVersion(downloadingResVer, UpdateMgr.PersistentResVer);
            
            //下载中版本与远程版本是否一致
            if (result == 0)
            {
                LogMgr.I("DownloadRes", "CheckPersistDownloadingRes", "下载中目录存在resVer:" + downloadingResVer+"并且版本与沙盒resVer一致", BeShowLog);
                //一致
                Utils.SB.Append(UnityUtil.PersistentPath);
                Utils.SB.Append("/");
                Utils.SB.Append(DownloadOverFileName);
                string downloadOverFilePath = Utils.SB.ToString();
                Utils.ClearSB();
                //上一次下载完成了么
                bool bDownloadOverFileExist = FileHelper.BeFileExist(downloadOverFilePath);
                
                if (bDownloadOverFileExist)
                {
                    LogMgr.I("DownloadRes", "CheckPersistDownloadingRes", "下载中目录存在下载完成标志,说明下载已经完成，上次复制资源流程可能发生了中断，准备掠过下载流程，进入复制流程", BeShowLog);
                    //下载已完成(此条件进入说明上次复制资源流程可能被打断)，继续复制资源
                    SetState(DownloadResState.downloadComplete);
                }
                else
                {
                    LogMgr.I("DownloadRes", "CheckPersistDownloadingRes", "下载中目录不存在下载完成标志,说明下载未完成，准备拉取remoteResInfoList", BeShowLog);
                    //下载未完成
                    SetState(DownloadResState.pullRemoteResInfoList);
                }
            }
            else
            {
                LogMgr.I("DownloadRes", "CheckPersistDownloadingRes", "下载中目录resVer:" + downloadingResVer +"沙盒ResVer:"+ UpdateMgr.PersistentResVer + "不一致,清理下载中目录，准备进行沙盒，remote resVer比较", BeShowLog);
                //不一致，清理下载中目录                              
                FileHelper.DeleteFolder(downloadingFolderPath);
                SetState(DownloadResState.compareResVerPersist_Remote);
            }
        }
        else
        {
            LogMgr.I("DownloadRes", "CheckPersistDownloadingRes", "下载中目录不存在resVer,说明不存在更新一半的资源,准备进行沙盒，remote resVer比较", BeShowLog);
            //本地没有更新一半的资源
            SetState(DownloadResState.compareResVerPersist_Remote);
        }

    }

    /// <summary>
    /// 比较沙盒与远程resVer
    /// </summary>
    private void CompareResVerPersist_Remote()
    {
        int result = VersionUtils.CompareVersion(UpdateMgr.PersistentResVer, UpdateMgr.RemoteResVer);
        
        switch (result)
        {
            case 0://相同，不需要更新
            case -1://persistentResVer大，理论上不应该存在本地比server大的情况
                LogMgr.I("DownloadRes", "CompareResVerPersist_Remote", "比较resVer persist:"+ UpdateMgr.PersistentResVer+" remoteResVer"+ UpdateMgr.RemoteResVer+" 不需要下载，准备进入复制流程", BeShowLog);
                SetState(DownloadResState.downloadComplete);
                break;
            case 1://RemoteResVer,准备更新流程
                LogMgr.I("DownloadRes", "CompareResVerPersist_Remote", "比较resVer persist:" + UpdateMgr.PersistentResVer + " remoteResVer" + UpdateMgr.RemoteResVer + " 需要下载，准备拉取remote resInfoList", BeShowLog);
                SetState(DownloadResState.pullRemoteResInfoList);
                break;
        }
    }

    /// <summary>
    /// 拉取远程cdn资源描述列表
    /// </summary>
    private void PullRemoteResInfoList()
    {        
        Action<byte[]> onSuccess = (bs) =>
        {
            IoBuffer ib = new IoBuffer();
            ib.PutBytes(bs);
            int num = ib.GetInt();
            LogMgr.I("DownloadRes", "PullRemoteResInfoList"," 获取remote resInfoList成功 资源数:"+num,BeShowLog);

            for (int i = 0; i < num; i++)
            {
                string keyRelePath = ib.GetString();
                string valueHash = ib.GetString();
                RemoteResInfoDic.Add(keyRelePath,valueHash);
            }

            SetState(DownloadResState.readPersistResInfoList);

        };
        Action<string> onFaile = (error) =>
        {
            LogMgr.E("DownloadRes", "PullRemoteResInfoList", "下载流程中断 获取remote resInfoList失败", BeShowLog);            
        };

        Utils.SB.Append(UpdateMgr.RemoteCdnUrl);
        Utils.SB.Append("/");
        Utils.SB.Append(UnityUtil.CurPlatform);
        Utils.SB.Append("/resInfoList.bytes");
        string remoteResInfoListPath = Utils.SB.ToString();
        Utils.ClearSB();
        LogMgr.I("DownloadRes", "PullRemoteResInfoList","开始拉取remote resInfoList url:"+ remoteResInfoListPath, BeShowLog);
        FileHelper.GetIns().ReadBytesFromApkFile(remoteResInfoListPath, onSuccess,onFaile);
    }

    /// <summary>
    /// 获取沙盒resInfoList，并存在内存中
    /// </summary>
    private void ReadPersistResInfoList()
    {
        Utils.SB.Append(UnityUtil.PersistentPath);
        Utils.SB.Append("/resInfoList.bytes");
        string persisResInfoListPath = Utils.SB.ToString();
        Utils.ClearSB();
        byte[] bs = FileHelper.ReadBytesFromFile(persisResInfoListPath);

        IoBuffer ib = new IoBuffer();
        ib.PutBytes(bs);
        int num = ib.GetInt();
        LogMgr.I("DownloadRes", "CompareResInfoListPersist_Remote", "获取沙盒resInfoList res num:" + num+" path:"+ persisResInfoListPath, BeShowLog);
        for (int i = 0; i < num; i++)
        {
            string keyRelePath = ib.GetString();
            string valueHash = ib.GetString();
            //LogMgr.I("DownloadRes", "CompareResInfoListPersist_Remote", " keyRelePath:"+ keyRelePath, BeShowLog);
            PersisResInfoDic.Add(keyRelePath, valueHash);
        }

        SetState(DownloadResState.compareResInfoListPersist_Remote);
    }

    /// <summary>
    /// 对比沙盒与远程ResInfoList
    /// 筛选出待更新资源表
    /// </summary>
    private void CompareResInfoListPersist_Remote()
    {
        LogMgr.I("DownloadRes", "CompareResInfoListPersist_Remote", "开始比对沙盒，远程resInfoList，并生成下载列表--------->",BeShowLog);

        foreach (var remoteRes in RemoteResInfoDic)
        {
            if (PersisResInfoDic.ContainsKey(remoteRes.Key))
            {
                string hashRemote = remoteRes.Value;
                string hashPersis = PersisResInfoDic[remoteRes.Key];
                if (!hashRemote.Equals(hashPersis))
                {
                    LogMgr.I("DownloadRes", "CompareResInfoListPersist_Remote", "资源变更:"+ remoteRes.Key, BeShowLog);
                    //有修改的资源
                    DownloadResInfoDic.Add(remoteRes.Key,remoteRes.Value);
                    //DownloadResInfoQueue.Enqueue(remoteRes.Key);
                }
            }
            else
            {
                LogMgr.I("DownloadRes", "CompareResInfoListPersist_Remote", "资源新增:" + remoteRes.Key, BeShowLog);
                //新增的资源
                DownloadResInfoDic.Add(remoteRes.Key, remoteRes.Value);
                //DownloadResInfoQueue.Enqueue(remoteRes.Key);
            }
        }

        LogMgr.I("DownloadRes", "CompareResInfoListPersist_Remote", "<----------------比对沙盒，远程resInfoList，并生成下载列表结束", BeShowLog);

        //这里缺少了步骤，检查是否是首次更新，还有存远程resVer到downloading的操作
        SetState(DownloadResState.checkBreakpoint);

    }

    /// <summary>
    /// 检查是否是断电续传
    /// </summary>
    private void CheckBreakpoint()
    {
        Utils.SB.Append(UnityUtil.PersistentPath);
        Utils.SB.Append(DownloadCachePath);
        Utils.SB.Append("/resVer.txt");
        string downladingResVerPath = Utils.SB.ToString();
        Utils.ClearSB();

        bool bFileExist = FileHelper.BeFileExist(downladingResVerPath);
        if (bFileExist)
        {
            LogMgr.I("DownloadRes", "CheckBreakpoint", "下载中目录已存在resVer，本次为断点续传,准备过滤updateResInfoList",BeShowLog);
            SetState(DownloadResState.filterUpdateResInfoList);
        }
        else
        {
            LogMgr.I("DownloadRes", "CheckBreakpoint", "下载中目录不存在resVer，本次非断点续传，准备存储resVer到下载中目录", BeShowLog);
            SetState(DownloadResState.saveRemoteResVer2Downloading);
        }

    }

    /// <summary>
    /// 过滤掉之前已经下载过的资源，断点续传没下载过的资源
    /// </summary>
    private void FilterUpdateResInfoList()
    {        
        Utils.SB.Append(UnityUtil.PersistentPath);
        Utils.SB.Append(DownloadCachePath);
        string downloadPath = Utils.SB.ToString();
        Utils.ClearSB();

        //断点前已经下载完的资源(downloading目录中已经有的资源),这些资源已经下载完，就不需要二次下载了
        string[] downloadedFilePaths = FileHelper.FindFilePathsInFolder(downloadPath);

        LogMgr.I("DownloadRes", "FilterUpdateResInfoList", "开始断点续传前，过滤已经下载资源", BeShowLog);

        for (int i = 0; i < downloadedFilePaths.Length; i++)
        {
            //这里的路径还不是相对路径，需要去掉下载中目录的路径才是相对路径
            string fileAbsPath = downloadedFilePaths[i];
            string fileRelePath = fileAbsPath.Substring(downloadPath.Length);

            if (DownloadResInfoDic.ContainsKey(fileRelePath))
            {
                LogMgr.I("DownloadRes", "FilterUpdateResInfoList", "过滤掉已下载资源:"+ fileRelePath, BeShowLog);
                //差量更新表中包含已经下载完毕的资源fileRelePath，排除掉
                DownloadResInfoDic.Remove(fileRelePath);
            }
        }

        SetState(DownloadResState.saveRemoteResVer2Downloading);
    }

    /// <summary>
    /// 将远程resVer存到下载中目录
    /// </summary>
    private void SaveRemoteResVer2Downloading()
    {
        Utils.SB.Append(UnityUtil.PersistentPath);
        Utils.SB.Append(DownloadCachePath);
        Utils.SB.Append("/resVer.txt");
        string savePath = Utils.SB.ToString();
        Utils.SB.Clear();
        FileHelper.WriteString2File_Append(savePath,UpdateMgr.RemoteResVer);
        LogMgr.I("DownloadRes", "SaveRemoteResVer2Downloading", "将远程resVer存到下载中目录 保存地址:"+ savePath, BeShowLog);
        SetState(DownloadResState.startDownload);
    }

    /// <summary>
    /// 启动下载
    /// </summary>
    private void StartDownload()
    {
        DownloadMgr.Init();

        //过滤后的待下载资源存储到下载队列中，准备下载
        foreach (var p in DownloadResInfoDic)
        {
            DownloadResInfoQueue.Enqueue(p.Key);
        }

        DownloadFileNum = DownloadResInfoQueue.Count;

        LogMgr.I("DownloadRes", "StartDownload", "启动下载 资源数:"+ DownloadFileNum, BeShowLog);

        StartCoroutine("DelayDown");        

    }

    /// <summary>
    /// 等待DownloadMgr完成初始化再继续
    /// </summary>
    /// <returns></returns>
    IEnumerator DelayDown()
    {
        yield return 0;

        if (DownloadResInfoQueue.Count > 0)
        {
            LogMgr.I("DownloadRes", "DelayDown", "首次下载 下载总数：" + DownloadResInfoQueue.Count, BeShowLog);
            DownloadOneFile();
        }
        else
        {
            LogMgr.E("DownloadRes", "DelayDown", "下载流程中断 待下载资源数为0，不该出现这种情况，版本变化了，但是没有变更任何资源" , BeShowLog);            
        }
    }

    private void DownloadOneFile()
    {

        if (DownloadResInfoQueue.Count <= 0)
        {            
            //全部下载完成
            SetState(DownloadResState.saveResInfoList2DownloadingFoler);
            return;
        }
        else
        {
            float downloadProgress = (1f - DownloadResInfoQueue.Count / DownloadFileNum)*0.9f;//下载从0-0.9f
            Messenger.Broadcast<float>("updateLoadingProgress", downloadProgress);
            Debug.LogError("真下载"+downloadProgress);
        }


        string resRelePath = DownloadResInfoQueue.Dequeue();

        Utils.SB.Append(UpdateMgr.RemoteCdnUrl);
        Utils.SB.Append("/");
        Utils.SB.Append(UnityUtil.CurPlatform);
        Utils.SB.Append(resRelePath);
        string remotePath = Utils.SB.ToString();
        Utils.ClearSB();

        Utils.SB.Append(UnityUtil.PersistentPath);
        Utils.SB.Append(DownloadCachePath);
        Utils.SB.Append(resRelePath);
        string savePath = Utils.SB.ToString();
        string saveFolderPath = FileHelper.GetDirectorName(savePath);
        FileHelper.CreateFolder(saveFolderPath);
        Utils.ClearSB();

        Action onSuccess = () =>
        {
            LogMgr.I("DownloadRes", "DownloadOneFile", "下载资源成功 remotePath：" + remotePath + " savePath:" + savePath, BeShowLog);
            //下载成功后启动下一次下载
            DownloadOneFile();
        };

        Action<string> onFaile = (error) =>
        {         
            LogMgr.E("DownloadRes", "DownloadOneFile", "下载流程中断，下载资源失败 remotePath：" + remotePath + " savePath:" + savePath, BeShowLog);
        };

        Action<int> onGetFileSize = (size) =>
        {
        };

        //单个资源的下载进度
        Action<float> onProgress = (progress) =>
        {

        };

        LogMgr.I("DownloadRes", "DelayDown", "开始下载资源 remotePath：" + remotePath+" savePath:"+savePath+" 剩余待下载资源数:"+ DownloadResInfoQueue.Count, BeShowLog);

        DownloadMgr.GetIns().StartDownload(remotePath, savePath, onSuccess, onFaile, onGetFileSize, onProgress);

    }

    /// <summary>
    /// 将remote最新的resInfoList保存到下载中目录，等待移动到沙盒位置
    /// </summary>
    private void SaveResInfoList2DownloadingFoler()
    {
        IoBuffer ib = new IoBuffer(102400);

        ib.PutInt(RemoteResInfoDic.Count);

        foreach (var p in RemoteResInfoDic)
        {
            ib.PutString(p.Key);
            ib.PutString(p.Value);
        }
        byte[] bs = ib.ToArray();
        Utils.SB.Append(UnityUtil.PersistentPath);
        Utils.SB.Append(DownloadCachePath);
        Utils.SB.Append("/resInfoList.bytes");
        string savePath = Utils.SB.ToString();
        Utils.ClearSB();
        FileHelper.WriteBytes2File_Create(savePath, bs);
        LogMgr.I("DownloadRes", "SaveResInfoList2DownloadingFoler", "将remote最新的resInfoList保存到下载中目录："+ savePath, BeShowLog);
        SetState(DownloadResState.generateDownloadOverFlagFile);
    }

    /// <summary>
    /// 生成下载完成标识
    /// </summary>
    private void GenerateDownloadOverFlagFile()
    {
        Utils.SB.Append(UnityUtil.PersistentPath);
        Utils.SB.Append(DownloadCachePath);
        Utils.SB.Append("/");
        Utils.SB.Append(DownloadOverFileName);
        string path = Utils.SB.ToString();
        Utils.ClearSB();
        FileHelper.CreateFile(path);
        LogMgr.I("DownloadRes", "GenerateDownloadOverFlagFile", "生成下载完成标识：" + path, BeShowLog);
        SetState(DownloadResState.downloadComplete);
    }
    #endregion func
}
