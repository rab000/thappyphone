using NLog;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 解压包内资源到沙盒
/// </summary>
public class UnCompress : MonoBehaviour
{
    bool BeShowLog = true;

    #region var

    /// <summary>
    /// 沙盒resVer是否存在    
    /// </summary>
    private bool BePersisResVerExist;

    /// <summary>
    /// 用于临时存储stream中resInfoList用于逐个取出stream中的资源到沙盒
    /// </summary>
    Dictionary<string, string> resInfoDicInStream = new Dictionary<string, string>();

    #endregion var

    #region fsm

    public enum UnCompressState
    {
        checkPersistResVer,//查看是否存在沙盒resVer(是否释放过资源),存在就读取并保存
        checkStreamResVer,//读取沙盒resVer
        compareResVerPersist_Stream,//比较沙盒和包内resVer 
        readStreamResInfoList,//获取steam中的resInfoList
        unCompressStreamRes,//释放stream中的res到沙盒
        readPersistResVerAfterUnCompress,//解压后读取沙盒resVer
        unCompressComplete,//释放阶段结束
        nil
    }

    private UnCompressState curState = UnCompressState.nil;

    private UnCompressState preState = UnCompressState.nil;

    public void SetState(UnCompressState state)
    {
        if (state == curState) return;

        LogMgr.I("UnCompress","SetState","SetState切换状态到:" + state,BeShowLog);

        preState = curState;
        StateExit(preState);
        curState = state;

        switch (curState)
        {
            case UnCompressState.checkPersistResVer:
                CheckPersistResVer();
                break;
            case UnCompressState.checkStreamResVer:
                CheckStreamResVer();
                break;
            case UnCompressState.compareResVerPersist_Stream:
                CompareLocalResVer();
                break;
            case UnCompressState.readStreamResInfoList:
                ReadStreamResInfoList();
                break;
            case UnCompressState.unCompressStreamRes:
                UnCompressStreamRes2Persistent();
                break;
            case UnCompressState.readPersistResVerAfterUnCompress:
                ReadPersistResVerAfterUnCompress();
                break;
            case UnCompressState.unCompressComplete:
                Clear();
                ntools.Messenger.Broadcast<float>("updateLoadingProgress", 1f);
                UpdateMgr.GetIns().SetState(UpdateMgr.CheckVerState.downLoad);           
                break;
        }
    }

    private void StateExit(UnCompressState state)
    {
        switch (state)
        {
            case UnCompressState.checkPersistResVer:
                ntools.Messenger.Broadcast<float>("updateLoadingProgress", 0.05f);
                
                break;
            case UnCompressState.checkStreamResVer:
                ntools.Messenger.Broadcast<float>("updateLoadingProgress", 0.1f);
                
                break;
            case UnCompressState.compareResVerPersist_Stream:
                ntools.Messenger.Broadcast<float>("updateLoadingProgress", 0.15f);
                
                break;
            case UnCompressState.readStreamResInfoList:
                ntools.Messenger.Broadcast<float>("updateLoadingProgress", 0.2f);
                
                break;
            case UnCompressState.unCompressStreamRes:
                //UnCompressStreamRes2Persistent();
                break;
            case UnCompressState.unCompressComplete:                               
                break;
        }
    }

    #endregion fsm

    #region func

    public void Clear()
    {
        resInfoDicInStream.Clear();       
    }

    /// <summary>
    /// //查看是否存在沙盒resVer(是否释放过资源),存在就读取并保存
    /// </summary>
    private void CheckPersistResVer()
    {        
        Utils.SB.Append(UnityUtil.PersistentPath);
        Utils.SB.Append("/resVer.txt");
        string persistResVerPath = Utils.SB.ToString();
        Utils.ClearSB();
        BePersisResVerExist = FileHelper.BeFileExist(persistResVerPath);

        if (BePersisResVerExist)
        {
            //释放过资源，比较下沙盒版本和包内版本
            //强更时沙盒资源版本可能比包里资源老，
            //要从包里更新下
            //这样即使需要热更，更新的资源也更少

            //获取沙盒ResVer           
            Utils.SB.Append(UnityUtil.PersistentPath);
            Utils.SB.Append("/resVer.txt");
            string path = Utils.SB.ToString();
            UpdateMgr.PersistentResVer = FileHelper.ReadStringFromFile(path);
            LogMgr.I("UnCompress", "CheckPersistResVer", "沙盒resVer存在 resVer:"+ UpdateMgr.PersistentResVer, BeShowLog);
            Utils.SB.Clear();
            SetState(UnCompressState.checkStreamResVer);
        }
        else
        {
            LogMgr.I("UnCompress", "CheckPersistResVer", "沙盒resVer path:"+ persistResVerPath + "不存在,准备读取包内resInfoList并释放资源", BeShowLog);
            //没释放过资源，释放stream中资源到沙盒
            SetState(UnCompressState.readStreamResInfoList);
        }
    }

