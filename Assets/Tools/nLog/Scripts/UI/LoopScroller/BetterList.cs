using UnityEngine;
using System.Collections.Generic;

namespace NLog.UI
{
    public class BetterList<T>
    {
        private T[] m_Data;
        private int m_Count;

        public T[] Data
        {
            get { return m_Data; }
        }

        public int Count
        {
            get { return m_Count; }
        }

        public T this[int i]
        {
            get
            {
                return m_Data[i];
            }
            set
            {
                m_Data[i] = value;
            }
        }

        public T First
        {
            get
            {
                if (m_Data == null || m_Count == 0)
                {
                    return default(T);
                }
                return m_Data[0];
            }
        }

        public T Last
        {
            get
            {
                if (m_Data == null || m_Count == 0)
                {
                    return default(T);
                }
                return m_Data[m_Count - 1];
            }
        }

        public bool Contains(T item)
        {
            if (m_Data == null || m_Count == 0)
            {
                return false;
            }

            for (var i = 0; i < m_Count; i++)
            {
                if (m_Data[i].Equals(item))
                {
                    return true;
                }
            }

            return false;
        }

        public void Add(T item)
        {
            if (m_Data == null || m_Count == m_Data.Length)
            {
                ResizeArray();
            }

            m_Data[m_Count] = item;
            m_Count++;
        }

        public void AddRange(List<T> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                Add(items[i]);
            }
        }

        public void AddRange(T[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                Add(items[i]);
            }
        }

        public void AddStart(T item)
        {
            Insert(item, 0);
        }

        public void Insert(T item, int index)
        {
            if (m_Data == null || m_Count == m_Data.Length)
            {
                ResizeArray();
            }

            for (var i = m_Count; i > index; i--)
            {
                m_Data[i] = m_Data[i - 1];
            }

            m_Data[index] = item;
            m_Count++;
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < m_Count; i++)
            {
                if (m_Data[i].Equals(item))
                {
                    return i;
                }
            }
            return -1;
        }

        public T RemoveStart()
        {
            return RemoveAt(0);
        }

        public T RemoveEnd()
        {
            if (m_Data == null || m_Count == 0)
            {
                return default(T);
            }

            m_Count--;
            T val = m_Data[m_Count];
            m_Data[m_Count] = default(T);
            return val;
        }

        public T RemoveAt(int index)
        {
            if (m_Data == null || m_Count == 0)
            {
                return default(T);
            }

            T val = m_Data[index];
            for (var i = index; i < m_Count - 1; i++)
            {
                m_Data[i] = m_Data[i + 1];
            }

            m_Count--;
            m_Data[m_Count] = default(T);
            return val;
        }

        public T Remove(T item)
        {
            if (m_Data == null || m_Count == 0)
            {
                return default(T);
            }

            for (var i = 0; i < m_Count; i++)
            {
                if (m_Data[i].Equals(item))
                {
                    return RemoveAt(i);
                }
            }

            return default(T);
        }

        public void Clear()
        {
            m_Count = 0;
        }

        private void ResizeArray()
        {
            T[] newData;
            if (m_Data != null)
            {
                newData = new T[Mathf.Max(m_Data.Length << 1, 64)];
            }
            else
            {
                newData = new T[64];
            }

            if (m_Data != null && m_Count > 0)
            {
                m_Data.CopyTo(newData, 0);

            }
            m_Data = newData;
        }
    }
}