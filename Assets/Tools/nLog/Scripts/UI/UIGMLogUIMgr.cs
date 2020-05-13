using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 用来管理log相关的ui
/// 分担UIGMLogContent的内容，使UIGMLogContent只包含滚动相关的内容
/// </summary>
namespace NLog.UI
{
    public class UIGMLogUIMgr : MonoBehaviour
    {
        private UIGMLogContent _UIGMLogContent;

        [SerializeField] Button ClearBtn;

        [SerializeField] InputField FilterInputField;

        [SerializeField] Button SettingBtn;

        [SerializeField] Text DetailText;

        [SerializeField] Toggle InfoToggle;

        [SerializeField] Toggle ErrorToggle;

        [SerializeField] Transform SettingPanel;

        [SerializeField] Button CloseBtn;

        public static UIGMLogUIMgr Ins;

        void Awake()
        {
            Ins = this;
        }

        void OnDestroy()
        {
            Ins = null;
        }

        void OnEnable()
        {
            ClearBtn.onClick.AddListener(OnClearClick);

            SettingBtn.onClick.AddListener(OnSettingClick);

            FilterInputField.onValueChanged.AddListener(OnFilterFileChanged);

            InfoToggle.onValueChanged.AddListener(OnInfoToggleValueChange);

            ErrorToggle.onValueChanged.AddListener(OnErrorToggleValueChange);

            CloseBtn.onClick.AddListener(OnCloseClick);
        }

        void OnDisable()
        {
            ClearBtn.onClick.RemoveAllListeners();

            SettingBtn.onClick.RemoveAllListeners();

            FilterInputField.onValueChanged.RemoveAllListeners();

            CloseBtn.onClick.RemoveAllListeners();
        }

        void OnClearClick()
        {
            LogMgr.ClearLog();
        }

        void OnSettingClick()
        {
            SettingPanel.gameObject.SetActive(true);
        }

        void OnFilterFileChanged(string content)
        {
           
            if (null == content)
            {
                LogMgr.RegexFilter(LogDefault.REGEX_DEFAULT);
            }
            else
            {
                LogMgr.RegexFilter(content);
            }
            
        }

        public void SetDetailLog(string s)
        {
            DetailText.text = s;
        }

        void OnInfoToggleValueChange(bool b)
        {
            if (b)
            {
                switch (LogConfig.Level)
                {
                    case LogDefault.LEVEL_ALL:
                    case LogDefault.LEVEL_INFO:
                        break;
                    case LogDefault.LEVEL_ERROR:
                        LogConfig.Level = LogDefault.LEVEL_ALL;
                        break;
                    case LogDefault.LEVEL_NOTHING:
                        LogConfig.Level = LogDefault.LEVEL_INFO;
                        break;
                }

                LogMgr.Ins.RefreshCurLogList();

                return;

            }

            switch (LogConfig.Level)
            {
                case LogDefault.LEVEL_ALL:
                    LogConfig.Level = LogDefault.LEVEL_ERROR;
                    break;
                case LogDefault.LEVEL_INFO:
                    LogConfig.Level = LogDefault.LEVEL_NOTHING;
                    break;
                case LogDefault.LEVEL_ERROR:                    
                case LogDefault.LEVEL_NOTHING:
                    break;
            }
            
            LogMgr.Ins.RefreshCurLogList();
        }

        void OnErrorToggleValueChange(bool b)
        {
            if (b)
            {
                switch (LogConfig.Level)
                {
                    case LogDefault.LEVEL_ALL:
                    case LogDefault.LEVEL_ERROR:                   
                        break;
                    case LogDefault.LEVEL_INFO:
                        LogConfig.Level = LogDefault.LEVEL_ALL;
                        break;
                    case LogDefault.LEVEL_NOTHING:
                        LogConfig.Level = LogDefault.LEVEL_ERROR;
                        break;
                }

                LogMgr.Ins.RefreshCurLogList();

                return;
            }

            switch (LogConfig.Level)
            {
                case LogDefault.LEVEL_ALL:
                    LogConfig.Level = LogDefault.LEVEL_INFO;
                    break;
                case LogDefault.LEVEL_ERROR:
                    LogConfig.Level = LogDefault.LEVEL_NOTHING;
                    break;                
                case LogDefault.LEVEL_INFO:
                case LogDefault.LEVEL_NOTHING:
                    break;
            }

            LogMgr.Ins.RefreshCurLogList();
        }

        void OnCloseClick()
        {
            UIDebugCanvas.Ins.CloseLogPanel();
        }

    }
}
