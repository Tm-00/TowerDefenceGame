using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HealerHealHandler : MonoBehaviour, IAttackHandler, IRotatable
{
    [Header("Unit Values")] 
    public Transform shootLocation;

    [Header("Audio")]
    public AudioSource src;
    public AudioClip audioClip;
    
    [Header("Heal Foundations")] 
    public LayerMask layerMask;
    private RaycastHit hit;
    private HealerStats healerStats;
    
    [Header("Heal Values")]
    public readonly float range = 20f;
    private readonly float aoeRadius = 10f;
    public bool unitDied;
    
    [Header("Cooldowns")]
    private float cooldown = 10;
    private float cooldownTime;
    
    [Header("Animator")] 
    private Animator anim;
    AnimatorStateInfo currentStateInfo;
    private int healActionHash = Animator.StringToHash("HealAction");
    
    void Awake()
    {
        layerMask = LayerMask.GetMask("Towers");
        healerStats = GetComponent<HealerStats>();
        anim = GetComponent<Animator>();
    }
    
    // Implement Attack from IAttackHandler
    public void Attack(GameObject targetHit)
    {
        if (targetHit != null)
        {
            //src.clip = audioClip;
           // src.Play(); 
            ApplyAoeHeal(targetHit.transform.position);
        }
    }
    
    private void ApplyAoeHeal(Vector3 aoeCenter)
    {
        // Find all colliders within the aoeRadius around the hit point
        Collider[] hitColliders = Physics.OverlapSphere(aoeCenter, aoeRadius, layerMask);
        
        HashSet<GameObject> uniqueAllies = new HashSet<GameObject>();
        // Loop through each object in the radius
        foreach (var hitCollider in hitColliders)
        {
            uniqueAllies.Add(hitCollider.gameObject);
        }

        if (cooldownTime <= 0)
        {
            cooldownTime = cooldown;
            // Loop through each unique enemy and apply damage
            foreach (GameObject targetHit in uniqueAllies)
            {
                anim.SetTrigger(healActionHash);
                UnitAoeHeal(targetHit);
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
    
        
    private void UnitAoeHeal(GameObject targetHit)
    {
        if (targetHit != null)
        {
            IUnitStats targetStats = targetHit.GetComponent<IUnitStats>();
            cooldownTime = cooldown;
            targetStats?.ApplyHeal(healerStats.healAmount);
        }
    }
    
    // Perform a death check and set enemyKilled to true if an enemy is killed
    public void DeathCheck(GameObject targethit)
    {
        IUnitStats targetHealth = targethit?.GetComponent<IUnitStats>();  
        
        if (targetHealth != null && targetHealth.IsDead())  
        {
            unitDied = true;  // Set enemyKilled to true when an enemy is killed
        }
    }
    
    // check if the enemy has been killed
    public bool IsEnemyKilled()
    {
        return unitDied;
    }

    // reset the enemyKilled status
    public void ResetEnemyKilledStatus()
    {
        unitDied = false;
    }
}
