using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TurretAttackState : TurretBaseState
{
    [Header("Turret Values")] 
    private readonly float rotationSpeed = 1.0f;
    
    [Header("Target Values")] 
    private Transform closestTarget;
    private readonly LayerMask turretLayerMask;
    private RaycastHit hit;
    
    [Header("Class References")]
    private IAttackHandler attackHandler; 
    private IRotatable rotatable;
    private TurretAttackHandler turretAttackHandler;
    private readonly UnitTracker unitTracker;

    
    [Header("Attack Foundations")]
    private readonly Transform shootLocation;
    
    [Header("Attack Values")]
    private readonly float range;
    
    public TurretAttackState(GameObject go)
    {
        attackHandler = go.GetComponent<IAttackHandler>();
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
        
        GameObject gameManager = GameObject.Find("GameManager");
        unitTracker = gameManager.GetComponent<UnitTracker>();
        
        turretLayerMask = turretAttackHandler.layerMask;  
        shootLocation = turretAttackHandler.shootLocation;
        range = turretAttackHandler.range;
    }
    
    public override void Enter(GameObject go)
    {
        Debug.Log("Turret Unit: Attack State");
        closestTarget = unitTracker.FindClosestEnemy(go)?.transform;
    }

    public override void Update(GameObject go)
    {
        if (closestTarget != null)
        {
            // rotate unit towards target
            rotatable.RotateToTarget(go, closestTarget, rotationSpeed);
            
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
    }

    public override void Exit(GameObject go)
    {
        turretAttackHandler.ResetEnemyKilledStatus(); 
    }

    public override TurretBaseState HandleInput(GameObject go)
    {
        // if the unit kills an enemy or their target dies go to the locate state to find a new target 
        return turretAttackHandler.IsEnemyKilled() ? new TurretLocateEnemyState(go) : null;
    }
}
