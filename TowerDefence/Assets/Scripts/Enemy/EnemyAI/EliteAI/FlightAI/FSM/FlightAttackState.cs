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
    
    [Header("Attack Foundations")]
    private readonly Transform shootLocation;
    private bool enemyKilled;
    
    [Header("Attack Values")]
    private float range;
    
    // reference to the core node 
    private LayerMask layerMask;
    private RaycastHit hit;
    
    
    public FlightAttackState(GameObject go)
    {
        // assign variables 
        agent = go.gameObject.GetComponent<NavMeshAgent>();
        coreNodePosition = UnitTracker.UnitTargets[0].transform;
        
        FlightAttackHandler flightAttackHandler = go.AddComponent<FlightAttackHandler>();
        
        layerMask = flightAttackHandler.layerMask;
        shootLocation = flightAttackHandler.shootLocation;
        range = flightAttackHandler.range;
    }
    
    // Enter
    public override void Enter(GameObject go)
    {
        Debug.Log("Flight Drone: Attack State");
    }

 public override void Update(GameObject go)
    {
        FlightAttackHandler flightAttackHandler = go.AddComponent<FlightAttackHandler>();
        
        closestTarget = UnitTracker.FindClosestWallUnit(agent)?.transform;
        
        if (closestTarget != null)
        {
            RotateUnitToTarget(go);
            
            if (Physics.Raycast(shootLocation.position, go.transform.TransformDirection(Vector3.forward), out hit, range, layerMask))
            {
                GameObject targethit = hit.collider.gameObject;
                if (targethit != null && targethit == closestTarget.gameObject)
                {
                    flightAttackHandler.FlightAttackUnit(targethit);
                }
                
            }
        }
    }
  
    // Exit
    public override void Exit(GameObject go)
    {
    
    }
    
    // input
    public override FlightBaseState HandleInput(GameObject go)
    {
        if (enemyKilled)
        {
            // if the target died find new target
            return new FlightMoveState(go);
        }
        return null;
    }

    private void RotateUnitToTarget(GameObject go)
    {
        Vector3 targetDirection = new Vector3(closestTarget.position.x - go.transform.position.x, 0,
            closestTarget.position.z - go.transform.position.z).normalized;
        float singlestep = rotationSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(go.transform.forward, targetDirection, singlestep, 0.0f);
        go.transform.localRotation = Quaternion.LookRotation(newDirection);
        DrawRay(targetDirection, go);
    }

    private void DrawRay(Vector3 targetDirection, GameObject go)
    {
        // Debug lines to visualize the directions
        Debug.DrawRay(shootLocation.position, targetDirection * 10f, Color.red);  // Red line pointing towards target
        Debug.DrawRay(shootLocation.transform.position, go.transform.forward * 10f, Color.green); // Green line showing current forward direction
    }
}
    

