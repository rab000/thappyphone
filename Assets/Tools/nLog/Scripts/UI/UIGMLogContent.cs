using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NLog.UI
{
    public class UIGMLogContent : UILoopScrollerContent<LogStruct, UIGMLogItem>
    {

        public override void Init()
        {           
            LogMgr.Ins.UpdateLogsEvent = RefreshLogUI;            
            m_Datas.Clear();
            ReloadData(0);            
        }

        public override float GetCellViewSize(LoopScroller scroller, int dataIndex)
        {
            string content = LogMgr.GetStringLog(m_Datas[dataIndex]);
            return CalculateTextPreferredHeight(m_ItemTemplate.logLabel, content);
        }

        public static float CalculateTextPreferredHeight(Text label, string content)
        {
            TextGenerationSettings settings = label.GetGenerationSettings(new Vector2(label.GetPixelAdjustedRect().size.x, 0f));
            label.cachedTextGenerator.Populate(content, settings);
            return label.cachedTextGenerator.GetPreferredHeight(content, settings) / label.pixelsPerUnit;
        }

        private void RefreshLogUI(List<LogStruct> list)
        {
            if (null == list) list = new List<LogStruct>();

            SetDatas(list);
        }
      
    }
}