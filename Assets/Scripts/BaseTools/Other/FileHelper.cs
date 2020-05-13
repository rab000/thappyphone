using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine.Networking;
using NLog;

public class FileHelper{

	static FileHelper ins;

	public static FileHelper GetIns(){
		if(ins==null)ins = new FileHelper();
		return ins;
	}

    #region file read

    public static string ReadStringFromFile(string url)
    {
        return ReadStringFromFile(url,Encoding.UTF8);
    }

    public static string ReadStringFromFile(string url,Encoding encode)
    {
        if (File.Exists(url))
            return File.ReadAllText(url, encode);
        else
        {
            Debug.LogError("FileHelper.ReadStringFromFile 读取url:"+url+"失败，文件不存在！");
            return null;
        }
            
    }

    public static string[] ReadStrLinesFromFile(string url)
    {
        return ReadStrLinesFromFile(url,Encoding.UTF8);
    }

    public static string[] ReadStrLinesFromFile(string url,Encoding encode)
    {
        if (File.Exists(url))
            return File.ReadAllLines(url, encode);
        else
        {
            Debug.LogError("FileHelper.ReadStrLinesFromFile 读取url:" + url + "失败，文件不存在！");
            return null;
        }
    }

    //从apk包里读取时要用这个方法
    /// <summary>
    /// 从apk包里读或者从远程读取
    /// </summary>
    /// <param name="url"></param>
    /// <param name="callback"></param>
    /// <param name="errorBack"></param>
    public void ReadBytesFromApkFile(string url,Action<byte[]> callback = null,Action<string> errorBack = null){
		ThreadManager.GetIns().StartCoroutine(go(url,callback,errorBack));
	}
	IEnumerator go(string url,Action<byte[]> callback = null,Action<string> errorBack = null){


        UnityWebRequest www = UnityWebRequest.Get(url);
        
		yield return www.SendWebRequest();

        if (www.isHttpError || www.isNetworkError)
        {
            Debug.LogError("FileHelper.go() www.error=" + www.error + " url:" + url + " isHttpError:" + www.isHttpError + " isNetworkError:" + www.isNetworkError);
            errorBack?.Invoke(www.error);
            yield break;
        }
        else
        {
            callback?.Invoke(www.downloadHandler.data);
        }

        //WWW www = new WWW(url);
  //      if (www.error!=null){
		//	Debug.LogError("FileHelper.go() www.error="+www.error+" url:"+url);
		//	if(null!=errorBack)errorBack(www.error);
		//	//if(null!=callback)callback(null);//TODO 这句不知道因为什么写的了，但现在会引发问题，所以去掉
		//	yield break;
		//}else{
		//	if(null!=callback)callback(www.bytes);
		//}

		//www.Dispose();
		//www = null;
	}

    public void ReadStringFromApkFile(string url, Action<string> callback = null, Action<string> errorBack = null)
    {
        ThreadManager.GetIns().StartCoroutine(go1(url, callback, errorBack));
    }
    IEnumerator go1(string url, Action<string> callback = null, Action<string> errorBack = null)
    {


        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if (www.isHttpError || www.isNetworkError)
        {
            Debug.LogError("FileHelper.go() www.error=" + www.error + " url:" + url + " isHttpError:" + www.isHttpError + " isNetworkError:" + www.isNetworkError);
            errorBack?.Invoke(www.error);
            yield break;
        }
        else
        {
            callback?.Invoke(www.downloadHandler.text);
        }

        //WWW www = new WWW(url);
        //      if (www.error!=null){
        //	Debug.LogError("FileHelper.go() www.error="+www.error+" url:"+url);
        //	if(null!=errorBack)errorBack(www.error);
        //	//if(null!=callback)callback(null);//TODO 这句不知道因为什么写的了，但现在会引发问题，所以去掉
        //	yield break;
        //}else{
        //	if(null!=callback)callback(www.bytes);
        //}

        //www.Dispose();
        //www = null;
    }

    /// <summary>
	/// 从文件读bytes
	/// </summary>
	/// <param name="path">文件夹路径</param>
	/// <param name="fileName">文件名</param>
	public static byte[] ReadBytesFromFile(string directorPath, string fileName)
    {
        string folderPath = Path.GetDirectoryName(directorPath);
        if (!Directory.Exists(folderPath))
        {
            Debug.LogError("GEditorDataManager.Get--->读取文件失败，找不到文件夹" + folderPath);
            return null;
        }
        string filePath = folderPath + "/" + fileName;
        return ReadBytesFromFile(filePath);
    }

