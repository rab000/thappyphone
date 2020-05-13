using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace NLog.UI
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(LayoutElement))]
    public abstract class UILoopScrollerCellViewBase<T> : LoopScrollerCellView
    {
        protected T m_Data;
        public T ItemData
        {
            get
            {
                return m_Data;
            }
        }

        protected Button m_ItemBtn;
        public Button ItemBtn
        {
            get
            {
                if (m_ItemBtn == null)
                {
                    m_ItemBtn = GetComponent<Button>();
                }
                return m_ItemBtn;
            }
        }

        protected LayoutElement m_LayoutElement;
        public LayoutElement Layout
        {
            get
            {
                if (m_LayoutElement == null)
                {
                    m_LayoutElement = GetComponent<LayoutElement>();
                }
                return m_LayoutElement;
            }
        }

        public float PreferredWidth
        {
            get { return Layout.preferredWidth; }
        }

        public float PreferredHeight
        {
            get { return Layout.preferredHeight; }
        }


        protected virtual void Awake()
        {
            m_ItemBtn = GetComponent<Button>();
            m_ItemBtn.onClick.AddListener(OnItemButtonClick);
        }

        public void SetData(T data)
        {
            m_Data = data;
            SetupUIByData(data);
        }

        public override void RefreshCellView()
        {
            base.RefreshCellView();
            SetupUIByData(m_Data);
        }

        protected abstract void SetupUIByData(T data);

        protected abstract void OnItemButtonClick();
    }

    public abstract class UILoopScrollerContent<TData, TCellView> : MonoBehaviour, ILoopScrollerDelegate 
        where TCellView : UILoopScrollerCellViewBase<TData>
    {
        [SerializeField]
        protected LoopScroller m_Scroller;
        [SerializeField]
        protected TCellView m_ItemTemplate;
        protected BetterList<TData> m_Datas = new BetterList<TData>();

        protected void Awake()
        {
            m_Scroller.Delegate = this;
        }

        public virtual void Init()
        {

        }

        public virtual void Init(List<TData> datas)
        {
            m_Datas.Clear();
            m_Datas.AddRange(datas);
            ReloadData();
        }

        public virtual void Init(TData[] datas)
        {
            m_Datas.Clear();
            m_Datas.AddRange(datas);
            ReloadData();
        }


        public void SetDatas(List<TData> datas)
        {
            m_Datas.Clear();
            m_Datas.AddRange(datas);
            ReloadData(0);
        }

        public void SetDatas(TData[] datas)
        {
            m_Datas.Clear();
            m_Datas.AddRange(datas);
            ReloadData(0);
        }

        public void AddData(TData data)
        {
            m_Datas.Add(data);
        }

        public void AddRange(TData[] datas)
        {
            m_Datas.AddRange(datas);
        }

        public void AddRange(List<TData> datas)
        {
            m_Datas.AddRange(datas);
        }

        public void ReloadData(float scrollPositionFactor = 0)
        {
            m_Scroller.ReloadData(scrollPositionFactor);
        }

        public void RefreshActiveCellViews()
        {
            m_Scroller.RefreshActiveCellViews();
        }

        public virtual LoopScrollerCellView GetCellView(LoopScroller scroller, int dataIndex, int cellIndex)
        {
            TCellView cellView = scroller.GetCellView(m_ItemTemplate) as TCellView;
            cellView.dataIndex = dataIndex;
            cellView.name = "Cell " + dataIndex.ToString();
            cellView.SetData(m_Datas[dataIndex]);
            if (!cellView.gameObject.activeSelf)
            {
                cellView.gameObject.SetActive(true);
            }
            return cellView;
        }

        public virtual float GetCellViewSize(LoopScroller scroller, int dataIndex)
        {
            return m_ItemTemplate.PreferredWidth;
        }

        public int GetNumberOfCells(LoopScroller scroller)
        {
            if (m_Datas == null)
            {
                return 0;
            }
            return m_Datas.Count;
        }
    }
}

