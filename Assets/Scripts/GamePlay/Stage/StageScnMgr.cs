using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntools;

/// <summary>
/// 
/// 舞台管理
/// 
/// 先填充属性，然后再创建StageScnMgr
/// 
/// </summary>
public class StageScnMgr : SingletonBehaviour<StageScnMgr>
{

	private static bool BeShowLog = true;

	//场景资源，角色信息
    public static StageScnInfoStruct scnInfo;

	//加载资源到固定阶段的进度
	private const float ScnProgress = 0.4f;
	private const float RoleProgress = 0.4f;
	private const float OtherProgress = 0.2f;

	//场景bundle
	private AssetBundle ScnBundle;

    public static void Create() 
    {
        GameObject go = new GameObject("stageScnMgr");
        var stageScnMgr = go.AddComponent<StageScnMgr>();
		go.AddComponent<StageSprMgr>();

    }

    private void Start()
    {
		SetState(StageState.loadScn);
	}

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
				GameEnum.SB1.Append(GameEnum.BundleExtName);
				string scnBundlePath = GameEnum.SB1.ToString();				
				LogMgr.I("StageScnMgr","SetState","加载stage场景 scnBundlePath:"+ scnBundlePath, BeShowLog);
				GameEnum.SB1.Clear();

				Action<float> loadScnProgress = (progress) =>
				{
					float per = ScnProgress * progress;
					Messenger.Broadcast<float>(GameEvent.loading_progress, per);
				};

				Action<AssetBundle> loadScnCB = (bundle) =>
				{
					ScnBundle = bundle;

					LogMgr.I("StageScnMgr", "SetState", "加载stage完毕 scnBundlePath:" + scnBundlePath, BeShowLog);

					Messenger.Broadcast<float>(GameEvent.loading_progress, ScnProgress);

					SetState(StageState.loadRole);
				};

				Action<string> errorCB = (error) =>
				{
					LogMgr.E("StageScnMgr", "SetState", " error:"+error, BeShowLog);
				};

				SimpleResMgr.GetIns().LoadScn(scnInfo.ScnName, scnBundlePath, loadScnProgress, loadScnCB, errorCB);

				break;

			case StageState.loadRole:

				LoadRole();

				break;

			case StageState.loadOther:
				AppMgr.GetIns().SetState(AppMgr.AppState.stageScn);
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


	void LoadRole() 
	{
		Listener onLoadAllRoleOver = () =>
		{
			LogMgr.I("StageScnMgr", "LoadRole", "全部角色载入完毕:", BeShowLog);
			SetState(StageState.loadOther);
		};

		CallbackMgr cbMgr = new CallbackMgr(onLoadAllRoleOver);

		cbMgr.ProgressEvent = (progress) => {

			//20-100的进度转换，就是20+80*progress           
			float p = ScnProgress + RoleProgress * progress;
			ntools.Messenger.Broadcast<float>(GameEvent.loading_progress, p);
			LogMgr.I("StageScnMgr", "LoadRole", "载入进度 progress:" + progress, BeShowLog);
		};

		int count = scnInfo.roleList.Count;
		for (int i = 0; i < count; i++)
		{
			var cb = LoadRole(scnInfo.roleList[i]);
			cbMgr.Add(cb);
		}

		cbMgr.Start();
		
	}

	private BaseCallback LoadRole(PlayerInfoStruct roleInfo)
	{
		BaseCallback cb = new BaseCallback();
		cb.OnStart = () => {

			Listener<string> onloadOver = (id) =>
			{
				LogMgr.I("StageScnMgr","LoadRole.cb.OnStart.onloadOver"," id:"+id,BeShowLog);
				cb.End();
			};

			TActor.Create(roleInfo, onloadOver);

		};

		return cb;
	}


}
