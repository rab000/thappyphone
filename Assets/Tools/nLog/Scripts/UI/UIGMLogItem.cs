using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NLog.UI
{
    public class UIGMLogItem : UILoopScrollerCellViewBase<LogStruct>
    {
        public Text logLabel;

        private string stackTrace;

        protected override void OnItemButtonClick()
        {
            //显示堆栈信息
            UIGMLogUIMgr.Ins.SetDetailLog(stackTrace);
        }

        protected override void SetupUIByData(LogStruct data)
        {
            logLabel.text = LogMgr.GetStringLog(data);
            //logLabel.text = data.Msg;
            this.stackTrace = data.StackTrace;
        }
    }
}

