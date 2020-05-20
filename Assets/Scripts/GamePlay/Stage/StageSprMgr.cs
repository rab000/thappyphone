using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntools;
using NLog;

public class StageSprMgr : SingletonBehaviour<StageSprMgr>
{
	private bool BeShowLog = true;

    static Dictionary<string, AbsSpr> spriteDic = new Dictionary<string, AbsSpr>();

    static List<AbsSpr> spriteList = new List<AbsSpr>();

    private void OnEnable()
    {
		Messenger.AddListener<string, AbsSpr>(GameEvent.create_spr, AddSprEvent);
    }

    private void OnDisable()
    {
		Messenger.RemoveListener<string, AbsSpr>(GameEvent.create_spr, AddSprEvent);

	}

	private void AddSprEvent(string id,AbsSpr spr) 
	{
		Add(spr, id);
	}

	public string Add(AbsSpr spr, string id)
	{

		if (spr == null)
			LogMgr.E("spr  null");

		if (spriteDic.ContainsKey(id))
		{
			LogMgr.E("SpriteMgr", "Add", "id重复，插入id为" + id + "的spr失败", BeShowLog);
			return "error";
		}

		LogMgr.I("SpriteMgr", "Add", "AddSpr id=" + id);

		spriteList.Add(spr);

		spriteDic.Add(id, spr);

		spr.transform.parent = transform;

		// 这里要返回所有精灵ID，由精灵管理器统一管理，本地游戏这么管理，如果是服务器就忽略这个值
		return id;
	}


	public void RemoveSpr(AbsSpr spr, bool beUnload = true)
	{
		if (spriteDic.ContainsKey(spr.ID))
		{
			spriteDic.Remove(spr.ID);
			spriteList.Remove(spr);
			if (beUnload) spr.UnloadRes();
		}
	}

	public AbsSpr Get(string sprID)
	{
		if (spriteDic.ContainsKey(sprID))
		{
			return spriteDic[sprID];
		}
		LogMgr.I("SpriteMgr", "Get", "未获取到id为:" + sprID + "的精灵");
		return null;
	}

	public AbsSpr[] GetAllSpr()
	{
		return spriteList.ToArray();
	}

	
	public void tUpdate()
	{
		int size = spriteList.Count;
		for (int i = 0; i < size; i++)
		{
			spriteList[i].tUpdate();
		}
	}

	public void RemoveAllSpr(bool bUnloadHero = true)
	{

		int sprNum = spriteList.Count;

		for (int i = 0; i < sprNum; i++)
		{
			//if (!bUnloadHero && spriteList[i].Type == SpriteEnum.SPRITE_TYPE.Hero)
			//	continue;

			spriteList[i].UnloadRes();
			//这里是直接删除go还是由spr自身删除
			Debug.Log("清理spr go  name:" + spriteList[i].gameObject);
			DestroyImmediate(spriteList[i].gameObject);
		}

		spriteDic.Clear();

		spriteList.Clear();

	}


}
