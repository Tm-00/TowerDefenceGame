using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class FlightAttackState : FlightBaseState
{
    // will be able to reference itself
    private NavMeshAgent agent;
    
    // reference to the core node 
    private Transform coreNodePosition;
    private Transform closestTarget;
    private LayerMask layerMask = LayerMask.GetMask("Towers");
    private RaycastHit hit;
    private float cooldown = 5f;
    private float cooldownTime;
    private int damageAmount = 50;
    private float range = 100f;
    private bool enemyKilled;
    private float speed = 1.0f;
    private Vector3 shootLocation;
    
    public FlightAttackState(GameObject go)
    {
        // assign variables 
        agent = go.gameObject.GetComponent<NavMeshAgent>();
        coreNodePosition = UnitTracker.UnitTargets[0].transform;
        shootLocation = GameObject.FindWithTag("FlightShootLocation").transform.position;
    }
    
    // Enter
    public override void Enter(GameObject go)
    {
        Debug.Log("Flight Drone: Attack State");
    }

    // Update
    public override void Update(GameObject go)
    {
        closestTarget = UnitTracker.FindClosestWallUnit(agent)?.transform;
        
        if (closestTarget != null)
        {
            Vector3 targetDirection = new Vector3(closestTarget.position.x - go.transform.position.x, 0,
                closestTarget.position.z - go.transform.position.z).normalized;
            float singlestep = speed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(go.transform.forward, targetDirection, singlestep, 0.0f);
            go.transform.localRotation = Quaternion.LookRotation(newDirection);
            
            // Debug lines to visualize the directions
            Debug.DrawRay(shootLocation, targetDirection * 10f, Color.red);  // Red line pointing towards target
            Debug.DrawRay(go.transform.position, go.transform.forward * 10f, Color.green); // Green line showing current forward direction
            
            if (Physics.Raycast(shootLocation, go.transform.TransformDirection(Vector3.forward), out hit, range, layerMask))
            {
                GameObject targethit = hit.collider.gameObject;
                //  maybe add a check to see if target hit == cloest target to fix bug
                if (targethit != null)
                {
                    AttackUnit(targethit);
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
    public override FlightBaseState HandleInput(GameObject go)
    {
        if (enemyKilled)
        {
            return new FlightMoveState(go);
        }
        return null;
    }
    
    // TODO change into a public method in a different class that all units can use
    private void AttackUnit(GameObject targethit)
    {
        if (targethit != null)
        {
            TowerHealth targetHealth = targethit.GetComponent<TowerHealth>();
            if (cooldownTime <= 0)
            {
                cooldownTime = cooldown;
                targetHealth.TakeDamage(damageAmount);
            }
            else
            {
                cooldownTime -= Time.deltaTime;
                //Debug.Log("active cd time " + cooldownTime);
            }
            //Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);
        }
    }
}
    

