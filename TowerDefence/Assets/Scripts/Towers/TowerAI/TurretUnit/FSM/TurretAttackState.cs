using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TurretAttackState : TurretBaseState
{
    [Header("Flight Values")] 
    private readonly float rotationSpeed = 1.0f;
    
    [Header("Target Values")] 
    private Transform closestTarget;
    
    [Header("Attack Foundations")]
    private readonly Transform shootLocation;
    private bool enemyKilled;
    
    [Header("Attack Values")]
    private float range;
    
    // reference to the core node 
    private LayerMask layerMask;
    private RaycastHit hit;
    private TurretAttackHandler turretAttackHandler;
    

    public TurretAttackState(GameObject go)
    {
        turretAttackHandler = go.GetComponent<TurretAttackHandler>();
        if (turretAttackHandler == null)
        {
            turretAttackHandler = go.AddComponent<TurretAttackHandler>();
        }

        
        layerMask = turretAttackHandler.layerMask;
        shootLocation = turretAttackHandler.shootLocation;
        range = turretAttackHandler.range;
    }
    
    public override void Enter(GameObject go)
    {
        Debug.Log("Turret Unit: Attack State");
    }

    public override void Update(GameObject go)
    {
        closestTarget = UnitTracker.FindClosestEnemy(go)?.transform;

        if (closestTarget != null)
        {
            RotateUnitToTarget(go);

            if (shootLocation != null)
            {
                if (Physics.Raycast(shootLocation.position, go.transform.TransformDirection(Vector3.forward), out hit, range, layerMask))
                {
                    GameObject targethit = hit.collider.gameObject;
                    if (targethit != null && targethit == closestTarget.gameObject)
                    {
                        turretAttackHandler.UnitAttack(targethit);
                    }

                    FlightStats flightStats = targethit.GetComponent<FlightStats>();
                    if (flightStats.EnemyDeath())
                    {
                        ObjectPoolManager.ReturnObjectToPool(targethit);
                        enemyKilled = true;
                    }
                }
            }
            else
            {
                Debug.LogWarning("shootLocation is null. Please check initialization.");
            }

        }
        else
        {
            Debug.LogWarning("No target found.");
        }
        
        
    }

    public override void Exit(GameObject go)
    {
        
    }

    public override TurretBaseState HandleInput(GameObject go)
    {
        return null;
    }

    private void DeathHandler(GameObject targethit)
    {
        RifleStats rifleStats = targethit.GetComponent<RifleStats>();
        ScoutStats scoutStats = targethit.GetComponent<ScoutStats>();
        FlightStats flightStats = targethit.GetComponent<FlightStats>();
        RobotStats robotStats = targethit.GetComponent<RobotStats>();
                
        if (rifleStats.EnemyDeath())
        {
            ObjectPoolManager.ReturnObjectToPool(targethit);
            enemyKilled = true;
        }
        
        if (scoutStats.EnemyDeath())
        {
            ObjectPoolManager.ReturnObjectToPool(targethit);
            enemyKilled = true;
        }
        
        if (flightStats.EnemyDeath())
        {
            ObjectPoolManager.ReturnObjectToPool(targethit);
            enemyKilled = true;
        }
        
        if (robotStats.EnemyDeath())
        {
            ObjectPoolManager.ReturnObjectToPool(targethit);
            enemyKilled = true;
        }
    }
    
    private void RotateUnitToTarget(GameObject go)
    {
        Vector3 targetDirection = new Vector3(closestTarget.position.x - go.transform.position.x, 0,
            closestTarget.position.z - go.transform.position.z).normalized;
        float singlestep = rotationSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(go.transform.forward, targetDirection, singlestep, 0.0f);
        go.transform.localRotation = Quaternion.LookRotation(newDirection);
        //DrawRay(targetDirection, go);
    }

    private void DrawRay(Vector3 targetDirection, GameObject go)
    {
        // Debug lines to visualize the directions
        Debug.DrawRay(shootLocation.position, targetDirection * 10f, Color.red);  // Red line pointing towards target
        Debug.DrawRay(shootLocation.transform.position, go.transform.forward * 10f, Color.green); // Green line showing current forward direction
    }
}
