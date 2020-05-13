using System;
using UnityEngine;
/// <summary>
/// Nafio 计时
/// 使用方法说明
/// 1 new Timer
/// 2 在update中 通过SurplusTimePercentage获取剩余时间百分比，通过百分比来处理一些持续更新逻辑
/// 3 在update中 IsOk（）{  Stop(); 处理其他逻辑； } 计时器结束处理完要保证Stop，避免IsOk被二次处理
/// 
/// 以后可以考虑加入暂停功能，暂时没必要
/// 
/// 注意事项Time.realtimeSinceStartup黑屏时也会走
/// 
/// </summary>
public class Timer{

    //倒计时结束回调
	public Listener OnEndOfCountDown;

    //每帧更新时回调，返回进度
	public Listener<float> OnUpdate;

    //计时器名称，方便同时存在多计时器时追踪log
    public string Name = "nil";

    //是否使用真实时间
    //真实时间不受timeScale影响，并且在进入后台后计时也不会停止
    //非真实时间受timeScale影响，并且在进入后台后会停止计时
    public bool BeRealTime = false;

    //剩余重复次数，默认1，-1代表无限循环
    public int suplusRepeatNum = 1;

    //持续时间
    private float durSec;

    //开始时间
    private float startTimeSec;

    //（暂停时）剩余时间
    private float suplusTimeSec = -1f;

    //是否暂停
    private bool isRunning = false;

    //计时器是否生命已经结束，生命结束的计时器要回池
    private bool _beAlive = true;
    private bool beAlive
    {
        set
        {
            _beAlive = value;
            if (!_beAlive) isRunning = false;
        }
        get
        {
            return _beAlive;
        }
    }

    #region public

    public void Start(float durSec)
    {
		this.durSec=durSec;
		startTimeSec= GetCurTime();
		isRunning = true;
        beAlive = true;
	}

    public void ReStart()
    {
        Start(durSec);
    }

    public void Pause()
    {
        isRunning = false;
        //计算剩余时间并记录，等待resume
        suplusTimeSec = GetSurplusTime();
    }

    public void Resume()
    {
        //注意，如果计时器有重复次数，暂停后，Start(surplus)就错了，循环时间不能变
        //所以这里只把isRunning = true;其他交给IsOk和tUpdate来处理
        isRunning = true;
    }

    public void Stop()
    {
        beAlive = false;
    }

    public bool IsRunning()
    {
		return isRunning;
	}

    public bool IsAlive()
    {
        return beAlive;
    }

    public void SetAlive(bool bAlive)
    {
        this.beAlive = bAlive;
    }

    //判断1次计时时间到了
	public bool IsOK()
    {
        if (!isRunning)
        {
            return false;
        }

        //暂停并恢复后的计时判断
        if (suplusTimeSec != -1f)
        {
            if (GetCurTime() - startTimeSec >= suplusTimeSec)
            {
                suplusTimeSec = -1f;
                return true;
            }
            return false;
        }

        //常规计时判断
		if(GetCurTime() - startTimeSec>=durSec)
        {
			return true;
		}
		return false;
	}

    public void Reset()
    {
        Name = "nil";

        OnEndOfCountDown = null;

        OnUpdate = null;

        suplusRepeatNum = 1;

        startTimeSec = -1;

        suplusTimeSec = -1f;

        durSec = 0f;

        isRunning = false;

        beAlive = true;
    }

    public void tUpdate()
    {
        if (!isRunning) return;

        //持续百分比
        float t = GetLastTimePerventage();
        OnUpdate?.Invoke(t);

        //是否到达计时终点
        bool bOk = IsOK();
        if (!bOk)
            return;

        if (suplusRepeatNum == -1)
        {
            OnEndOfCountDown?.Invoke();
            ReStart();
            return;
        }

        if (suplusRepeatNum > 0)
        {
            //未完成重复次数
            suplusRepeatNum--;
            OnEndOfCountDown?.Invoke();
            ReStart();
            return;
        }

        //计时器结束
        beAlive = false;
        //OnEndOfCountDown?.Invoke();

    }

    #endregion

    #region progress
    /// <summary>
    /// 剩余时间
    /// </summary>
    /// <returns>The time.</returns>
    public float GetSurplusTime()
    {
		return startTimeSec + durSec - GetCurTime();
    }

	/// <summary>
	/// 获取剩余时间比例
	/// </summary>
	/// <returns>The time percentage.</returns>
	public float GetSurplusTimePercentage()
    {
		float t = GetSurplusTime();
		if(t <= 0)
			return 0;
		return t / durSec;
	}

	/// <summary>
	/// 持续时间
	/// </summary>
	/// <returns>The time.</returns>
	public float GetLastTime()
	{
		return GetCurTime() - startTimeSec;
	}

    private float GetCurTime()
    {
        if (BeRealTime)
        {
            return Time.realtimeSinceStartup;
        }
        else
        {
            return Time.time;
        }

    }

    /// <summary>
    /// 持续时间占总时间的百分比
    /// </summary>
    /// <returns>The time perventage.</returns>
    public float GetLastTimePerventage()
    {
		float t = GetLastTime();
		if(t <= 0)
			return 0;
		return t / durSec;
	}

    #endregion

}//end of class




