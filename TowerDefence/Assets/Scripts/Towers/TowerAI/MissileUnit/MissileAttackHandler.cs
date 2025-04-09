using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileAttackHandler : MonoBehaviour, IAttackHandler, IRotatable
{
    [Header("Audio")]
    public AudioSource src;
    public AudioClip audioClip;
    
    [Header("Unit Values")] 
    public Transform shootLocation;
    
    [Header("Attack Foundations")] 
    public LayerMask layerMask;
    private RaycastHit hit;
    private MissileStats missileStats;
    
    [Header("Attack Values")] 
    public readonly float range = 25f;
    private readonly float aoeRadius = 10f;
    public bool enemyKilled;
    
    [Header("Cooldowns")]
    private float cooldown = 10;
    private float cooldownTime;
    
    private void Awake()
    {
        layerMask = LayerMask.GetMask("Enemies");
        missileStats = GetComponent<MissileStats>();
    }
    
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
                src.clip = audioClip;
                src.Play(); 
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
    
    private void UnitAoeAttack(GameObject targetHit)
    {
        if (targetHit != null)
        {
            IEnemyStats targetStats = targetHit.GetComponent<IEnemyStats>();
            cooldownTime = cooldown;
            targetStats?.ApplyDamage(missileStats.damageAmount);
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
