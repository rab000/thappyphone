using ntools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StagePanel : MonoBehaviour
{
    [SerializeField] Button ExitBtn;

    private void OnEnable()
    {
        ExitBtn.onClick.AddListener(ExitClick);
    }
    private void OnDisable()
    {
        ExitBtn.onClick.RemoveAllListeners();
    }

    public void ExitClick()
    {       
        Messenger.Broadcast<string>(GameEvent.SWITCH_MENU_STATE, "login");
    }

    public void TempEnterStageClick()
    {
        AppMgr.GetIns().SwitchScn("StageScn1");
    }


}
