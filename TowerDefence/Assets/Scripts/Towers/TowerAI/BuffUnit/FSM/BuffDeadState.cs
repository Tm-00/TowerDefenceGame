using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDeadState : BuffBaseState
{
    [Header("Class References")]
    internal BuffStats buffStats;
    
    public BuffDeadState(GameObject go)
    {
        buffStats = go.GetComponent<BuffStats>();
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
        if (buffStats.currentHealth > 0)
        {
            return new BuffIdleState(go);
        }
        return null;
    }
}