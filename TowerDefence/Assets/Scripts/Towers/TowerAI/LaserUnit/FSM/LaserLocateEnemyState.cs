using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LaserLocateEnemyState : LaserBaseState
{
    private Vector3 closestTarget;
    private float speed = 1.0f;
    private readonly UnitTracker unitTracker;

    
    public LaserLocateEnemyState(GameObject go)
    {
        GameObject gameManager = GameObject.Find("GameManager");
        unitTracker = gameManager.GetComponent<UnitTracker>();
    }
    public override void Enter(GameObject go)
    {
        Debug.Log("Laser: LocateEnemyState");
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

    public override LaserBaseState HandleInput(GameObject go)
    {
        // Move -> Attack
        if (Vector3.Distance(go.transform.position, closestTarget) <= 25)
        {
            return new LaserAttackState(go);
        }

        return null;
    }
}