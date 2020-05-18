
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using NLog;
using ntools;
public class MainMenu : MonoBehaviour
{
    
    public Transform LoginPanelTrm;

    public Transform StagePanelTrm;

	private void Awake() 
	{
		ScreenOrientationHelper.SetOrientation(ScreenOrientationHelper.NScreenOrientation.Portrait);
	}

	private void OnEnable()
	{
		Messenger.AddListener<string>(GameEvent.SWITCH_MENU_STATE, OnSwitchMainmenu);
	}

	private void OnDisable()
	{
		Messenger.RemoveListener<string>(GameEvent.SWITCH_MENU_STATE, OnSwitchMainmenu);
	}

	private void OnSwitchMainmenu(string state) 
	{
		switch(state)
		{
			case "stage":
				SetState(MainMenuState.stage);
				break;
			case "login":
				SetState(MainMenuState.login);
				break;
		}
	}

	#region fsm

	public enum MainMenuState
	{
		login,
		stage,
		nil
	}

	MainMenuState curState = MainMenuState.nil;
	MainMenuState preState = MainMenuState.nil;

	public void SetState(MainMenuState state)
	{
		if (state == curState) return;

		LogMgr.I("AppMgr", "SetState", "状态切换到:" + state, true);

		preState = curState;

		StateExit(preState);

		curState = state;

		switch (curState)
		{
			case MainMenuState.login:
				LoginPanelTrm.gameObject.SetActive(true);
				break;
			case MainMenuState.stage:
				StagePanelTrm.gameObject.SetActive(true);
				break;
			case MainMenuState.nil:
				break;

		}
	}

	public bool IsCurState(MainMenuState state)
	{
		if (curState == state) return true;
		else return false;
	}

	void StateExit(MainMenuState preState)
	{
		switch (curState)
		{
			case MainMenuState.login:
				LoginPanelTrm.gameObject.SetActive(false);
				break;
			case MainMenuState.stage:
				StagePanelTrm.gameObject.SetActive(false);
				break;
			case MainMenuState.nil:
				break;

		}
	}

	#endregion

}
