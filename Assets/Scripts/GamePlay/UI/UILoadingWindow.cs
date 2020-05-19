using UnityEngine.UI;
using UnityEngine;

public class UILoadingWindow :SingletonBehaviour<UILoadingWindow> {

	public Image BgImg;
	public Slider LoadingSlider;
	public Text TextInfo;
	public Text TextProgress;

    /// <summary>
    /// 是否启动进度平滑
    /// </summary>
    public bool BeProgressSmoth = false;

    private static GameObject SelfGo;

    private void OnEnable()
    {
        
        ntools.Messenger.AddListener<float>("updateLoadingProgress", UpdateProgress);
        ntools.Messenger.AddListener<string>("updateLoadingText", UpdateInfoText);
    }

    private void OnDisable()
    {
        LoadingSlider.value = 0;
        ntools.Messenger.RemoveListener<float>("updateLoadingProgress", UpdateProgress);
        ntools.Messenger.RemoveListener<string>("updateLoadingText", UpdateInfoText);
    }

    public static void Create() 
    {
        SelfGo = GameObject.Instantiate(Resources.Load("Prefabs/UI/LoadingCanvas")) as GameObject;
        SelfGo.name = "LoadingCanvas";

    }

    public static void Close() 
    {       
        Destroy(SelfGo);
        SelfGo = null;
    }


	public void Open(object uiDatas=null)
    {	
        LoadingSlider.value = 0;
	}
    
    private void UpdateProgress(float progress)
    {
        //Debug.LogError("接收 progress:"+progress);

        int intProgress = (int)(progress * 100);

        if (BeProgressSmoth)
        {
            //这里的问题，更新进度的时候要更新文字           
            CurRealProcess = intProgress;
        }
        else
        {
            TextProgress.text = intProgress.ToString();
            LoadingSlider.value = progress;

            if (progress == 1.0f)
            {
                ActionAfterLoading?.Invoke();
            }
        }
    }

	public void SetBgImg(Sprite sprite)
    {
		BgImg.overrideSprite = sprite;
	}

    private void UpdateInfoText(string str)
    {
        //Debug.LogError("更新loading text:" + str);
        TextInfo.text = str;
	}
   
    public  void Clear(){
        
		LoadingSlider.value = 0f;
		TextInfo.text = "载入中...";
		TextProgress.text ="载入中0";
		CurRealProcess = 0f;
		CurProcess = 0f;
        ntools.Messenger.RemoveListener<float>("updateLoadingProgress", UpdateProgress);
        ntools.Messenger.RemoveListener<string>("updateLoadingText", UpdateInfoText);
    }

    #region lecacy

    /// <summary>
	/// 当前真实加载进度,最大不超过100
	/// </summary>
	public float CurRealProcess;

    /// <summary>
    /// 当前实时加载进度,最大不超过100
    /// </summary>
    public float CurProcess;

    private const float LOAD_SPEED_FACTOR = 0.01f;

    private Listener ActionAfterLoading;

    /// <summary>
    /// 设置loading后续回调
    /// </summary>
    /// <param name="callback"></param>
    public void SetActionAfterLoading(Listener callback)
    {
        ActionAfterLoading = callback;
    }

    public void Update()
    {

        if (!BeProgressSmoth) return;

        if (CurProcess >= 100)
        {           
            ActionAfterLoading?.Invoke();
            return;
        }
                
        if (CurProcess < CurRealProcess)
        {
            CurProcess++;
        }
        else if (CurProcess > CurRealProcess)
        {
            CurProcess = CurRealProcess;
        }

        TextProgress.text = CurProcess.ToString();

        float f = CurProcess * LOAD_SPEED_FACTOR;

        LoadingSlider.value = f;

    }
    #endregion lecacy

}