    /// <summary>
    /// 从文件读bytes
    /// </summary>
    /// <param name="path">文件夹路径</param>
    /// <param name="fileName">文件名</param>
    public static byte[] ReadBytesFromFile(string url)
    {
        if (!File.Exists(url))
        {
            Debug.LogError("GEditorDataManager.Get--->读取文件失败，找不到文件" + url);
            return null;
        }
        FileStream fs = new FileStream(url, FileMode.Open);
        byte[] bs = new byte[fs.Length];
        fs.Read(bs, 0, bs.Length);
        fs.Close();
        return bs;
    }

    #endregion

    #region file write
    public static void WriteBytes2File(string path, string msg)
    {
        //using (FileStream fs = new FileStream(@"d:\test.txt", FileMode.OpenOrCreate, FileAccess.Write))
        using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.BaseStream.Seek(0, SeekOrigin.End);
                //sw.WriteLine("{0}\n", msg, DateTime.Now);
                sw.WriteLine(msg);
                //sw.Write(msg);
                sw.Flush();
            }
        }
    }
    /// <summary>
    /// 创建文件并写入
    /// </summary>
    /// <param name="url"></param>
    /// <param name="bytes"></param>
    public static void WriteBytes2File_Create(string url, byte[] bytes)
    {
        if (File.Exists(url))
            File.Delete(url);

        FileStream fs = new FileStream(url, FileMode.Create);
        fs.Write(bytes, 0, bytes.Length);
        fs.Close();
        fs.Dispose();
    }

    /// <summary>
    /// 附加写入bytes
    /// </summary>
    /// <param name="url"></param>
    /// <param name="bytes"></param>
    public static void WriteBytes2File_Append(string url, byte[] bytes)
    {
        if (!File.Exists(url))
            File.Create(url).Dispose();
        //Debug.LogError ("写入------->content:"+content);
        FileStream fs = new FileStream(url, FileMode.Append);
        fs.Write(bytes, 0, bytes.Length);
        fs.Close();
        fs.Dispose();
    }

    /// <summary>
    /// 附加写入string
    /// </summary>
    /// <param name="url"></param>
    /// <param name="content"></param>
    public static void WriteString2File_Append(string url, string content)
    {
        if (!File.Exists(url))
            File.Create(url).Dispose();

        FileStream fs = new FileStream(url, FileMode.Append);

        StreamWriter sw = new StreamWriter(fs);

        sw.WriteLine(content);

        sw.Close();

        fs.Close();

        sw.Dispose();

        fs.Dispose();

    }
    #endregion

    #region file find
    public static long GetFileSize(string filePath)
    {
        long val = 0;
        if (File.Exists(filePath))
        {
            try
            {
                using (var fileStream = File.OpenRead(filePath))
                {
                    val = fileStream.Length;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }
        else
        {
            Debug.LogError(string.Format("文件{0}不存在", filePath));
        }
        return val;
    }

    /// <summary>
    /// 获取文件MD5
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string GetFileMd5(string filePath)
    {
        string md5 = "";
        if (File.Exists(filePath))
        {
            try
            {
                using (var fileStream = File.OpenRead(filePath))
                {
                    MD5 t5 = MD5.Create();
                    byte[] bytes = t5.ComputeHash(fileStream);
                    md5 = System.BitConverter.ToString(bytes).Replace("-", "");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }
        else
        {
            Debug.LogError(string.Format("文件{0}不存在", filePath));
        }
        return md5;
    }

    /// <summary>
    /// 获取文件hash
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string GetFileHash(string filePath)
    {
        return FileHash.MD5File(filePath);
    }

    public static bool BeFileExist(string filePath)
    {
        return File.Exists(filePath);
    }

    public static void BeFileSame(string file1Path, string file2Path)
    {
        //计算第一个文件的哈希值
        var hash = System.Security.Cryptography.HashAlgorithm.Create();
        var stream_1 = new System.IO.FileStream(file1Path, System.IO.FileMode.Open);
        byte[] hashByte_1 = hash.ComputeHash(stream_1);
        stream_1.Close();
        //计算第二个文件的哈希值
        var stream_2 = new System.IO.FileStream(file2Path, System.IO.FileMode.Open);
        byte[] hashByte_2 = hash.ComputeHash(stream_2);
        stream_2.Close();

        //比较两个哈希值
        if (BitConverter.ToString(hashByte_1) == BitConverter.ToString(hashByte_2))
            Debug.LogError("两个文件相同");
        else
            Debug.LogError("两个文件不同");

    }

    /// <summary>
	/// 查找目录下所有文件
	/// 输出目录eg:Assets/NEditor/RoleEditor/Res/Female/Materials\female_top-2_orange.mat
	/// </summary>
	/// <returns>The all files.</returns>
	/// <param name="folderPath">文件夹目录，eg:Assets/NEditor/RoleEditor/Res/Female/Materials</param>
	public static string[] FindFilePathsInFolder(string folderPath)
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

    /// <summary>
    /// 查找文件夹下所有文件的数量
    /// </summary>
    /// <param name="folderPath"></param>
    /// <returns></returns>
    public static int FindFileNumInFolder(string folderPath)
    {
        string[] fileURLs = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);
        return fileURLs.Length;
    }

    #endregion

    #region file operate

    /// <summary>
	/// Creates the file.
	/// </summary>
	/// <param name="path">Path.</param>
	public static void DeleteFileIfExists(string path)
    {
        if (!File.Exists(path)) return;
        LogMgr.I("EditorHelper", "DeleteFileIfExists", "待删除文件" + path + "存在，删除之");
        File.Delete(path);
    }

    public static void CreateFile(string path)
    {
        byte[] bs = new byte[1];
        FileHelper.WriteBytes2File_Create(path, bs);

        //if (!File.Exists(path))
        //    File.Create(path);
        //else
        //{
        //    Debug.LogError("FileHelper createFile faile file exist path:"+path);
        //}
    }

    /// <summary>
    /// 注意这里必须确保destPath包含的所有目录存在，否则会报错
    /// </summary>
    /// <param name="srcPath"></param>
    /// <param name="destPath"></param>
    public static void MoveFile(string srcPath, string destPath)
    {
        if (!BeFileExist(destPath))
        {
            Debug.LogError("FileHelper.MoveFile 移动的文件不存在,srcPath:"+srcPath);
            return;
        }

        if (BeFileExist(destPath))
        {
            DelFile(destPath);
        }

        FileInfo fi = new FileInfo(srcPath);
        fi.MoveTo(destPath);
    }

    /// <summary>
    /// file copy
    /// </summary>
    /// <param name="_Src_Path"></param>
    /// <param name="_Target_Path"></param>
    /// <param name="_Imperative"> 如果目标文件是否先删除再copy</param>
    public static void CopyFile(string _Src_Path, string _Target_Path, bool _Imperative = false)
    {
        if (_Imperative)
        {
            if (File.Exists(_Target_Path))
            {
                File.Delete(_Target_Path);
            }
        }
        if (File.Exists(_Src_Path))
            File.Copy(_Src_Path, _Target_Path);
        else
        {
            Debug.LogError("拷贝文件出错");
        }
    }

    public static void DelFile(string path)
    {
        if (BeFileExist(path))
        {
            File.Delete(path);
        }
    }

    #endregion

    #region absPath relePath fileName directorName
    /// <summary>
    /// 得到当前路径(可以是文件或文件夹路径)上一级目录的路径
    /// </summary>
    /// <returns>The parent folder path.</returns>
    /// <param name="path">Path.</param>
    public static string GetParentFolderPath(string path)
    {
        return Path.GetDirectoryName(path);
    }


    /// <summary>
    /// 得到路径中最后位置的名称(可以是文件名或者是文件夹名,最后一个/后面的内容如果是文件则包括后缀名)
    /// </summary>
    /// <returns>The file name from path.</returns>
    /// <param name="path">Path.</param>
    /// <param name="beWithoutExtension">是否包括扩展名</c> be without extension.</param>
    public static string GetFileNameFromPath(string path, bool beWithoutExtension = false)
    {

        if (beWithoutExtension)
        {
            return Path.GetFileNameWithoutExtension(path);
        }
        return Path.GetFileName(path);
    }

    /// <summary>
    /// 获取文件路径所在folder路径
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetDirectorName(string filePath)
    {
        return Path.GetDirectoryName(filePath);
    }

    #endregion

    #region Folder
    public static bool BeFolderExist(string folderPath)
    {
        return Directory.Exists(folderPath);
    }

    /// <summary>
    /// Creates the folder.
    /// </summary>
    /// <returns>The folder.</returns>
    /// <param name="folderPath">Folder path.</param>
    /// <param name="bRebuild">If set to <c>true</c> 目录存在则先删除再重建.</param>
    public static void CreateFolder(string folderPath)
    {
        if (!Directory.Exists(folderPath))
        {
            LogMgr.I("FileHelper", "CreateFolder", "文件夹" + folderPath + "不存在，创建文件夹");
            Directory.CreateDirectory(folderPath);
        }
    }

    /// <summary>
    /// 删除文件夹
    /// 注意:
    /// 
    /// Directory.Delete第一个参数结尾不能是/否则异常提示找不到文件夹
    /// 
    /// Directory.Delete第二个参数
    /// =true只会删除文件夹下的所有文件及文件夹
    /// =false只有文件夹为null时才能删除自己,如果文件夹不为null，还设置为false，那么报错io异常
    /// 
    /// Directory.GetDirectories说明
    /// 如果传入参数如"Asset/FolderA"  那么返回的string[] 第一个就是"Asset/FolderA"
    /// 如果传入参数如"Asset/FolderA/" 那么返回的string[] 只有子folder
    /// 
    /// delSubFolder是否删除文件夹(目录结构),如果是false，只删除文件保留文件夹结构
    public static void DeleteFolder(string folderPath, bool delSubFolder = true)
    {

        if (!Directory.Exists(folderPath)) return;

        if (folderPath.EndsWith("/"))
        {
            //LogMgr.E("EditorHelper","DeleteFolder","删除文件夹路径以/结尾，错误，终止删除");
            return;
        }

        //LogMgr.E ("EditorHelper","DeleteFolder","待删除文件夹"+folderPath+"存在，删除文件夹及子内容");
        Directory.Delete(folderPath, true);//先把所有文件删除(文件夹结构还没删除)

    }

    /// <summary>
    /// 获取当前路径的所有子目录的绝对path
    /// </summary>
    /// <returns>The sub folder path.</returns>
    /// <param name="path">Path.</param>
    public static string[] GetSubFolderPaths(string path)
    {
        return Directory.GetDirectories(path);
    }

    /// <summary>
    /// 获取当前路径的所有文件的绝对path
    /// </summary>
    /// <returns>The sub folder path.</returns>
    /// <param name="path">Path.</param>
    public static string[] GetSubFilesPaths(string path)
    {
        return Directory.GetFiles(path);
    }


    /// <summary>
    /// 递归删除目录
    /// </summary>
    /// <param name="dir">要删除的目录</param>
    public static void DeleteFolder1(string dir)
    {
        foreach (string d in Directory.GetFileSystemEntries(dir))
        {
            if (File.Exists(d))
            {
                FileInfo fi = new FileInfo(d);
                if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                    fi.Attributes = FileAttributes.Normal;
                File.Delete(d);
            }
            else
            {
                DirectoryInfo d1 = new DirectoryInfo(d);
                if (d1.GetFiles().Length != 0)
                {
                    DeleteFolder1(d1.FullName);////递归删除子文件夹
                }
                Directory.Delete(d);
            }
        }
    }
    /// <summary>
    /// 深度拷贝目录
    /// </summary>
    /// <param name="sourcePath">源目录</param>
    /// <param name="destinationPath">目标目录</param>
    public static void CopyDirectory(string sourcePath, string destinationPath)
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

    
    /// <summary>
    /// 递归遍历子文件夹移动文件夹下所有文件
    /// 这个方法适合在编辑器中，没有返回移动进度
    /// </summary>
    /// <param name="srcFolder"></param>
    /// <param name="destFolder"></param>
    public static void MoveFoldersFiles(string srcFolder, string destFolder)
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
            Utils.SB.Append(destFolder);
            Utils.SB.Append("/");
            Utils.SB.Append(file.Name);
            string path = Utils.SB.ToString();
            //Debug.LogError("------->path:"+path);
            Utils.ClearSB();
            if (BeFileExist(path))
            {
                DelFile(path);
            }
            file.MoveTo(path);
            
        }

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
            if (!BeFolderExist(_destFolder))
            {
                CreateFolder(_destFolder);
            }
            Utils.ClearSB();
            MoveFoldersFiles(_srcFolder, _destFolder);

        }

    }

    /// <summary>
    /// 得到项目的名称
    /// </summary>
    public static string ProjectName
    {
        get
        {
            foreach (string arg in System.Environment.GetCommandLineArgs())
            {
                if (arg.StartsWith("project"))
                {
                    return arg.Split('-')[1];
                }
            }
            return "test";
        }
    }


    #endregion

}