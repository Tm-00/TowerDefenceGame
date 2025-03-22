using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretIdleState : TurretBaseState
{
    public TurretIdleState(GameObject go)
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

    public override TurretBaseState HandleInput(GameObject go)
    {
        if (UnitTracker.EnemyTargets != null && TowerPlacement.hasBeenPlaced)
        {
            if (UnitTracker.EnemyTargets.Count >= 1)
            {
                return new TurretLocateEnemyState(go);
            }
        }
        return null;
    }
}
