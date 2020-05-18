using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using NLog;

public class GameEditor : MonoBehaviour
{

    public static string ABOutputPath
    {
        get
        {
            return Path.GetDirectoryName(Application.dataPath) + "/data/";
        }
    }

    public static string StreamPath 
    {
        get 
        {
#if UNITY_ANDROID
        return Application.streamingAssetsPath;
#elif UNITY_IOS
		//注意ios，mac上需要加file:///
		return "file:///" + Application.streamingAssetsPath;		
#elif UNITY_EDITOR_OSX
        return "file:///"+Application.streamingAssetsPath;		
#elif UNITY_EDITOR_WIN
		//win下是否有file:///都能加载
		return "file:///"+Application.streamingAssetsPath;
#endif
        }
    } 

    [MenuItem("Tools/switchRes/Win")]
    public static void SwitchWinBundle()
    {
        ClearCurRes();
        CopyRes("win");
    }
    [MenuItem("Tools/switchRes/mac")]
    public static void SwitchMacBundle()
    {
        ClearCurRes();
        CopyRes("mac");
    }

    [MenuItem("Tools/switchRes/android")]
    public static void SwitchAndroidBundle()
    {
        ClearCurRes();
        CopyRes("android");
    }

    [MenuItem("Tools/switchRes/ios")]
    public static void SwitchIosBundle()
    {
        ClearCurRes();
        CopyRes("ios");
    }

    private static void ClearCurRes() 
    {
        string path = ABOutputPath+"bundle";
        DeleteFolder(path);
    }

    private static void CopyRes(string platform) 
    {
        string originalPath = ABOutputPath + "/" + platform;
        string targetPath = StreamPath + "/bundle";
        CopyDirectory(originalPath,targetPath);
        AssetDatabase.Refresh();
    }

    static void DeleteFolder(string folderPath, bool delSubFolder = true)
    {

        if (!Directory.Exists(folderPath)) return;

        if (folderPath.EndsWith("/"))
        {
            LogMgr.E("EditorHelper", "DeleteFolder", "删除文件夹路径以/结尾，错误，终止删除",false);
            return;
        }

        LogMgr.E("EditorHelper", "DeleteFolder", "待删除文件夹" + folderPath + "存在，删除文件夹及子内容",false);
        Directory.Delete(folderPath, true);//先把所有文件删除(文件夹结构还没删除)

    }

    static void CopyDirectory(string sourcePath, string destinationPath)
    {
        DirectoryInfo info = new DirectoryInfo(sourcePath);
        Directory.CreateDirectory(destinationPath);
        foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
        {
            string destName = Path.Combine(destinationPath, fsi.Name);
            if (fsi is System.IO.FileInfo)
                File.Copy(fsi.FullName, destName);
            else
            {
                Directory.CreateDirectory(destName);
                CopyDirectory(fsi.FullName, destName);
            }
        }
    }

}