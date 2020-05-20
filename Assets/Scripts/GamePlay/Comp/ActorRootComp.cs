using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorRootComp : Comp
{
    public Transform RootTrm;

    public Transform OffsetTrm;
    public ActorRootComp(AbsSpr spr) : base(spr) 
    {
    
    }

}
