using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TAppPause : MonoBehaviour
{

    bool BeShowLog = false;

    bool BeFocus = false;

    bool BePaused = false;

    [HideInInspector] public bool BeSleep = false;

    public static TAppPause Ins;

    void Awake()
    {
        Ins = this;
    }

    /// <summary>
    /// 唤醒事件
    /// </summary>
    public event System.Action ResumeEvent;

    public event System.Action SleepEvent;

    void OnApplicationFocus(bool hasFocus)
    {
        BeFocus = hasFocus;
        if (BeShowLog) Debug.Log("[nafio-net] focus---->" + hasFocus);

        SetPauseState(BeFocus, BePaused);
    }

    void OnApplicationPause(bool pauseStatus)
    {
        BePaused = pauseStatus;
        if (BeShowLog) Debug.Log("[nafio-net] pause---->" + pauseStatus);

        SetPauseState(BeFocus, BePaused);
    }

    private void SetPauseState(bool focus, bool pause)
    {
        if (focus && !pause)
        {
            //唤醒
            BeSleep = false;
            if (BeShowLog) Debug.Log("[nafio-net]--->唤醒");


            ResumeEvent?.Invoke();
        }

        if (!focus && pause)
        {
            if (BeShowLog) Debug.Log("[nafio-net]--->睡眠");

            //睡
            BeSleep = true;
            SleepEvent?.Invoke();

        }

    }


}
