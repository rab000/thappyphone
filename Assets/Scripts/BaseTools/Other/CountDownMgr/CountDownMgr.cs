
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;


/// <summary>
/// 倒计时组件
/// </summary>
/// <remarks>
/// 对象上绑定此组件可增加定时回调功能
/// </remarks>
public class CountDownMgr : SingletonBehaviour<CountDownMgr>
{
    public class CountItem
    {
        public Action callback = null;
        public Action<object> callback1 = null;
        public object param = null;
        internal float curCountTime = 0f;
        internal float realTimeToCall = 0f;
        public float startTimeToCall = 0f;
        public float endimeToCall = -1f;
        internal int curRepeatCount = 0;
        public int totalRepeatCount = -1;

        public int GetLastTime()
        {
            int lastTime = (int)(startTimeToCall - curCountTime);
            if (lastTime < 0)
                lastTime = 0;
            return lastTime;
        }
    }

    private readonly List<CountItem> _eventList = new List<CountItem>();

    public void ClearData()
    {
        _eventList.Clear();
    }

    /// <summary>
    /// 添加回调事件多长时间之后执行回调
    /// </summary>
    /// <param name="startTime">多长时间</param>
    /// <param name="endTime">多长时间</param>
    /// <param name="callback">执行回调</param>
    /// <param name="repeatCount">重复次数  -1:永久重复</param>
    public void AddCallBack(Action callback, float startTime, int repeatCount = 1, float endTime = -1)
    {
        CountItem it = new CountItem();
        it.callback = callback;
        it.startTimeToCall = startTime;
        it.endimeToCall = endTime;
        if (it.endimeToCall > it.startTimeToCall)
        {
            Random random = new Random();
            it.realTimeToCall = Random.Range(it.startTimeToCall, it.endimeToCall);
        }
        else
        {
            it.realTimeToCall = it.startTimeToCall;
        }
        it.totalRepeatCount = repeatCount;
        _eventList.Add(it);
    }
    public void AddCallBack(Action<object> callback, object param, float startTime, int repeatCount = 1, float endTime = -1)
    {
        CountItem it = new CountItem();
        it.callback1 = callback;
        it.param = param;
        it.startTimeToCall = startTime;
        it.endimeToCall = endTime;
        if (it.endimeToCall > it.startTimeToCall)
        {
            Random random = new Random();
            it.realTimeToCall = Random.Range(it.startTimeToCall, it.endimeToCall);
        }
        else
        {
            it.realTimeToCall = it.startTimeToCall;
        }
        it.totalRepeatCount = repeatCount;
        _eventList.Add(it);
    }
    public void AddItem(CountItem it)
    {
        if (it.endimeToCall > it.startTimeToCall)
        {
            Random random = new Random();
            it.realTimeToCall = Random.Range(it.startTimeToCall, it.endimeToCall);
        }
        else
        {
            it.realTimeToCall = it.startTimeToCall;
        }
        _eventList.Add(it);
    }
    public void RemoveItem(CountItem it)
    {
        if (it == null)
            return;
        if (_eventList.Contains(it))
        {
            _eventList.Remove(it);
        }
    }
    /// <inheritdoc>
    /// </inheritdoc>
    public void Update()
    {
        int count = _eventList.Count;
        for (int i = 0; i < count; i++)
        {
            if (_eventList[i] == null)
            {
                _eventList.RemoveAt(i);
                break;
            }
            if (_eventList[i].curCountTime >= _eventList[i].startTimeToCall)
            {
                if (_eventList[i] == null)
                {
                    _eventList.RemoveAt(i);
                    break;
                }
                try
                {
                    _eventList[i].callback?.Invoke();
                    _eventList[i].callback1?.Invoke(_eventList[i].param);
                    _eventList[i].curRepeatCount++;
                    if (_eventList[i].curRepeatCount == _eventList[i].totalRepeatCount)
                    {
                        _eventList.Remove(_eventList[i]);
                        break;
                    }
                    else
                    {
                        if (_eventList[i].endimeToCall > _eventList[i].startTimeToCall)
                        {
                            System.Random random = new System.Random();
                            _eventList[i].realTimeToCall = random.Next((int)_eventList[i].startTimeToCall, (int)_eventList[i].endimeToCall);
                        }
                        else
                        {
                            _eventList[i].realTimeToCall = _eventList[i].startTimeToCall;
                        }
                        _eventList[i].curCountTime = 0;
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e.Message);
                    _eventList.RemoveAt(i);
                    break;
                }
            }
            else
            {
                _eventList[i].curCountTime += Time.unscaledDeltaTime;
            }
        }
    }

    void OnDisable()
    {
        int count = _eventList.Count;
        for (int i = 0; i < count; i++)
        {
            _eventList[i] = null;
        }
        _eventList.Clear();
    }

    public void OnDestroy()
    {
        int count = _eventList.Count;
        for (int i = 0; i < count; i++)
        {
            _eventList[i] = null;
        }
        _eventList.Clear();
    }
}
