using UnityEngine;
using System.Collections;

namespace NLog.UI
{
    public interface ILoopScrollerDelegate
    {
        int GetNumberOfCells(LoopScroller scroller);

        float GetCellViewSize(LoopScroller scroller, int dataIndex);

        LoopScrollerCellView GetCellView(LoopScroller scroller, int dataIndex, int cellIndex);
    }
}