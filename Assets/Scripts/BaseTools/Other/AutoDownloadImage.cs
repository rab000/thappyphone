using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Text;

/// <summary>
/// 网络图片加载
/// by nafio 2018.12.19
/// </summary>
[RequireComponent(typeof(Image))]
public class AutoDownloadImage : MonoBehaviour
{

    private bool BeShowLog = false;

    /// <summary>
    /// 远程图片地址
    /// </summary>
    public string ImgUrl;

    /// <summary>
    /// 图片扩展名
    /// </summary>
    private string TempExtension;

    /// <summary>
    /// 默认显示的图片
    /// </summary>
    public Sprite Placeholder;

    /// <summary>
    /// 图片本图
    /// </summary>
    private Image SelfImg;

    private StringBuilder SB = new StringBuilder();


    /// <summary>
    /// 缓存地址
    /// </summary>
    /// <value>The cache path.</value>
    private static string CachePath
    {
        get
        {

            string cpath = null;

#if UNITY_ANDROID
			cpath = Application.persistentDataPath+ "/HeadIconCache/";
#elif UNITY_IOS
            cpath = Application.persistentDataPath + "/HeadIconCache/";
#else
            cpath = Application.persistentDataPath + "/HeadIconCache/";
#endif

            return cpath;
        }
    }


    public static bool Init()
    {
		//if(BeShowLog)Debug.Log("AutoDownload.Init CachePath:" + CachePath);

        if (!Directory.Exists(CachePath))
        {
            Directory.CreateDirectory(CachePath);
        }

        return true;

    }

    void Awake()
    {
        SelfImg = gameObject.GetComponent<Image>();
    }

    void Start()
    {

        if (null == Placeholder)
        {
            Placeholder = Resources.Load("Ext/defaultHeadIcon") as Sprite;
        }

        if (null != ImgUrl && !ImgUrl.Equals(""))
        {
			if(BeShowLog)Debug.LogError("AutoDownloadImage.Start ImgUrl:" + ImgUrl);
            SetImgUrl(ImgUrl, false);
        }

    }

    void Update()
    {
        //		if (Input.GetKeyUp (KeyCode.Alpha2)) {
        //
        //			Init ();
        //			string s = @"http://pic.wenwen.soso.com/p/20111223/20111223004721-219301942.jpg";
        //			SetImgUrl(s);
        //		}
    }

    /// <summary>
    /// 设置图片地址
    /// beEmail = true时，url应该是邮箱名首字母
    /// </summary>
    /// <param name="url">URL.</param>
    public void SetImgUrl(string url, bool beEmail)
    {

        if (beEmail)
        {
            ProcessEmailHead(url);
            return;
        }


        SelfImg.color = Color.white;

        if (null == url || url.Equals(""))
        {
			if (BeShowLog)Debug.LogError("AutoDownloadImage.SetImgUrl url无效！！！ 设置失败 url:" + url);
            return;
        }

        TempExtension = Path.GetExtension(url); ;

        if (TempExtension.Equals(""))
        {
            TempExtension = ".png";
        }

        //		if (TempExtension.Equals (".png") || TempExtension.Equals (".jpg"))
        //		{
        //
        //		}
        //		else
        //		{
        //			Debug.LogError ("AutoDownloadImage.SetImgUrl url无效  不是有效图片文件");
        //			return;
        //		}

        ImgUrl = url;

        SetAsyncImage(url, SelfImg);
    }

    public void SetImgUrl2(string url, bool beEmail, bool b = false)
    {

        if (beEmail)
        {
            ProcessEmailHead(url);
            return;
        }


        SelfImg.color = Color.white;

        if (null == url || url.Equals(""))
        {
            Debug.LogError("AutoDownloadImage.SetImgUrl url无效！！！ 设置失败 url:" + url);
            return;
        }

        TempExtension = Path.GetExtension(url); ;

        ImgUrl = url;

        SelfImg.sprite = Placeholder;

        SB.Remove(0, SB.Length);

        SB.Append(CachePath);

        SB.Append(url.GetHashCode());

        SB.Append(TempExtension);

        string localPath = SB.ToString();

        //		if (BeShowLog)
        //			Debug.Log ("SetAsyncImage localPath:" + localPath);

        //		if (!File.Exists (localPath))
        //		{

        StartCoroutine(DownloadImage(url, SelfImg));
        //		}
        //		else
        //		{
        //			StartCoroutine (LoadLocalImage (url, SelfImg));
        //		}
    }

