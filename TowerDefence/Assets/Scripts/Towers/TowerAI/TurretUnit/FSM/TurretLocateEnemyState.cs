using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TurretLocateEnemyState : TurretBaseState
{
    private Vector3 closestTarget;
    private float speed = 1.0f;

    private TurretStats turretStats;
    private readonly UnitTracker unitTracker;

    
    public TurretLocateEnemyState(GameObject go)
    {
        GameObject gameManager = GameObject.Find("GameManager");
        unitTracker = gameManager.GetComponent<UnitTracker>();
        turretStats = go.GetComponent<TurretStats>();
    }
    public override void Enter(GameObject go)
    {
        Debug.Log("Turret: LocateEnemyState");
    }

    public override void Update(GameObject go)
    {
        var cloestEnemy = unitTracker.FindClosestEnemy(go);
        if (cloestEnemy != null)
        {
            closestTarget = unitTracker.FindClosestEnemy(go).transform.position;
            Vector3 targetDirection = closestTarget - go.transform.position;
            float singlestep = speed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(go.transform.forward, targetDirection, singlestep, 0.0f);
            go.transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }

    public override void Exit(GameObject go)
    {
        
    }

    public override TurretBaseState HandleInput(GameObject go)
    {
        // Move -> Attack
        if (Vector3.Distance(go.transform.position, closestTarget) <= 10)
        {
            return new TurretAttackState(go);
        }
        if (turretStats.currentHealth <= 0)
        {
            return new TurretDeadState(go);
        }

        return null;
    }
}