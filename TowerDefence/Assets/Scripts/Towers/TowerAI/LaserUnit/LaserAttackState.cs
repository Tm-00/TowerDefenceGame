using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LaserAttackState : LaserBaseState
{
    private Transform closestTarget;
    private LayerMask layerMask = LayerMask.GetMask("Enemies");
    private RaycastHit hit;
    private float cooldown = 7.5f;
    private float cooldownTime;
    private int damageAmount = 50;
    private float speed = 1.0f;
    private float range = 100f;

    

    public LaserAttackState(GameObject go)
    {
        
    }
    
    public override void Enter(GameObject go)
    {
        Debug.Log("Turret: Attack State");
    }

    public override void Update(GameObject go)
    {
        //TODO change logic so that it finds and identifies the enemy first then shoots raycast

        closestTarget = UnitTracker.FindClosestEnemy(go)?.transform;

        if (closestTarget!= null)
        {
            // rotate towards target
            Vector3 targetDirection = new Vector3(closestTarget.position.x - go.transform.position.x, 0,
                closestTarget.position.z - go.transform.position.z).normalized;
            float singlestep = speed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(go.transform.forward, targetDirection, singlestep, 0.0f);
            go.transform.localRotation = Quaternion.LookRotation(newDirection);
        
            // Debug lines to visualize the directions
            Debug.DrawRay(go.transform.position, targetDirection * 10f, Color.red);  // Red line pointing towards target
            Debug.DrawRay(go.transform.position, go.transform.forward * 10f, Color.green); // Green line showing current forward direction
        
            if (Physics.Raycast(go.transform.position, go.transform.TransformDirection(Vector3.forward), out hit, range, layerMask))
            {
                GameObject targethit = hit.collider.gameObject;
                if (targethit != null)
                {
                    AttackEnemy(targethit);
                }
                EnemyHealth enemyHealth = targethit.GetComponent<EnemyHealth>();
                if (enemyHealth.EnemyDeath())
                {
                    Debug.Log("array length " + UnitTracker.enemyArray.Length);
                    if (UnitTracker.enemyArray.Length > 1)
                    {
                        //go.transform.LookAt(closestTarget);
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
    
    private void AttackEnemy(GameObject targethit)
    {
        if (targethit != null)
        {
            EnemyHealth enemyHealth = targethit.GetComponent<EnemyHealth>();
            if (cooldownTime <= 0)
            {
                cooldownTime = cooldown;
                enemyHealth.EnemyTakeDamage(damageAmount);
            }
            else
            {
                cooldownTime -= Time.deltaTime;
                //Debug.Log("active cd time " + cooldownTime);
            }
            if (enemyHealth.EnemyDeath())
            {
                ObjectPoolManager.ReturnObjectToPool(targethit);
            }
            //Debug.DrawRay(go.transform.position, go.transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);
        }
        //Debug.Log("Did Hit");
    }
}
