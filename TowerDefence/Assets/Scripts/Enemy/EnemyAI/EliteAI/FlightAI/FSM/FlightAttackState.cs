using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class FlightAttackState : FlightBaseState
{
    [Header("Flight Values")] 
    private readonly float rotationSpeed = 1.0f;
    private NavMeshAgent agent;
    
    [Header("Target Values")] 
    private Transform coreNodePosition;
    private Transform closestTarget;
    private LayerMask flightLayerMask;
    private RaycastHit hit;
    
    [Header("Class References")]
    private IAttackHandler attackHandler; 
    private IRotatable rotatable;
    private readonly FlightAttackHandler flightAttackHandler;
    
    [Header("Attack Foundations")]
    private readonly Transform shootLocation;
    private bool enemyKilled;
    
    [Header("Attack Values")]
    private readonly float range;
    
    public FlightAttackState(GameObject go)
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
        
        flightAttackHandler = go.GetComponent<FlightAttackHandler>();
        if (rotatable == null)
        {
            Debug.LogError("GameObject is missing an FlightAttackHandler component!");
        }
        
        flightLayerMask = flightAttackHandler.layerMask;
        shootLocation = flightAttackHandler.shootLocation;
        range = flightAttackHandler.range;
    }
    
    // Enter
    public override void Enter(GameObject go)
    {
        Debug.Log("Flight Drone: Attack State");
        agent = go.GetComponent<NavMeshAgent>();
        coreNodePosition = UnitTracker.UnitTargets[0].transform;
        closestTarget = UnitTracker.FindClosestUnit(agent)?.transform;
    }

 public override void Update(GameObject go)
    {
        if (closestTarget != null)
        {
            // rotate unit towards target
            rotatable.RotateToTarget(go, closestTarget, rotationSpeed);

            if (shootLocation != null)
            {
                if (Physics.Raycast(shootLocation.position, go.transform.TransformDirection(Vector3.forward), out hit, range, flightLayerMask))
                {
                    // confirm a target was hit then store it as a gameobject 
                    var targetHit = hit.collider.gameObject;
                    
                    // check that the target hit was the cloest target then perform attack methods
                    if (targetHit != null && targetHit == closestTarget.gameObject)
                    {
                        flightAttackHandler.Attack(targetHit);
                    }
                }
            }
        }
    }
  
    // Exit
    public override void Exit(GameObject go)
    {
        flightAttackHandler.ResetEnemyKilledStatus(); 
    }
    
    // input
    public override FlightBaseState HandleInput(GameObject go)
    {
        // if the unit kills an enemy or their target dies go to the move state to find a new target 
        return flightAttackHandler.IsEnemyKilled() ? new FlightMoveState(go) : null;
    }
}
    

