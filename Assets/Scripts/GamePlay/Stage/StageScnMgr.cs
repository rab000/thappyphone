using NLog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// 舞台管理
/// 
/// 先填充属性，然后再创建StageScnMgr
/// 
/// </summary>
public class StageScnMgr : SingletonBehaviour<StageScnMgr>
{

    public static StageScnInfoStruct scnInfo;

    public static void Create() 
    {

        GameObject go = new GameObject("stageScnMgr");

        var stageScnMgr = go.AddComponent<StageScnMgr>();

    }

    private void Start()
    {
        StartLoad();
    }

    #region load
    private void StartLoad() 
    {
		SetState(StageState.loadScn);
	}

    public void LoadStage() 
    {
        
    }

    public void LoadRole() 
    {
        
    }

    public void LoadUI() 
    {
        
    }

    #endregion


    #region fsm

    public enum StageState
	{
		loadScn,
		loadRole,
		loadOther,
		initStage,
		unloading,
		nil
	}

	public StageState curState = StageState.nil;
	public StageState preState = StageState.nil;

	public void SetState(StageState state)
	{
		if (state == curState) return;

		LogMgr.I("StageScnMgr", "SetState", "状态切换到:" + state, true);

		preState = curState;

		StateExit(preState);

		curState = state;

		switch (curState)
		{
			case StageState.loadScn:							
				GameEnum.SB1.Append(GameConfig.ResRootPathStr);
				GameEnum.SB1.Append("/bundle/");
				GameEnum.SB1.Append(scnInfo.ScnName);
				GameEnum.SB1.Append(".ab");
				string scnBundlePath = GameEnum.SB1.ToString();
				Debug.Log("------------->加载scn:"+ scnBundlePath);
				GameEnum.SB1.Clear();
				SimpleResMgr.GetIns().LoadScn(scnInfo.ScnName, scnBundlePath, null,null,null);
				break;

			case StageState.loadRole:
				
				break;
			case StageState.loadOther:
				break;
			case StageState.initStage:
				break;
			case StageState.unloading:
				break;

		}
	}

	public bool IsCurState(StageState state)
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
			case StageState.loadScn:
				break;
			case StageState.loadRole:

				break;
			case StageState.loadOther:
				break;
			case StageState.initStage:
				break;
			case StageState.unloading:
				break;

		}
	}

	void StateExit(StageState preState)
	{
		switch (curState)
		{
			case StageState.loadScn:
				break;
			case StageState.loadRole:
				break;
			case StageState.loadOther:
				break;
			case StageState.initStage:
				break;
			case StageState.unloading:
				break;

		}
	}

	#endregion

}
