using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

//下载句柄，自定义部分操作和数据
public class TDownloadHandler : DownloadHandlerScript
{
    //文件流，用来写入文件数据
    private FileStream fs;  

    //要下载的文件总长度
    private int _ContentLength = 0;  
    private int ContentLength
    {
        get { return _ContentLength; }
    }

    //已经下载的数据长度
    private int _DownedLength = 0;   
    public int DownedLength
    {
        get { return _DownedLength; }
    }

    //要保存的文件名称，带扩展名
    private string _FileName;    
    public string FileName
    {
        get { return _FileName; }
    }

    //下载中的临时文件名，可自定义：fileName+.temp
    public string FileNameTemp
    {
        get { return _FileName + ".temp"; }
    }

    //要保存的文件路径
    private string _SavePath = null;
    //保存的文件目录名称
    public string SavePath
    {
        get { return _SavePath.Substring(0, _SavePath.LastIndexOf('/')); }
    }
		
	
    #region regist event

	private event Action<int> EventTotalLength = null; //接收到数据总长度的事件回调，传递的参数是文件总大小，单位：字节

	private event Action<float> EventProgress = null;    //进度通知事件，传递的是进度浮点数

	private event Action<string> EventComplete = null;  //完成后的事件回调，传递的参数是文件路径

	/// <summary>
	/// 注册收到文件总长度的事件，传递的参数是文件总大小，单位：字节
	/// </summary>
	/// <param name="back">Back.</param>
    public void RegisteReceiveTotalLengthBack(Action<int> back)
    {
        if (back != null)
            EventTotalLength += back;
    }

	/// <summary>
	/// 注册进度事件，传递的是进度浮点数
	/// </summary>
	/// <param name="back">Back.</param>
    public void RegisteProgressBack(Action<float> back)
    {
        if (back != null)
            EventProgress += back;
    }

	/// <summary>
	/// 注册下载完成后的事件，传递的参数是文件路径
	/// </summary>
	/// <param name="back">Back.</param>
    public void RegisteCompleteBack(Action<string> back)
    {
        if (back != null)
            EventComplete += back;
    }

    #endregion

	#region ovveride
    /// <summary>
    /// 初始化下载句柄，定义每次下载的数据上限为200kb
    /// </summary>
    /// <param name="filePath">保存到本地的文件路径</param>
    public TDownloadHandler(string filePath) : base(new byte[1024 * 200])
    {
        _SavePath = filePath.Replace('\\','/');
        _FileName = _SavePath.Substring(_SavePath.LastIndexOf('/') + 1);  //获取文件名

        this.fs = new FileStream(_SavePath + ".temp", FileMode.Append, FileAccess.Write);    //文件流操作的是临时文件，结尾添加.temp扩展名
        _DownedLength = (int)fs.Length;  //设置已经下载的数据长度
        //fs.Position = _downedLength; //设置继续写入数据的起始位置
    }
		
    /// <summary>
    /// 当从网络接收数据时的回调，每帧调用一次
    /// </summary>
    /// <param name="data">接收到的数据字节流，总长度为构造函数定义的200kb，并非所有的数据都是新的</param>
    /// <param name="dataLength">接收到的数据长度，表示data字节流数组中有多少数据是新接收到的，即0-dataLength之间的数据是刚接收到的</param>
    /// <returns>返回true表示当下载正在进行，返回false表示下载中止</returns>
    protected override bool ReceiveData(byte[] data, int dataLength)
    {
        //Debug.Log("接收到的数据长度:" + ((float)(dataLength / 1024)).ToString("0.0") + "KB/" + ((float)(data.Length / 1024)).ToString("0.0") + "KB");
        if (data == null || data.Length == 0)
        {
            Debug.Log("没有获取到数据缓存！");
            return false;
        }
        fs.Write(data, 0, dataLength);
        _DownedLength += dataLength;
        //Debug.Log("数据进度=>" + ((float)(_downedLength / 1024)).ToString("0.0") + "KB/" + ((float)(_contentLength / 1024)).ToString("0.0") + "KB");

        if (EventProgress != null)
            EventProgress.Invoke((float)_DownedLength / _ContentLength);   //通知进度消息

        return true;
    }

    /// <summary>
    /// 所有数据接收完成的回调，将临时文件保存为制定的文件名
    /// </summary>
    protected override void CompleteContent()
    {
        Debug.Log("下载完成！");
        string CompleteFilePath = SavePath + "/" + FileName;   //完整路径
        string TempFilePath = fs.Name;   //临时文件路径
        OnDispose();

        if (File.Exists(TempFilePath))
        {
            if (File.Exists(CompleteFilePath))
            {
                File.Delete(CompleteFilePath);
            }
            File.Move(TempFilePath, CompleteFilePath);
            //Debug.Log("重命名文件！");
        }
        else
        {
            Debug.Log("生成文件失败=>下载的文件不存在！");
        }
        if (EventComplete != null)
            EventComplete.Invoke(CompleteFilePath);

    }

    public void OnDispose()
    {
        //Debug.Log("下载总量=>" + _downedLength);
        fs.Close();
        fs.Dispose();
    }

    /// <summary>
    /// 请求下载时，会先接收到文件的数据总量
    /// </summary>
    /// <param name="contentLength">如果是从网络上下载资源，则表示文件剩余下载的大小；如果是本地拷贝资源，则表示文件总长度</param>
    protected override void ReceiveContentLength(int contentLength)
    {
        _ContentLength = contentLength + _DownedLength;
        if (EventTotalLength != null)
            EventTotalLength.Invoke(_ContentLength);
        //Debug.Log(string.Format("收到头部信息==>>数据总长度{0}", contentLength));
    }

	#endregion
}
