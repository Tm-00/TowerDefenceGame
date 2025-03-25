using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffIdleState : BuffBaseState
{
    public BuffIdleState(GameObject go)
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

    public override BuffBaseState HandleInput(GameObject go)
    {
        if (UnitTracker.UnitTargets != null && TowerPlacement.hasBeenPlaced)
        {
            if (UnitTracker.UnitTargets.Count >= 1)
            {
                return new BuffLocateAllyState(go);
            }
        }
        return null;
    }
}
