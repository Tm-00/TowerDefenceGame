using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileDeadState : MissileBaseState
{
    [Header("Class References")]
    internal MissileStats missileStats;
    
    public MissileDeadState(GameObject go)
    {
        missileStats = go.GetComponent<MissileStats>();
    }
    public override void Enter(GameObject go)
    {
        Debug.Log("Missile: DeadState");
        ObjectPoolManager.ReturnObjectToPool(go);
    }

    public override void Update(GameObject go)
    {
        
    }

    public override void Exit(GameObject go)
    {
        
    }

    public override MissileBaseState HandleInput(GameObject go)
    {
        if (missileStats.currentHealth > 0)
        {
            return new MissileIdleState(go);
        }
        return null;
    }
}