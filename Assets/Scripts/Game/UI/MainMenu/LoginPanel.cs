using UnityEngine;
using UnityEngine.UI;
using ntools;

public class LoginPanel : MonoBehaviour
{
    [SerializeField] Button StageBtn;

    [SerializeField] Button GameBtn;

    [SerializeField] Button ExitBtn;

    private void OnEnable()
    {
        StageBtn.onClick.AddListener(StageClick);
        GameBtn.onClick.AddListener(GameClick);
        ExitBtn.onClick.AddListener(ExitClick);
    }

    private void OnDisable()
    {
        StageBtn.onClick.RemoveAllListeners();
        GameBtn.onClick.RemoveAllListeners();
        ExitBtn.onClick.RemoveAllListeners();
    }

    public void StageClick()
    {
        Messenger.Broadcast<string>(GameEvent.SWITCH_MENU_STATE,"stage");       
    }

    public void GameClick() 
    {
        AppMgr.GetIns().SwitchScn("Game");
    }

    public void ExitClick()
    {
        AppMgr.GetIns().ExitApp();
    }
    
}
