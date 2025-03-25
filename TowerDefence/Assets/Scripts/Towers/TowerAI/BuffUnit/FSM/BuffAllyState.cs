using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuffAllyState : BuffBaseState
{
    [Header("Buff Values")] 
    private readonly float rotationSpeed = 1.0f;
    
    [Header("Target Values")] 
    private Transform closestAlly;
    private readonly LayerMask layerMask;
    private RaycastHit hit;
    
    [Header("Attack Foundations")]
    private readonly Transform shootLocation;
    
    [Header("Attack Values")]
    private readonly float range;
    private readonly float aoeRadius; 
    
    [Header("Class")]
    private readonly BuffHandler buffHandler;

    public BuffAllyState(GameObject go)
    {
        buffHandler = go.GetComponent<BuffHandler>();
        if (buffHandler == null)
        {
            buffHandler = go.AddComponent<BuffHandler>();
        }
        layerMask = buffHandler.layerMask;
        shootLocation = buffHandler.shootLocation;
        range = buffHandler.range;
    }
    
    public override void Enter(GameObject go)
    {
        Debug.Log("Healer: Heal State");
    }

    public override void Update(GameObject go)
    {
        // Find and identify the closest enemy
        closestAlly = UnitTracker.FindClosestAlly(go)?.transform;
        if (closestAlly != null)
        {
            // rotate unit towards target
            buffHandler.RotateUnitToTarget(go, closestAlly, rotationSpeed);
            // check if the shootlocation is assigned 
            if (shootLocation != null)
            {
                // shoot a raycast at a max distance of the range relating to the unit of origin
                if (Physics.Raycast(shootLocation.position, go.transform.forward, out hit, range, layerMask))
                {
                    buffHandler.ApplyAoeBuff(hit.point);
                }
            }
        }
    }

    public override void Exit(GameObject go)
    {
        
    }

    public override BuffBaseState HandleInput(GameObject go)
    {
        if (closestAlly == null)
        {
            return new BuffIdleState(go);
        }
        return null;
    }
    
}
