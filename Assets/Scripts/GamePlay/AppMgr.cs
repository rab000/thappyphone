using NLog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppMgr : SingletonBehaviour<AppMgr>
{
	private void Start()
	{		
		GameConfig.LoadConfig();

		GameEnum.SB1.Append(GameEnum.StreamPath);
		GameEnum.SB1.Append("/bundle/");
		GameEnum.SB1.Append(GameEnum.CurPlatform.ToString());
		string xmlPath = GameEnum.SB1.ToString();
		GameEnum.SB1.Clear();
		SimpleResMgr.GetIns().LoadResXml(xmlPath);
	}

	#region helper
	public void SetFrameRate(int fr)
	{
		Application.targetFrameRate = fr;
	}

	public void SwitchScn(string scnName) 
	{		
		SceneManager.LoadScene(scnName);
	}

	public void ExitApp()
	{
		Application.Quit();
	}

    #endregion

    #region fsm

    public enum AppState
	{
		mainMenu,
		stageScn,
		gameScn,
		nil
	}

	AppState curState = AppState.nil;
	AppState preState = AppState.nil;

	public void SetState(AppState state)
	{
		if (state == curState) return;

		LogMgr.I("AppMgr", "SetState", "状态切换到:" + state,true);

		preState = curState;

		StateExit(preState);

		curState = state;

		switch (curState)
		{
			case AppState.mainMenu:
				break;
			case AppState.stageScn:
				//StageScnMgr.Create();
				break;
			case AppState.gameScn:
				break;

		}
	}

	public bool IsCurState(AppState state)
	{
		if (curState == state) return true;
		else return false;
	}

	public void tUpdate()
	{
		StateUpdate();		
	}

	public void StateUpdate()
	{
		switch (curState)
		{
			case AppState.mainMenu:
				break;
			case AppState.stageScn:
				break;
			case AppState.gameScn:
				break;

		}
	}

	void StateExit(AppState preState)
	{
		switch (curState)
		{
			case AppState.mainMenu:
				break;
			case AppState.stageScn:
				break;
			case AppState.gameScn:
				break;

		}
	}

    #endregion


}
