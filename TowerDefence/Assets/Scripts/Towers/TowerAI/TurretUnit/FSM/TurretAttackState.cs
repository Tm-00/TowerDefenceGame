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
    private readonly LayerMask layerMask;
    private RaycastHit hit;
    
    [Header("Attack Foundations")]
    private readonly Transform shootLocation;
    
    [Header("Attack Values")]
    private readonly float range;
    
    [Header("Class")]
    private readonly TurretAttackHandler turretAttackHandler;
    

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
            // rotate unit towards target
            turretAttackHandler.RotateUnitToTarget(go, closestTarget, rotationSpeed);
            
            // check if the shootlocation is assigned 
            if (shootLocation != null)
            {
                // shoot a raycast at a max distance of the range relating to the unit
                if (Physics.Raycast(shootLocation.position, go.transform.TransformDirection(Vector3.forward), out hit, range, layerMask))
                {
                    // confirm a target was hit then store it as a gameobject 
                    var targetHit = hit.collider.gameObject;
                    
                    // check that the target hit was the cloest target then perform attack methods
                    if (targetHit != null && targetHit == closestTarget.gameObject)
                    {
                        turretAttackHandler.UnitAttack(targetHit);
                    }
                }
            }
        }
    }

    public override void Exit(GameObject go)
    {
        turretAttackHandler.enemyKilled = false;
    }

    public override TurretBaseState HandleInput(GameObject go)
    {
        // if the unit kills an enemy or their target dies go to the locate state to find a new target 
        return turretAttackHandler.enemyKilled ? new TurretLocateEnemyState(go) : null;
    }
}
