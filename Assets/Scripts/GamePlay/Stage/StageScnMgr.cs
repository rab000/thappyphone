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

    private void StartLoad() 
    {
        //返回进度
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

}
