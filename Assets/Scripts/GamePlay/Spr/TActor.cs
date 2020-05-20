using UnityEngine;
using ntools;

public class TActor : AbsSpr
{

    #region comp

    public ActorRootComp _ActorRootComp;

    //演出播放器
    private ActorPerformComp _ActorPerformComp;

    //加载器
    private ActorLoaderComp _ActorLoaderComp;

    //表情控制
    private ActorFaceCtrComp _ActorFaceCtrComp;

    #endregion

    #region mono

    protected override void Awake()
    {
        _ActorRootComp = new ActorRootComp(this);
        _ActorPerformComp = new ActorPerformComp(this);
        _ActorLoaderComp = new ActorLoaderComp(this);
        _ActorFaceCtrComp = new ActorFaceCtrComp(this);
    }

    private void OnDestroy()
    {
        
    }

    #endregion

    #region func

    public void LoadModel(PlayerInfoStruct info,Listener<string> cb) 
    {
        _ActorLoaderComp.StartLoad(info,cb);
    }

    #endregion


    #region simple factory

    //public static event Listener PlayerLoadoverEvent;

    public static TActor Create(PlayerInfoStruct info,Listener<string> OnLoadModelOver) 
    {
        NLog.LogMgr.I("TActor","Create"," 创建角色:"+info.ID,true);

        var playerRootGo = GameObject.Instantiate(Resources.Load("Prefabs/Spr/player")) as GameObject;

        playerRootGo.name = "player";       

        TActor _player = playerRootGo.AddComponent<TActor>();

        _player.ID = info.ID;

        _player._ActorRootComp.RootTrm = playerRootGo.transform;

        _player._ActorRootComp.OffsetTrm = playerRootGo.transform.Find("offset");

        _player.LoadModel(info,OnLoadModelOver);

        Messenger.Broadcast<string, AbsSpr>(GameEvent.create_spr, _player.ID, _player);

        return _player;

    }

    //private static void OnLoadModelOver(string ID)
    //{
    //    PlayerLoadoverEvent?.Invoke();
    //}

    #endregion

}


public struct PlayerInfoStruct 
{
    public string ID;

    public string BodyModelResID;

    public string FaceModelResID;

    public string HairModelResID;

}