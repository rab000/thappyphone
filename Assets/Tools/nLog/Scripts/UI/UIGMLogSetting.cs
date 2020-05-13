using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
public class UIGMLogSetting : MonoBehaviour
{
    [SerializeField] InputField TagInputField;

    [SerializeField] InputField IpInputField1;

    [SerializeField] InputField IpInputField2;

    [SerializeField] InputField IpInputField3;

    [SerializeField] InputField IpInputField4;

    [SerializeField] InputField PortInputField;

    [SerializeField] Button OKBtn;

    [SerializeField] Button CloseBtn;

    private void OnEnable()
    {
        OKBtn.onClick.AddListener(OnOkClick);
        CloseBtn.onClick.AddListener(OnCloseClick);
        TagInputField.text = PlayerPrefs.GetString("nlogTag", NLog.LogDefault.TAG_NAME_ALL);
        IpInputField1.text = PlayerPrefs.GetString("nlogIP1", "192");
        IpInputField2.text = PlayerPrefs.GetString("nlogIP2", "168");
        IpInputField3.text = PlayerPrefs.GetString("nlogIP3", "0");
        IpInputField4.text = PlayerPrefs.GetString("nlogIP4", "1");
        PortInputField.text = PlayerPrefs.GetString("nlogPort", "5432");
    }

    private void OnDisable()
    {
        OKBtn.onClick.RemoveAllListeners();
        CloseBtn.onClick.RemoveAllListeners();
    }

    void OnOkClick()
    {
        StringBuilder sb = new StringBuilder();
        string tag = TagInputField.text;
        if (null == tag || tag.Trim().Equals(""))
        {
            tag = NLog.LogDefault.TAG_NAME_ALL;
        }
        PlayerPrefs.SetString("nlogTag",tag);

        PlayerPrefs.SetString("nlogIP1", IpInputField1.text);
        PlayerPrefs.SetString("nlogIP2", IpInputField2.text);
        PlayerPrefs.SetString("nlogIP3", IpInputField3.text);
        PlayerPrefs.SetString("nlogIP4", IpInputField4.text);
        PlayerPrefs.SetString("nlogPort", PortInputField.text);

        sb.Append(IpInputField1.text);
        sb.Append(".");
        sb.Append(IpInputField2.text);
        sb.Append(".");
        sb.Append(IpInputField3.text);
        sb.Append(".");
        sb.Append(IpInputField4.text);

        sb.Append(":");
        sb.Append(PortInputField.text);


        string url = sb.ToString();

        //这里的几处调用可以再斟酌下
        NLog.Server.LogHttpClient.Ins.URL = url;
        NLog.LogConfig.Tag = tag;
        NLog.LogMgr.Ins.RefreshCurLogList();

        gameObject.SetActive(false);


    }

    void OnCloseClick()
    {
        gameObject.SetActive(false);
    }
}
