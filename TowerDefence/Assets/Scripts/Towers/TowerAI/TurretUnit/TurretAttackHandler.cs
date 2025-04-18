using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAttackHandler : MonoBehaviour, IAttackHandler, IRotatable
{
    [Header("Audio")]
    public AudioSource src;
    public AudioClip audioClip;
    
    [Header("Unit Values")] 
    public Transform shootLocation;
    
    [Header("Attack Foundations")] 
    public LayerMask layerMask;
    private RaycastHit hit;
    private TurretStats turretStats;
    
    [Header("Attack Values")] 
    public readonly float range = 25f;
    private bool enemyKilled;
    
    [Header("Cooldowns")] 
    private readonly float cooldown = 4f;
    private float cooldownTime;
    
    // Start is called before the first frame update
    private void Awake()
    {
        layerMask = LayerMask.GetMask("Enemies");
        turretStats = GetComponent<TurretStats>();
    }

    // Implement Attack from IAttackHandler
    public void Attack(GameObject targetHit)
    {
        if (targetHit != null)
        {
            IEnemyStats targetStats = targetHit.GetComponent<IEnemyStats>();
            if (cooldownTime <= 0)
            {
                src.clip = audioClip;
                src.Play(); 
                cooldownTime = cooldown;
                targetStats?.ApplyDamage(turretStats.damageAmount);
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