    /// <summary>
    /// 读取stream中resVer
    /// </summary>
    private void CheckStreamResVer()
    {
        Utils.SB.Append(UnityUtil.StreamingAssetsPath);
        Utils.SB.Append("/resVer.txt");
        string streamResVerPath = Utils.SB.ToString();
        Utils.ClearSB();

        LogMgr.I("UnCompress", "CheckStreamResVer", "开始读取包内resVer path:" + streamResVerPath, BeShowLog);

        Action<byte[]> onSuccess = (bs) =>
        {
            UpdateMgr.StreamResVer = TypeConvertUtils.Bytes2String(bs);
            LogMgr.I("UnCompress", "CheckStreamResVer", "读取到包内resVer:" + UpdateMgr.StreamResVer, BeShowLog);
            SetState(UnCompressState.compareResVerPersist_Stream);
        };

        Action<string> onFaile = (error) =>
        {
            LogMgr.E("UnCompress", "CheckStreamResVer", " 解压流程中断 onFaile error:" + error,BeShowLog);
        };

        FileHelper.GetIns().ReadBytesFromApkFile(streamResVerPath, onSuccess, onFaile);
    }

    private void CompareLocalResVer()
    {
        int result = VersionUtils.CompareVersion(UpdateMgr.PersistentResVer, UpdateMgr.StreamResVer);
        LogMgr.I("UnCompress", "CompareLocalResVer"," 沙盒版本:"+ UpdateMgr.PersistentResVer+" 包内版本:"+ UpdateMgr.StreamResVer+"比较结果:"+result,BeShowLog);
        switch (result)
        {
            case 0://相同，不需要再次释放资源
            case -1://persistentResVer大，不需要释放资源
                SetState(UnCompressState.unCompressComplete);
                break;
            case 1://streamResVer大,释放资源
                SetState(UnCompressState.readStreamResInfoList);
                break;
        }
    }

    /// <summary>
    /// 获取stream中resInfoList
    /// </summary>
    private void ReadStreamResInfoList()
    {
        Utils.SB.Append(UnityUtil.StreamingAssetsPath);
        Utils.SB.Append("/resInfoList.bytes");
        string resInfoListPathInStream = Utils.SB.ToString();
        Utils.ClearSB();
        LogMgr.I("UnCompress", "ReadSteamResInfoList.onSuccess", "开始读取包内resInfoList path:" + resInfoListPathInStream, BeShowLog);
        //读取stream里资源描述列表
        Action<byte[]> onSuccess = (bs) =>
        {
            IoBuffer ib = new IoBuffer(102400);
            ib.PutBytes(bs);
            int count = ib.GetInt();
            LogMgr.I("UnCompress", "ReadSteamResInfoList.onSuccess", "读取包内resInfoList成功 资源数:" + count, BeShowLog);
            for (int i = 0; i < count; i++)
            {
                string relePath = ib.GetString();
                string hash = ib.GetString();
                resInfoDicInStream.Add(relePath, hash);
            }
            SetState(UnCompressState.unCompressStreamRes);
        };

        Action<string> onFaile = (error) =>
        {
           LogMgr.E("UnCompress","ReadSteamResInfoList.onFaile","读取包内resInfoList失败，解压中断 onFaile error:" + error,BeShowLog);
        };

        FileHelper.GetIns().ReadBytesFromApkFile(resInfoListPathInStream, onSuccess, onFaile);
    }

