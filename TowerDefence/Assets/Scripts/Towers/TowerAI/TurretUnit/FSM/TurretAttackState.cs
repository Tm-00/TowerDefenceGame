using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TurretAttackState : TurretBaseState
{
    [Header("Turret Values")]
    private const float RotationSpeed = 10.0f;

    [Header("Target Values")] 
    private Transform closestTarget;
    private readonly LayerMask turretLayerMask;
    private RaycastHit hit;

    [Header("Interface References")]
    private readonly IRotatable rotatable;
    
    [Header("Class References")]
    private readonly TurretAttackHandler turretAttackHandler;
    private readonly TurretStats turretStats;
    private readonly UnitTracker unitTracker;

    [Header("Attack Foundations")]
    private readonly Transform shootLocation;
    
    [Header("Attack Values")]
    private readonly float range;
    
    public TurretAttackState(GameObject go)
    {
        var attackHandler = go.GetComponent<IAttackHandler>();
        var gameManager = GameObject.Find("GameManager");
        
        if (attackHandler == null)
        {
            Debug.LogError("GameObject is missing an IAttackHandler component!");
        }
        
        rotatable = go.GetComponent<IRotatable>();
        if (rotatable == null)
        {
            Debug.LogError("GameObject is missing an IRotatable component!");
        }

        turretAttackHandler = go.GetComponent<TurretAttackHandler>();
        if (rotatable == null)
        {
            Debug.LogError("GameObject is missing an TurretAttackHandler component!");
        }
        
        turretStats = go.GetComponent<TurretStats>();
        if (rotatable == null)
        {
            Debug.LogError("GameObject is missing an TurretStats component!");
        }
        
        
        unitTracker = gameManager.GetComponent<UnitTracker>();
        turretLayerMask = turretAttackHandler.layerMask;  
        shootLocation = turretAttackHandler.shootLocation;
        range = turretAttackHandler.range;
    }
    
    public override void Enter(GameObject go)
    {
        Debug.Log("Turret Unit: Attack State");
    }

    public override void Update(GameObject go)
    {
        closestTarget = unitTracker.FindClosestEnemy(go)?.transform;
        
        if (closestTarget != null)
        {
            // rotate unit towards target
            rotatable.RotateToTarget(go, closestTarget, RotationSpeed);
            
            // check if the shootlocation is assigned 
            if (shootLocation != null)
            {
                // shoot a raycast at a max distance of the range relating to the unit
                if (Physics.Raycast(shootLocation.position, go.transform.TransformDirection(Vector3.forward), out hit, range, turretLayerMask))
                {
                    // confirm a target was hit then store it as a gameobject 
                    var targetHit = hit.collider.gameObject;
                    
                    // check that the target hit was the cloest target then perform attack methods
                    if (targetHit != null && targetHit == closestTarget.gameObject)
                    {
                        turretAttackHandler.Attack(targetHit);
                    }
                }
            }
        }
        Debug.DrawRay(shootLocation.transform.position, shootLocation.transform.forward * 10f, Color.green); // Green line showing current forward direction
    }

    public override void Exit(GameObject go)
    {
        turretAttackHandler.ResetEnemyKilledStatus(); 
    }

    public override TurretBaseState HandleInput(GameObject go)
    {
        // if the unit kills an enemy or their target dies go to the locate state to find a new target 
        if (turretAttackHandler.IsEnemyKilled())
        {
            return new TurretLocateEnemyState(go);
        }
        if (turretStats.currentHealth <= 0)
        {
            return new TurretDeadState(go);
        }
        return null;
    }
}
