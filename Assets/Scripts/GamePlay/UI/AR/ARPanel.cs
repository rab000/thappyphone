using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ARPanel : MonoBehaviour
{
    [SerializeField] Button ExitBtn;


    private void OnEnable()
    {
        ExitBtn.onClick.AddListener(OnClickExit);
    }

    private void OnDisable()
    {
        ExitBtn.onClick.RemoveListener(OnClickExit);
    }

    private void OnClickExit() 
    {
        AppMgr.GetIns().SwitchScn("MainMenu");
    }
   

}
