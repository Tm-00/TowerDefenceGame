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
    public int amount = 25;
    private bool enemyKilled;

    
    public FlightAttackState(GameObject go)
    {
        // assign variables 
        agent = go.gameObject.GetComponent<NavMeshAgent>();
        coreNodePosition = UnitTracker.UnitTargets[0].transform;
    }
    
    // Enter
    public override void Enter(GameObject go)
    {
        Debug.Log("Rifle Drone: Attack State");
    }

    // Update
    public override void Update(GameObject go)
    {
        closestTarget = UnitTracker.FindClosestWallUnit(agent)?.transform;
        
        if (closestTarget != null)
        {
            if (Physics.Raycast(agent.transform.position, agent.transform.TransformDirection(Vector3.forward), out hit,
                    5f, layerMask))
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
                targetHealth.TakeDamage(amount);
            }
            else
            {
                cooldownTime -= Time.deltaTime;
                //Debug.Log("active cd time " + cooldownTime);
            }
            if (targetHealth.Death())
            {
                ObjectPoolManager.ReturnObjectToPool(targethit);
            }
            //Debug.DrawRay(agent.transform.position, agent.transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);
        }
    }
}
    

