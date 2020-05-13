
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using System.IO;
using NLog;
/// <summary>
/// 将下载中目录下载好的内容覆盖到沙盒资源位置
/// </summary>
public class CopyRes :MonoBehaviour
{
    private bool BeShowLog = true;

    private const string DownloadCachePath = "/downloadCache";

    private const string DownloadOverFileName = "downloadOverFlag.txt";

    public void tStart()
    {
        Utils.SB.Append(UnityUtil.PersistentPath);
        Utils.SB.Append(DownloadCachePath);
        Utils.SB.Append("/");
        Utils.SB.Append(DownloadOverFileName);
        string path = Utils.SB.ToString();
        Utils.ClearSB();

        if (!FileHelper.BeFileExist(path))
        {
            LogMgr.I("CopyRes","tStart", "下载中目录不存在下载完成标志，掠过资源覆盖阶段，直接进入updateComplete");
            //没有下载完成标识文件，不需要复制过程，
            UpdateMgr.GetIns().SetState(UpdateMgr.CheckVerState.updateComplete);
            return;
        }

        //NTODO 这里需要返回进度，另外要最后再移动resVer

        //有下载完成标识，准备开始移动覆盖文件
        Utils.SB.Append(UnityUtil.PersistentPath);
        Utils.SB.Append(DownloadCachePath);
        string _srcPath = Utils.SB.ToString();
        Utils.ClearSB();

        Listener<float> progressCB = (progress) =>
        {
            //发出进度事件           
            float progress1 = progress * 0.9f;
            LogMgr.I("CopyRes", "tStart", "移动文件夹进度:" + progress+" 整体copy进度:"+ progress1, BeShowLog);
            ntools.Messenger.Broadcast<float>("updateLoadingProgress", progress1);
        };

        Listener onComplete = () =>
        {
            LogMgr.I("CopyRes", "tStart", "移动文件夹完毕", BeShowLog);
            ntools.Messenger.Broadcast<float>("updateLoadingProgress", 0.9f);
            ProcessResVer();
        };

        List<string> ignroeFileList = new List<string>();
        ignroeFileList.Add("resVer.txt");

        LogMgr.I("CopyRes", "tStart","移动srcPath:"+ _srcPath+" 到目标路径:"+ UnityUtil.PersistentPath,BeShowLog);
        FileMove.GetIns().MoveFoldersFiles(_srcPath, UnityUtil.PersistentPath, progressCB, onComplete, ignroeFileList);

    }

    private void ProcessResVer()
    {
        //最后更新沙盒版本号
        Utils.SB.Append(UnityUtil.PersistentPath);
        Utils.SB.Append(DownloadCachePath);
        Utils.SB.Append("/resVer.txt");
        string srcResVerPath = Utils.SB.ToString();
        Utils.SB.Clear();

        Utils.SB.Append(UnityUtil.PersistentPath);
        Utils.SB.Append("/resVer.txt");
        string destResVerPath = Utils.SB.ToString();
        Utils.SB.Clear();

        LogMgr.I("CopyRes", "ProcessResVer", "移动resVer:" + srcResVerPath + " 到目标路径:" + destResVerPath, BeShowLog);
        FileHelper.MoveFile(srcResVerPath, destResVerPath);

        ntools.Messenger.Broadcast<float>("updateLoadingProgress", 1f);

        //清理下载中目录，其实不需要清理，因为都移走了
        UpdateMgr.GetIns().SetState(UpdateMgr.CheckVerState.updateComplete);
    }
    
}
