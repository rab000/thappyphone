using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NLog;

public class CallbackMgr : AbsCallbackAction {

    /// <summary>
    /// 序列中的动作事件数量
    /// </summary>
    private float ActionNum;

    /// <summary>
    /// 输出时间处理进度
    /// </summary>
    public Listener<float> ProgressEvent;

	public CallbackMgr(Listener callback)
	{
		OnEnd = callback;
	}

	private Queue<BaseCallback> CallbackActionQue = new Queue<BaseCallback>();

	//添加callback
	public void Add(BaseCallback action)
	{
		action.OnEnd = OnOneActionCallback;
		CallbackActionQue.Enqueue(action);
	}

	//开始执行
	public override void Start()
	{
        ActionNum = CallbackActionQue.Count;

        if (ActionNum > 0)
        {
            ProgressEvent?.Invoke(0f);
            BaseCallback cb = CallbackActionQue.Dequeue();
            cb.Start();
        }
        else
        {
            LogMgr.I("CallbackMgr","Start","ActionNum==0 直接执行CallbackMgr的End，检查是否添加过callback",true);
            ProgressEvent?.Invoke(1f);
            End();
        }
	}

	public override void End()
	{
		if (null != OnEnd)
			OnEnd ();
	}

	private void OnOneActionCallback()
	{
		if (CallbackActionQue.Count > 0)
        {
            float progress = 1f - CallbackActionQue.Count/ActionNum;
            ProgressEvent?.Invoke(progress);
            BaseCallback cb = CallbackActionQue.Dequeue ();
			cb.Start ();
		} 
		else 
		{
            ProgressEvent?.Invoke(1f);
            End();
		}
	}

}



