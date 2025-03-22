using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class RobotAttackState : RobotBaseState
{
    // will be able to reference itself
    private NavMeshAgent agent;
    
    
    [Header("Robot Values")] 
    private readonly float rotationSpeed = 1.0f;
    [Header("Target Values")] 
    [Header("Attack Foundations")]
    
    
    private bool enemyKilled;
    private readonly Transform shootLocation;
    
    [Header("Attack Values")]
    private float range;
    
    // reference to the core node 
    private Transform coreNodePosition;
    private Transform closestTarget;
    private LayerMask layerMask;
    private RaycastHit hit;
    

    
    public RobotAttackState(GameObject go)
    {
        // assign variables 
        agent = go.gameObject.GetComponent<NavMeshAgent>();
        coreNodePosition = UnitTracker.UnitTargets[0].transform;
       
        RobotAttackHandler robotAttackHandler = go.AddComponent<RobotAttackHandler>();
        shootLocation = robotAttackHandler.shootLocation;
        range = robotAttackHandler.range;
    }
    
    // Enter
    public override void Enter(GameObject go)
    {
        Debug.Log("Robot Drone: Attack State");
    }

  public override void Update(GameObject go)
    {
        RobotAttackHandler robotAttackHandler = go.AddComponent<RobotAttackHandler>();
        
        closestTarget = UnitTracker.FindClosestWallUnit(agent)?.transform;
        
        if (closestTarget != null)
        {
            Vector3 targetDirection = new Vector3(closestTarget.position.x - go.transform.position.x, 0,
                closestTarget.position.z - go.transform.position.z).normalized;
            float singlestep = rotationSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(go.transform.forward, targetDirection, singlestep, 0.0f);
            go.transform.localRotation = Quaternion.LookRotation(newDirection);
            
            // Debug lines to visualize the directions
            Debug.DrawRay(shootLocation.position, targetDirection * 10f, Color.red);  // Red line pointing towards target
            Debug.DrawRay(go.transform.position, go.transform.forward * 10f, Color.green); // Green line showing current forward direction
            
            if (Physics.Raycast(shootLocation.position, go.transform.TransformDirection(Vector3.forward), out hit, range, layerMask))
            {
                GameObject targethit = hit.collider.gameObject;
                //  maybe add a check to see if target hit == cloest target to fix bug
                if (targethit != null)
                {
                    robotAttackHandler.AttackUnit(targethit);
                }
                TowerHealth targetHealth = targethit.GetComponent<TowerHealth>();
                if (targetHealth.Death())
                {
                    ObjectPoolManager.ReturnObjectToPool(targethit);
                    enemyKilled = true;
                }
            }
        }
    }
  
    // Exit
    public override void Exit(GameObject go)
    {
    
    }
    
    // input
    public override RobotBaseState HandleInput(GameObject go)
    {
        if (enemyKilled)
        {
            return new RobotMoveState(go);
        }
        return null;
    }
    
    
}
    

