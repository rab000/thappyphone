using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NLog;

public class ActorLoaderComp : Comp
{
    private bool BeShowLog = true;

    PlayerInfoStruct _PlayerInfoStruct;

    ActorRootComp _actorRootComp;
    ActorRootComp _ActorRootComp 
    {
        get 
        {
            if (null == _actorRootComp) _actorRootComp = (sprite as TActor)._ActorRootComp;
            return _actorRootComp;
        }
    }


    private AssetBundle bodyBundle;

    public ActorLoaderComp(AbsSpr spr) : base(spr) 
    {
        
    }
    public void StartLoad(PlayerInfoStruct info,Listener<string> OnLoadCB) 
    {
        _PlayerInfoStruct = info;

        Action<AssetBundle> cb = (bundle) =>
        {
            bodyBundle = bundle;

            var prefabObj = bundle.LoadAsset<GameObject>(_PlayerInfoStruct.BodyModelResID+".prefab");

            var prefabGo = GameObject.Instantiate(prefabObj) as GameObject;

            prefabGo.transform.SetParent(_ActorRootComp.OffsetTrm,false);

            OnLoadCB?.Invoke(_PlayerInfoStruct.ID);
            
        };

        Action<string> onError = (error) =>
        {
            LogMgr.E("PlayerLoaderComp","StartLoad","error:"+error,BeShowLog);
        };

        GameEnum.SB1.Append(GameConfig.ResRootPathStr);
        GameEnum.SB1.Append("/bundle/");
        GameEnum.SB1.Append(info.BodyModelResID);
        GameEnum.SB1.Append(GameEnum.BundleExtName);
        string path = GameEnum.SB1.ToString();
        GameEnum.SB1.Clear();

        LogMgr.I("ActorLoaderComp","StartLoad"," path:"+path,BeShowLog);

        SimpleResMgr.GetIns().LoadRes(path, cb, onError);

    }


}
