using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleDeadState : RifleBaseState
{
    [Header("Class References")]
    internal RifleStats rifleStats;
    
    public RifleDeadState(GameObject go)
    {
        rifleStats = go.GetComponent<RifleStats>();
    }
    public override void Enter(GameObject go)
    {
        ObjectPoolManager.ReturnObjectToPool(go);
    }

    public override void Update(GameObject go)
    {
    }

    public override void Exit(GameObject go)
    {
    }

    public override RifleBaseState HandleInput(GameObject go)
    {
        if (rifleStats.currentHealth > 0)
        {
            return new RifleIdleState(go);
        }
        return null;
    }
}
