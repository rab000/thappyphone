using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownMgrDemo : MonoBehaviour
{
    
    void Start()
    {
        CountDownMgr.GetIns().AddCallBack(() =>
        {
            Debug.Log("XXX");
        }, 3f);

        var item = GetCountItem();

        CountDownMgr.GetIns().AddItem(item);

        CountDownMgr.GetIns().RemoveItem(item);

    }

    private CountDownMgr.CountItem GetCountItem()
    {
        var item = new CountDownMgr.CountItem();
        item.callback = cb;
        item.startTimeToCall = 10f;
        return item;
    }

    private void cb()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
