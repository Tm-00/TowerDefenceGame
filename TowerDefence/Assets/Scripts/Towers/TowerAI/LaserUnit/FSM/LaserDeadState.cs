using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDeadState : LaserBaseState
{
    [Header("Class References")]
    internal LaserStats laserStats;
    
    public LaserDeadState(GameObject go)
    {
        laserStats = go.GetComponent<LaserStats>();
    }
    public override void Enter(GameObject go)
    {
        Debug.Log("Laser: DeadState");
        ObjectPoolManager.ReturnObjectToPool(go);
    }

    public override void Update(GameObject go)
    {

    }

    public override void Exit(GameObject go)
    {

    }

    public override LaserBaseState HandleInput(GameObject go)
    {
        if (laserStats.currentHealth > 0)
        {
            return new LaserIdleState(go);
        }
        return null;
    }
}