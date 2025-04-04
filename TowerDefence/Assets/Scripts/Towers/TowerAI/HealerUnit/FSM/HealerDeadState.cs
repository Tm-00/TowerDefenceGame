using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerDeadState : HealerBaseState
{
    [Header("Class References")]
    internal HealerStats healerStats;
    
    public HealerDeadState(GameObject go)
    {
        healerStats = go.GetComponent<HealerStats>();
    }
    public override void Enter(GameObject go)
    {
        Debug.Log("Healer: DeadState");
        ObjectPoolManager.ReturnObjectToPool(go);
    }

    public override void Update(GameObject go)
    {
   
    }

    public override void Exit(GameObject go)
    {
       
    }

    public override HealerBaseState HandleInput(GameObject go)
    {
        if (healerStats.currentHealth > 0)
        {
            return new HealerIdleState(go);
        }
        return null;
    }
}