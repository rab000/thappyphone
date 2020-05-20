using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPlayer : AbsSpr
{

    #region comp

    //演出播放器
    private PlayerPerformComp _PerformComp;

    //加载器
    private PlayerLoaderComp _PlayerLoaderComp;

    //表情控制
    private PlayerFaceCtrComp _PlayerFaceCtrComp;

    #endregion

    #region mono

    protected override void Awake()
    {
        _PerformComp = new PlayerPerformComp(this);
        _PlayerLoaderComp = new PlayerLoaderComp(this);
        _PlayerFaceCtrComp = new PlayerFaceCtrComp(this);

    }

    private void OnDestroy()
    {
        
    }

    #endregion


    #region simple factory

    public static event Listener PlayerLoadoverEvent;

    public static TPlayer Create(PlayerInfnStruct info) 
    {
        var playerRootGo = GameObject.Instantiate(Resources.Load("Prefabs/Spr/player")) as GameObject;

        playerRootGo.name = "player";

        TPlayer _player = playerRootGo.AddComponent<TPlayer>();

        return null;

    }

    #endregion

}


public struct PlayerInfnStruct 
{
    public string ID;

    public string Name;

    public string BodyModelResID;

    public string FaceModelResID;

    public string HairModelResID;

}