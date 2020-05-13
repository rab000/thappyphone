using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NLog.UI;

public class UIDebugCanvas : MonoBehaviour
{
    [SerializeField] Transform DebugPanelTrm;

    [SerializeField] Button OpenBtn;

    [SerializeField] UIGMLogContent UILogContent;

    public static UIDebugCanvas Ins;

    private void Awake()
    {
        Ins = this;
        //NINFO 注意这里必须先初始化(注册ui观察logMgr的消息事件)，否则log显示不到屏幕上
        UILogContent.Init();
    }

    private void OnDestroy()
    {
        Ins = null;
    }
    private void OnEnable()
    {
        OpenBtn.onClick.AddListener(OnOpenBtnClick);
    }

    private void OnDisable()
    {
        OpenBtn.onClick.RemoveAllListeners();
    }

    void OnOpenBtnClick()
    {
        OpenLogPanel();
    }

    public void OpenLogPanel()
    {
        DebugPanelTrm.gameObject.SetActive(true);
        OpenBtn.gameObject.SetActive(false);
    }

    public void CloseLogPanel()
    {
        DebugPanelTrm.gameObject.SetActive(false);
        OpenBtn.gameObject.SetActive(true);
    }
}
