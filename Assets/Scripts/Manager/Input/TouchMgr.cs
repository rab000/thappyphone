using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;
using NLog;
/// <summary>
/// 使用leanTouch
/// 全局处理触控拖动，旋转，缩放
/// </summary>
public class TouchMgr : SingletonBehaviour<TouchMgr>
{
    private bool BeShowLog = true;

    // 拖动事件
    public event Listener<Vector2> TouchMoveEvent;

    // 缩放事件
    public event Listener<float> TouchScaleEvent;

    // 忽略从ui上开始点击的touch
    [HideInInspector] public bool IgnoreStartedOverGui = true;

    // 忽略在ui上操作的touch
    [HideInInspector] public bool IgnoreIsOverGui = true;

    // LeanTouch需要传入的参数，暂时用不到
    [HideInInspector] public int RequiredFingerCount;

    // 缩放率
    [Range(-1.0f, 1.0f)]
    public float WheelSensitivity;

    //临时变量
    private Vector2 ScreenDelta = new Vector2();
   
    public void tUpdate()
    {
        
        if (UnityUtil.CurPlatform == UnityUtil.PlatformEnum.android || UnityUtil.CurPlatform == UnityUtil.PlatformEnum.ios)
        {
            //在pc上，使用leanTouch模拟手势触控，是监控不到Input.touchCount的，所以不做这个判断
            //移动平台加这个判断，避免性能浪费
            //没触碰就掠过
            if (Input.touchCount <= 0) return;
        }

        //没注册事件也掠过，尽量避免浪费cpu
        if (null == TouchMoveEvent && null == TouchScaleEvent) return;

        //发出拖动事件
        Vector2 delta = GetScreenDelta();
        TouchMoveEvent.Invoke(delta);

        if (UnityUtil.CurPlatform == UnityUtil.PlatformEnum.android || UnityUtil.CurPlatform == UnityUtil.PlatformEnum.ios)
        {
            //touch小于2不计算缩放，要求双指缩放
            if (Input.touchCount < 2) return;
        }

        //发出缩放事件
        float ratio = GetPinchRatio();
        TouchScaleEvent.Invoke(ratio);

    }

    /// <summary>
    /// 获取拖动或旋转值
    /// </summary>
    /// <returns></returns>
    public Vector2 GetScreenDelta()
    {
        List<LeanFinger> fingers = LeanTouch.GetFingers(IgnoreStartedOverGui, IgnoreIsOverGui, RequiredFingerCount);
        Vector2 lastScreenPoint = LeanGesture.GetLastScreenCenter(fingers);
        Vector2 screenPoint = LeanGesture.GetScreenCenter(fingers);
        ScreenDelta = screenPoint - lastScreenPoint;
        LogMgr.I("TouchMgr", "GetScreenDelta", "拖动 ScreenDelta:"+ ScreenDelta,BeShowLog);
        return ScreenDelta;
    }

    /// <summary>
    /// 获取缩放值
    /// 触屏中取代鼠标中轮
    /// </summary>
    /// <returns></returns>
    public float GetPinchRatio()
    {
        List<LeanFinger> fingers = LeanTouch.GetFingers(IgnoreStartedOverGui, IgnoreIsOverGui, RequiredFingerCount);
        float pinchRatio = LeanGesture.GetPinchRatio(fingers, WheelSensitivity);
        LogMgr.I("TouchMgr", "GetPinchRatio", "缩放 pinchRatio:" + pinchRatio, BeShowLog);
        return (1f-pinchRatio);
    }

    private void PressDown(LeanFinger finger)
    {
        //if (IgnoreStartedOverGui == true && finger.IsOverGui == true) { return; }
        //if (_OperatableMonoObj != null && _OperatableMonoObj.IsSelected == false) { return; }
        //Ray ray = MainCamera.ScreenPointToRay(finger.ScreenPosition);
        //RaycastHit hit = default(RaycastHit);
        ////LayerMask lm = ~(1 << GameEnum.Layer8_Ground);
        //if (Physics.Raycast(ray, out hit, 1000/*,lm*/))
        //{
        //    OperatableMonoObj obj = hit.transform.GetComponent<OperatableMonoObj>();
        //    if (null != obj)
        //    {
        //        obj.OnPressDown(hit.point);
        //    }

        //}
    }

    private void PressUp(LeanFinger finger)
    {
        //if (IgnoreStartedOverGui == true && finger.IsOverGui == true) { return; }
        //if (_OperatableMonoObj != null && _OperatableMonoObj.IsSelected == false) { return; }
        //Ray ray = MainCamera.ScreenPointToRay(finger.ScreenPosition);
        ////LayerMask lm = ~(1 << GameEnum.Layer8_Ground);
        //RaycastHit hit = default(RaycastHit);
        //if (Physics.Raycast(ray, out hit, 1000/*,lm*/) == true)
        //{
        //    OperatableMonoObj obj = hit.transform.GetComponent<OperatableMonoObj>();
        //    if (null != obj)
        //    {
        //        obj.OnPressUp(hit.point);
        //    }

        //}
    }


}