    /// <summary>
    /// 解压释放包内资源到沙盒
    /// </summary>
    private void UnCompressStreamRes2Persistent()
    {
        LogMgr.I("UpdateMgr", "UnCompressStreamRes2Persistent", "开始释放包内资源", BeShowLog);

        Listener onUnCompressOver = () =>
        {
            LogMgr.I("UpdateMgr", "UnCompressStreamRes2Persistent", "解压包内资源完毕:", BeShowLog);
            SetState(UnCompressState.readPersistResVerAfterUnCompress);
        };

        CallbackMgr cbMgr = new CallbackMgr(onUnCompressOver);

        cbMgr.ProgressEvent = (progress) => {

            //20-100的进度转换，就是20+80*progress           
            float p = 0.2f + 0.8f * progress;
            ntools.Messenger.Broadcast<float>("updateLoadingProgress", p);
            LogMgr.I("UpdateMgr", "UnCompressStreamRes2Persistent", "释放进度 progress:" + progress, BeShowLog);
        };

        foreach (var p in resInfoDicInStream)
        {
            cbMgr.Add(BuildCB(p.Key));

        }//foreach结尾


        //解压resVer.txt
        cbMgr.Add(BuildCB("/resVer.txt"));
        //解压resInfoList.bytes
        cbMgr.Add(BuildCB("/resInfoList.bytes"));

        cbMgr.Start();

    }

    /// <summary>
    /// 这里提取resVer有两种情况
    ///1 沙盒原本没有resVer，就是首次解压
    ///2 沙盒版本低于stream，覆盖安装
    ///与前面提取沙盒resVer是不同函数
    ///这里如果重新读取，那么上面两种情况下，沙盒resVer就没读取过
    ///后面下载比较沙盒和远程resVer时，内存中存的沙盒resVer就是null
    /// </summary>
    private void ReadPersistResVerAfterUnCompress()
    {
        Utils.SB.Append(UnityUtil.PersistentPath);
        Utils.SB.Append("/resVer.txt");
        string persistResVer = Utils.SB.ToString();
        Utils.ClearSB();
        BePersisResVerExist = FileHelper.BeFileExist(persistResVer);

        if (BePersisResVerExist)
        {
            //获取沙盒ResVer           
            Utils.SB.Append(UnityUtil.PersistentPath);
            Utils.SB.Append("/resVer.txt");
            string path = Utils.SB.ToString();
            UpdateMgr.PersistentResVer = FileHelper.ReadStringFromFile(path);
            LogMgr.I("UnCompress", "ReadPersistResVerAfterUnCompress", "释放过stream资源后读取沙盒resVer:" + UpdateMgr.PersistentResVer, BeShowLog);
            Utils.SB.Clear();
            SetState(UnCompressState.unCompressComplete);
        }
        else
        {
            //已经释放过资源resVer不可能不存在
            LogMgr.E("UnCompress", "ReadPersistResVerAfterUnCompress", "释放过stream资源后找不到沙盒resVer，解压中断", BeShowLog); ;
        }
    }

    private BaseCallback BuildCB(string relePath)
    {
        Utils.SB.Append(UnityUtil.StreamingAssetsPath);
        Utils.SB.Append(relePath);
        string path = Utils.SB.ToString();
        Utils.ClearSB();
        BaseCallback cb = new BaseCallback();
        cb.OnStart = () => {
            
            Action<byte[]> onSuccess = (bs) =>
            {
                //读取到stream中的bundle后存到persist中
                Utils.SB.Append(UnityUtil.PersistentPath);
                Utils.SB.Append(relePath);
                string bundlePath = Utils.SB.ToString();
                string parentFolderPath = FileHelper.GetDirectorName(bundlePath);
                FileHelper.CreateFolder(parentFolderPath);
                //LogMgr.I("UpdateMgr", "BuildCB", "解压资源成功 原始路径:"+ path+" 目标路径:" + bundlePath+" 文件夹路径:"+ parentFolderPath, BeShowLog);
                FileHelper.WriteBytes2File_Create(bundlePath, bs);
                Utils.SB.Clear();
                cb.End();
            };
            Action<string> onFaile = (error) =>
            {
                LogMgr.E("UpdateMgr", "BuildCB", "解压资源 原始路径:" + path + "失败，解压中断", BeShowLog);               
            };

            //LogMgr.I("UpdateMgr", "BuildCB", "开始解压资源:" + path, BeShowLog);
            FileHelper.GetIns().ReadBytesFromApkFile(path, onSuccess, onFaile);
        };

        return cb;
    }

    #endregion func
}
