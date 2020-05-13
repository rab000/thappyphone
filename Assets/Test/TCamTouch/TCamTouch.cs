using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCamTouch : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A)) 
        {
            Debug.Log("bind camera input event");
            CameraMgr.GetIns().BindInputEvent();
        }

        if (null == PCInputMgr.GetIns()) 
        {
            Debug.Log("pcinput = null");
            return;
        }  

        if (null == TouchMgr.GetIns()) 
        {
            Debug.Log("touch input = null");
            return;
        }

        //Debug.Log("update");

        PCInputMgr.GetIns().tUpdate();

        TouchMgr.GetIns().tUpdate();


    }

    private void LateUpdate()
    {
        if (null == CameraMgr.GetIns()) return;

        CameraMgr.GetIns().tLateUpdate();
    }
}
