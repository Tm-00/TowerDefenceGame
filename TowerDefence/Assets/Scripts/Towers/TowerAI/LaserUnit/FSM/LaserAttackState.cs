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
    private readonly LayerMask layerMask;
    private RaycastHit hit;
    
    [Header("Attack Foundations")]
    private readonly Transform shootLocation;
    
    [Header("Attack Values")]
    private readonly float range;
    
    [Header("Class")]
    private readonly LaserAttackHandler laserAttackHandler;
    
    public LaserAttackState(GameObject go)
    {
        laserAttackHandler = go.GetComponent<LaserAttackHandler>();
        if (laserAttackHandler == null)
        {
            laserAttackHandler = go.AddComponent<LaserAttackHandler>();
        }
        layerMask = laserAttackHandler.layerMask;
        shootLocation = laserAttackHandler.shootLocation;
        range = laserAttackHandler.range;
    }
    
    public override void Enter(GameObject go)
    {
        Debug.Log("Laser Unit: Attack State");
    }

    public override void Update(GameObject go)
    {
        closestTarget = UnitTracker.FindClosestEnemy(go)?.transform;
        if (closestTarget != null)
        {
            // rotate unit towards target
            laserAttackHandler.RotateUnitToTarget(go, closestTarget, rotationSpeed);
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
                        laserAttackHandler.UnitAttack(targetHit);
                    }
                }
            }
        }
    }
    public override void Exit(GameObject go)
    {
        
    }

    public override LaserBaseState HandleInput(GameObject go)
    {
        return null;
    }
}
