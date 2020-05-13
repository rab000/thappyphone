using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// 移动文件，返回进度，可以过滤不移动的文件
/// </summary>
public class FileMove : SingletonBehaviour<FileMove>
{
    /// <summary>
    /// 移动文件夹下所有文件
    /// 返回进度
    /// </summary>
    private float FoldersFilesNum = 0;
    private float CurMoveNum = 0;
    private Listener<float> progressCB;
    private Listener OnComplete;
    private Dictionary<string, string> IgnoreFileNameDic;
    public void MoveFoldersFiles(string srcFolder, string destFolder, Listener<float> progressCB = null, Listener onComplete=null,List<string> ignoreFileNames = null)
    {
        //添加忽略文件名
        IgnoreFileNameDic = new Dictionary<string, string>();
        if (null != ignoreFileNames)
        {
            for (int i = 0; i < ignoreFileNames.Count; i++)
            {
                IgnoreFileNameDic.Add(ignoreFileNames[i], ignoreFileNames[i]);
            }
        }

        this.progressCB = progressCB;
        this.OnComplete = onComplete;
        //文件夹中文件数
        FoldersFilesNum = FileHelper.FindFileNumInFolder(srcFolder);
        if (null != ignoreFileNames)
        {
            FoldersFilesNum -= ignoreFileNames.Count;
        }          
        //当前已经移动的文件数
        CurMoveNum = 0;
        Debug.LogError("原始地址;"+srcFolder+" 目标地址:"+ destFolder);
        StartCoroutine(MoveFoldersFiles2(srcFolder, destFolder));
    }

    /// <summary>
    /// 递归遍历子文件夹移动文件夹下所有文件
    /// </summary>
    /// <param name="srcFolder"></param>
    /// <param name="destFolder"></param>
    private IEnumerator MoveFoldersFiles2(string srcFolder, string destFolder)  
    {
        //移动所有文件
        DirectoryInfo directoryInfo = new DirectoryInfo(srcFolder);
        FileInfo[] files = directoryInfo.GetFiles();

        //for (int i = 0; i < files.Length; i++)
        //{
        //i--->0 name: atest1.txt str:D:\test\a\atest1.txt fullName:D:\test\a\atest1.txt
        //Debug.LogError("i--->"+i+" name:"+files[i].Name+" str:"+files[i].ToString()+" fullName:"+ files[i].FullName);
        //}

        foreach (FileInfo file in files) // Directory.GetFiles(srcFolder)
        {
            //if (file.Extension == ".png")
            //{
            //    file.MoveTo(Path.Combine(destFolder, file.Name));
            //}

            if (IgnoreFileNameDic.ContainsKey(file.Name))
            {
                continue;
            }

            Utils.SB.Append(destFolder);
            Utils.SB.Append("/");
            Utils.SB.Append(file.Name);
            string path = Utils.SB.ToString();
            Debug.LogError("------->path:"+path);
            Utils.ClearSB();
            if (FileHelper.BeFileExist(path))
            {
                FileHelper.DelFile(path);
            }

            file.MoveTo(path);
            //计算进度
            CurMoveNum += 1;

            float progress = CurMoveNum / FoldersFilesNum;

            progressCB?.Invoke(progress);

            if (CurMoveNum == FoldersFilesNum)
            {
                OnComplete?.Invoke();
            }

            yield return 0;

        }

        yield return 0;

        //移动所有子文件夹
        DirectoryInfo[] subDirectorys = directoryInfo.GetDirectories();
        for (int i = 0; i < subDirectorys.Length; i++)
        {
            //i--->0 name: sub path:D:\test\a\sub
            //Debug.LogError("i--->" + i + " name:" + subDirectorys[i].Name +" path:"+ subDirectorys[i].FullName);
            Utils.SB.Append(srcFolder);
            Utils.SB.Append("/");
            Utils.SB.Append(subDirectorys[i].Name);
            string _srcFolder = Utils.SB.ToString();
            Utils.ClearSB();

            Utils.SB.Append(destFolder);
            Utils.SB.Append("/");
            Utils.SB.Append(subDirectorys[i].Name);
            string _destFolder = Utils.SB.ToString();
            if (!FileHelper.BeFolderExist(_destFolder))
            {
                FileHelper.CreateFolder(_destFolder);
            }
            Utils.ClearSB();
            StartCoroutine(MoveFoldersFiles2(_srcFolder, _destFolder));
            
        }

    }

}
