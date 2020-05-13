using UnityEngine;
using System;
using System.Collections;

namespace NLog.UI
{
    public class LoopScrollerCellView : MonoBehaviour
    {
        public string cellIdentifier;

        [NonSerialized]
        public int cellIndex;

        [NonSerialized]
        public int dataIndex;

        [NonSerialized]
        public bool active;

        public virtual void RefreshCellView() { }
    }
}