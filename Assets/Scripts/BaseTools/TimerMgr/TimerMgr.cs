using NLog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerMgr : SingletonBehaviour<TimerMgr>
{
    private static bool BeShowLog = false;

    private readonly List<Timer> TimerList = new List<Timer>();

    //只存储有名称得timer(有名称得timer不可以重名),便于查找并停止timer
    private readonly Dictionary<string,Timer> TimerDic = new Dictionary<string, Timer>();

    public static ClassPool<Timer> TimerPool = new ClassPool<Timer>();

    private int TimerNums {
        get
        {
            return TimerList.Count;
        }
    }

    public void StartTimer(float time,Listener onEndOfCountDown,int repeatNum = 1,Listener<float> onUpdate=null,string timerName = "nil")
    {
        Timer timer = TimerPool.Get();        
        timer.OnEndOfCountDown = onEndOfCountDown;
        timer.OnUpdate = onUpdate;
        timer.suplusRepeatNum = repeatNum;
        
        if (!timerName.Equals("nil"))
        {
            timer.Name = timerName;
            TimerDic.Add(timerName,timer);
        }

        TimerList.Add(timer);
        LogMgr.I("TimerMgr", "StartTimer", "启动计时器 time:" + time + " name:" + name +" repeatNum:"+ repeatNum, BeShowLog);

        timer.Start(time);

    }

    public void StopTimer(string name)
    {
        if (!TimerDic.ContainsKey(name))
        {
            LogMgr.E("TimerMgr","StopTimer","停止计时器失败，找不到计时器 name:"+name,BeShowLog);
            return;
        }

        LogMgr.I("TimerMgr", "StopTimer", "主动停止计时器 name:" + name, BeShowLog);
        TimerDic[name].SetAlive(false);

    }

    void Update()
    {      
        //遍历更新全局计时器
        int count = TimerNums;

        Timer tempTimer = null;

        for (int i = 0; i < count; i++)
        {
            tempTimer = TimerList[i];

            tempTimer.tUpdate();

            if (!tempTimer.IsAlive())
            {
                TimerList.RemoveAt(i);

                if (!tempTimer.Name.Equals("nil"))
                {
                    TimerDic.Remove(tempTimer.Name);
                }

                TimerPool.Put(tempTimer);
            }

        }

    }

}
