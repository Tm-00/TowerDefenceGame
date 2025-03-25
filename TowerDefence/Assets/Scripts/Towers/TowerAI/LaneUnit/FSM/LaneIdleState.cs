using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneIdleState : LaneBaseState
{
    public LaneIdleState(GameObject go)
    {
        Debug.Log("Turret: IdleState");
    }
    public override void Enter(GameObject go)
    {
        
    }

    public override void Update(GameObject go)
    {
        
    }

    public override void Exit(GameObject go)
    {
        
    }

    public override LaneBaseState HandleInput(GameObject go)
    {
        return null;
    }
}
