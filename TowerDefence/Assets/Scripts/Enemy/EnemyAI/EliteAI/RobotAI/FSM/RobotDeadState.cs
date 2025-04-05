using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotDeadState : RobotBaseState
{
    [Header("Class References")]
    internal RobotStats robotStats;
    
    
    public RobotDeadState(GameObject go)
    {
        robotStats = go.GetComponent<RobotStats>();
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

    public override RobotBaseState HandleInput(GameObject go)
    {
        if (robotStats.currentHealth > 0)
        {
            return new RobotIdleState(go);
        }
        return null;
    }
}
