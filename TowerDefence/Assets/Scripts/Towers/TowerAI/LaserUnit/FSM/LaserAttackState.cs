using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LaserAttackState : LaserBaseState
{
    [Header("Laser Values")] 
    private readonly float rotationSpeed = 1.0f;
    
    [Header("Target Values")] 
    private Transform closestTarget;
    private readonly LayerMask laserLayerMask;
    private RaycastHit hit;
    
    [Header("Class References")]
    private IAttackHandler attackHandler; 
    private IRotatable rotatable;
    private LaserAttackHandler laserAttackHandler;
    
    [Header("Attack Foundations")]
    private readonly Transform shootLocation;
    
    [Header("Attack Values")]
    private readonly float range;
    
    public LaserAttackState(GameObject go)
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

        laserAttackHandler = go.GetComponent<LaserAttackHandler>();
        if (rotatable == null)
        {
            Debug.LogError("GameObject is missing an LaserAttackHandler component!");
        }
        
        laserLayerMask = laserAttackHandler.layerMask;  
        shootLocation = laserAttackHandler.shootLocation;
        range = laserAttackHandler.range;
    }
    
    public override void Enter(GameObject go)
    {
        Debug.Log("Laser Unit: Attack State");
        closestTarget = UnitTracker.FindClosestEnemy(go)?.transform;
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
                if (Physics.Raycast(shootLocation.position, go.transform.TransformDirection(Vector3.forward), out hit, range, laserLayerMask))
                {
                    // confirm a target was hit then store it as a gameobject 
                    var targetHit = hit.collider.gameObject;
                    
                    // check that the target hit was the cloest target then perform attack methods
                    if (targetHit != null && targetHit == closestTarget.gameObject)
                    {
                        laserAttackHandler.Attack(targetHit);
                    }
                }
            }
        }
    }
    public override void Exit(GameObject go)
    {
        laserAttackHandler.ResetEnemyKilledStatus();
    }

    public override LaserBaseState HandleInput(GameObject go)
    {
        // if the unit kills an enemy or their target dies go to the locate state to find a new target 
        return laserAttackHandler.IsEnemyKilled() ? new LaserLocateEnemyState(go) : null;
    }
}
