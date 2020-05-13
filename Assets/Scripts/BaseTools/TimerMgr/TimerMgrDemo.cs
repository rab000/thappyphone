using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerMgrDemo : MonoBehaviour
{
       
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            Debug.LogError("启动计时器");
            TimerMgr.GetIns().StartTimer(1f,Callback, 2, null, "test1");
        }

        if (Input.GetKeyUp(KeyCode.B))
        {
            Debug.LogError("停止计时器");
            TimerMgr.GetIns().StopTimer("test1");
        }
    }


    private void Callback()
    {
        Debug.LogError("---->"+TimeUtils.GetTimeStamp());
    }
}
