using UnityEngine;
using UnityEngine.UI;
using ntools;

public class LoginPanel : MonoBehaviour
{
    [SerializeField] Button EnterBtn;

    [SerializeField] Button ExitBtn;

    private void OnEnable()
    {
        EnterBtn.onClick.AddListener(EnterClick);
        ExitBtn.onClick.AddListener(ExitClick);
    }

    private void OnDisable()
    {
        EnterBtn.onClick.RemoveAllListeners();
        ExitBtn.onClick.RemoveAllListeners();
    }

    public void EnterClick()
    {
        Messenger.Broadcast<string>(GameEvent.SWITCH_MENU_STATE,"stage");       
    }

    public void ExitClick()
    {
        AppMgr.GetIns().ExitApp();
    }
    
}
