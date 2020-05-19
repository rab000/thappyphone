using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct StageScnInfoStruct
{
    public string ScnName;

    public List<StageRoleInfoStruct> roleList;

}

public struct StageRoleInfoStruct
{
    public string BodyResID;

    public string HeadResID;

    public string HairResID;

}
