using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAttackHandler : MonoBehaviour, IAttackHandler, IRotatable
{
    [Header("Unit Values")] 
    public Transform shootLocation;
    
    [Header("Attack Foundations")] 
    public LayerMask layerMask;
    private RaycastHit hit;
    
    [Header("Attack Values")] 
    internal int damageAmount = 5;
    public readonly float range = 10f;
    private bool enemyKilled;
    
    [Header("Cooldowns")] 
    private readonly float cooldown = 5f;
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
            IEnemyStats targetStats = targetHit.GetComponent<IEnemyStats>();
            if (cooldownTime <= 0)
            {
                cooldownTime = cooldown;
                targetStats?.ApplyDamage(damageAmount);
            }
            else
            {
                cooldownTime -= Time.deltaTime;
            }
            DeathCheck(targetHit);
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
