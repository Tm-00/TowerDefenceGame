using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackHandler : MonoBehaviour, IAttackHandler, IRotatable
{
    [Header("Unit Values")] 
    public Transform shootLocation;
    
    [Header("Attack Foundations")] 
    public LayerMask layerMask;
    private RaycastHit hit;
    
    [Header("Attack Values")] 
    internal int damageAmount = 0;
    public readonly float range = 10f;
    private readonly float aoeRadius = 5f;
    public bool enemyKilled;
    
    [Header("Cooldowns")]
    private float cooldown = 3;
    private float cooldownTime;
    
    // Start is called before the first frame update
    private void Awake()
    {
        layerMask = LayerMask.GetMask("Enemies");
    }
    
    // Implement Attack from IAttackHandler
    public void Attack(GameObject targetHit)
    {
        if (targetHit != null)
        {
            ApplyAoeDamage(targetHit.transform.position);
        }
    }
    
    public void ApplyAoeDamage(Vector3 aoeCenter)
    {
        // Find all colliders within the aoeRadius around the hit point
        Collider[] hitColliders = Physics.OverlapSphere(aoeCenter, aoeRadius, layerMask);
        
        HashSet<GameObject> uniqueEnemies = new HashSet<GameObject>();
        // Loop through each object in the radius
        foreach (var hitCollider in hitColliders)
        {
            uniqueEnemies.Add(hitCollider.gameObject);
        }

        if (cooldownTime <= 0)
        {
            cooldownTime = cooldown;
            // Loop through each unique enemy and apply damage
            foreach (GameObject targetHit in uniqueEnemies)
            {
                UnitAoeAttack(targetHit);
                DeathCheck(targetHit);
            }
        }
        else
        {
            cooldownTime -= Time.deltaTime;
        }
        //rays for visualising and debugging 
        Debug.DrawRay(aoeCenter, Vector3.up * 2f, Color.blue, 2.0f); // Draw the AoE center
        Debug.DrawLine(aoeCenter, aoeCenter + Vector3.up * 2f, Color.yellow, 2.0f);
    }
    
    public void UnitAoeAttack(GameObject targetHit)
    {
        if (targetHit != null)
        {
            IEnemyStats targetStats = targetHit.GetComponent<IEnemyStats>();
            cooldownTime = cooldown;
            targetStats?.ApplyDamage(damageAmount);
        }
    }
    
    // Perform a death check and set enemyKilled to true if an enemy is killed
    public void DeathCheck(GameObject targethit)
    {
        IEnemyStats targetHealth = targethit?.GetComponent<IEnemyStats>();  
        
        if (targetHealth != null && targetHealth.IsDead())  
        {
            ObjectPoolManager.ReturnObjectToPool(targethit);
            enemyKilled = true;  // Set enemyKilled to true when an enemy is killed
        }
    }
    
    // check if the enemy has been killed
    public bool IsEnemyKilled()
    {
        return enemyKilled;
    }

    // reset the enemyKilled status
    public void ResetEnemyKilledStatus()
    {
        enemyKilled = false;
    }
}
