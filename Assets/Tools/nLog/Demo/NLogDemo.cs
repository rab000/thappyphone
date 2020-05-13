using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NLog;
using NLog.UI;
public class NLogDemo : MonoBehaviour
{
    public UIGMLogContent logUI;

    void Awake()
    {
        //Debug.Log("<color=#aa0000>wooden</color>");
        //Debug.Log(string.Format("<color=#ff00ff>{0}</color>", "hello world"));
        //System.Console.
        logUI.Init();
    }

    int num = 0;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            LogMgr.I("logI","taga");
        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            LogMgr.E("logE", "tagb");
        }

        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            num++;
            Debug.Log("tUnityLogI:"+num);
        }

        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            Debug.LogError("tUnityLogE");
        }

        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            print("test print");
        }

        if (Input.GetKeyUp(KeyCode.Alpha6))
        {
            string ip = NetUtils.GetLocalIPv4();

            Debug.LogError("ip----->"+ip);
        }

        if (Input.GetKeyUp(KeyCode.Alpha7))
        {

            Debug.LogError("测试写入位置:" + LogConfig.LogFileSavePath);

            int a = 1;
            int b = 0;
            int c = a / b;

            //FileHelper.WriteString2File_Append(LogConfig.LogFileSavePath, "baocun!!我的");
        }
    }

}
