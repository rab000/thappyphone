using UnityEngine;
using UnityEngine.UI;


//xxx缓存问题
//超时问题
//下载目标不存在时的提示，下载失败的temp文件没清理
//目前不支持后台下载
//测试下载
//关于httpNetError这些参数的使用，按理说，如果没有网，或者检查到没文件，就应该停止下载

public class DownloadDemo : MonoBehaviour {

    //网络上的资源支持断点续传
    string url1 = "http://localhost:8090/test.zip";
    string url2 = "http://localhost:8090/test2.zip";
    public Text textUrl1;
    public Text textProgress1;
    

    private void Start()
    {
        //前端显示资源路径
        textUrl1.text = "URL1:\n" + url1;
       
		DownloadMgr.Init ();
    }

	void Update()
	{
       
        if (Input.GetKeyUp (KeyCode.A)) 
		{
			Debug.Log ("启动下载 url:"+url1);
            TestDownload(url1);
		}
		if (Input.GetKeyUp (KeyCode.B)) 
		{
			Debug.Log ("启动下载 url:"+url2);
            TestDownload(url2);

        }
	}

    public void TestDownload(string url)
    {
        string[] URLStr = url1.Split('/');
        string FileName = URLStr[URLStr.Length - 1];

        string _savePath = Application.dataPath + "/../" + FileName;

        DownloadMgr.GetIns().StartDownload(url, _savePath, OnSuccess,OnFaile, Total,Progress1);

    }

    public void StopDownload(string url)
    {
		DownloadMgr.GetIns().StopDownload(url);
    }


    #region 回调

    private void OnSuccess()
    {
        Debug.Log("下载成功！");
    }

    private void OnFaile(string error)
    {
        Debug.Log("下载成功失败 "+error);
    }

    //完成下载
    private void Complete(string path)
    {
        Debug.Log("下载完成，文件路径=>" + path);
    }

    //文件总大小
    private void Total(int length)
    {
        Debug.Log("要下载的文件总大小=>" + length + "字节");
    }

    //下载进度
    private void Progress1(float progress)
    {
        textProgress1.text = "进度:" + (progress * 100).ToString("0.00") + "%";
    }

    

    #endregion

}
