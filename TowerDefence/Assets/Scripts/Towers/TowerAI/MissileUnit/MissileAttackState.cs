using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MissileAttackState : MissileBaseState
{
    private Transform closestTarget;
    private readonly LayerMask layerMask = LayerMask.GetMask("Enemies");
    private RaycastHit hit;
    private float cooldown = 5f;
    private float cooldownTime;
    private float speed = 1.0f;

    private Vector3 shootLocation;

    private float range = 50f; // Raycast range
    private float aoeRadius = 5f; // AoE radius for damage
    private float damageAmount = 10f; // Amount of damage for the AoE attack
    

    public MissileAttackState(GameObject go)
    {
        shootLocation = GameObject.FindWithTag("MissileShootLocation").transform.position;
    }
    
    public override void Enter(GameObject go)
    {
        Debug.Log("Turret: Attack State");
    }

    public override void Update(GameObject go)
    {
        // Find and identify the closest enemy
        closestTarget = UnitTracker.FindClosestEnemy(go)?.transform;
        

        if (closestTarget != null)
        {
            // Rotate towards the target
            Vector3 targetDirection = new Vector3(
                closestTarget.position.x - go.transform.position.x, 
                0, 
                closestTarget.position.z - go.transform.position.z).normalized;

            float singlestep = speed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(go.transform.forward, targetDirection, singlestep, 0.0f);
            go.transform.localRotation = Quaternion.LookRotation(newDirection);

            // Visualize the directions with debug rays
            Debug.DrawRay(go.transform.position, targetDirection * 10f, Color.red);  // Red points towards the target
            Debug.DrawRay(go.transform.position, go.transform.forward * 10f, Color.green); // Green shows forward direction

            // Check if there is a clear line of sight (Raycast) to the target
            Ray ray = new Ray(shootLocation, go.transform.forward);
            if (Physics.Raycast(ray, out hit, range, layerMask))
            {
                Debug.Log("Raycast hit: " + hit.transform.name);

                // Visualize the ray and hit point for debugging
                Debug.DrawRay(shootLocation, go.transform.forward * hit.distance, Color.red, 1.0f);
                Debug.DrawRay(hit.point, Vector3.up * 2f, Color.yellow, 2.0f);
                if (cooldownTime <= 0)
                {
                    cooldownTime = cooldown;
                    ApplyAOEDamage(hit.point);
                }
                else
                {
                    cooldownTime -= Time.deltaTime;
                }
            }
        }
        
    }

    void ApplyAOEDamage(Vector3 aoeCenter)
    {
        // Find all colliders within the aoeRadius around the hit point
        Collider[] hitColliders = Physics.OverlapSphere(aoeCenter, aoeRadius, layerMask);

        // Loop through each object in the radius
        foreach (var hitCollider in hitColliders)
        {
            // Get the GameObject that was hit
            GameObject target = hitCollider.gameObject;
                
            // Check if the target has a health or enemy component
            EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                // Apply damage to the enemy
                enemyHealth.EnemyTakeDamage(damageAmount);
                Debug.Log("Damaged enemy: " + target.name);
                if (enemyHealth.EnemyDeath())
                {
                    ObjectPoolManager.ReturnObjectToPool(target.gameObject);
                }
            }
        }
        // Optional: Visualize the AoE sphere for debugging
        Debug.DrawRay(aoeCenter, Vector3.up * 2f, Color.blue, 2.0f); // Draw the AoE center
    }

    public override void Exit(GameObject go)
    {
        
    }

    public override MissileBaseState HandleInput(GameObject go)
    {
        return null;
    }
    
    
}