    private void ProcessEmailHead(string url)
    {

        SelfImg.sprite = Resources.Load<Sprite>("Sprites/email_headIcon/email_bg");

        //NAFIO TODO 这里有在线信息时再设置
        //Debug.LogError ("---------->修改bg为red");
        //SelfImg.color = Color.red;

        //Debug.LogError ("---------->"+SelfImg.gameObject.name);

        GameObject textGo = GameObject.Instantiate(Resources.Load("Ext/Game/Text") as GameObject);

        var text = textGo.GetComponent<Text>();

        textGo.transform.SetParent(SelfImg.transform, false);

        text.rectTransform.sizeDelta = SelfImg.rectTransform.sizeDelta;


        text.alignByGeometry = true;

        text.fontSize = 50;

        string s = null;

        if (url == null || url.Equals(""))
        {
            return;
        }

        if (url.Length > 0)
        {
            s = url[0].ToString();
        }

        text.text = s;

    }

    private void SetAsyncImage(string url, Image image)
    {
        image.sprite = Placeholder;

        SB.Remove(0, SB.Length);

        SB.Append(CachePath);

        SB.Append(url.GetHashCode());

        SB.Append(TempExtension);

        string localPath = SB.ToString();

        if (BeShowLog)
            Debug.Log("SetAsyncImage localPath:" + localPath + " url:" + url + " CachePath:" + CachePath + " hash:" + url.GetHashCode() + " te:" + TempExtension);

        if (!File.Exists(localPath))
        {

            StartCoroutine(DownloadImage(url, image));
        }
        else
        {
            StartCoroutine(LoadLocalImage(url, image));
        }

    }

    IEnumerator DownloadImage(string url, Image image)
    {
        if (BeShowLog) Debug.Log("AutoDownloadImage.DownloadImage 本地没有文件，从网络下载 url:" + url + " hash:" + url.GetHashCode());

        WWW www = new WWW(url);

        yield return www;

        bool bSuccess = true;

        if (www.error != null)
        {
            bSuccess = false;
			if (BeShowLog)Debug.LogError("AutoDownload.DownloadImage  error:" + www.error + " url:" + url);
            //yield break;
        }

        Texture2D tex2d = www.texture;

        if (null == tex2d)
        {
            bSuccess = false;
			if (BeShowLog) Debug.LogError("AutoDownload.DownloadImage  texture = null url:" + tex2d);

        }

        if (bSuccess)
        {
            byte[] pngData = tex2d.EncodeToPNG();

            SB.Remove(0, SB.Length);

            SB.Append(CachePath);

            SB.Append(url.GetHashCode());

            SB.Append(TempExtension);

            string path4Cache = SB.ToString();

            if (BeShowLog)
                Debug.Log("AutoDownload.DownloadImage img save---> path4Cache:" + path4Cache + " url:" + url + " CachePath:" + CachePath + " hash:" + url.GetHashCode() + " te:" + TempExtension);

            Sprite m_sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), new Vector2(0, 0));

            image.sprite = m_sprite;

            Init();

            File.WriteAllBytes(path4Cache, pngData);
        }

    }


    IEnumerator LoadLocalImage(string url, Image image)
    {



#if UNITY_ANDROID
		string filePath = CachePath + url.GetHashCode () +TempExtension;
#else
        string filePath = "file:///" + CachePath + url.GetHashCode() + TempExtension;
#endif

        if (BeShowLog) Debug.Log("AutoDownloadImage.DownloadImage 本地已有文件，从本地加载 path:" + filePath + " hash:" + url.GetHashCode());

        bool bSuccess = true;

        WWW www = new WWW(filePath);

        yield return www;

        if (www.error != null)
        {
            bSuccess = false;
			if (BeShowLog)Debug.LogError("AutoDownload.LoadLocalImage  error:" + www.error + " filePath:" + filePath);
            //yield break;
        }

        Texture2D texture = www.texture;

        if (null == texture)
        {
            bSuccess = false;
			if (BeShowLog)Debug.LogError("AutoDownload.LoadLocalImage  texture = null filePath:" + filePath);
            //yield break;
        }


        if (bSuccess)
        {
            Sprite m_sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
            image.sprite = m_sprite;
        }


    }



}
