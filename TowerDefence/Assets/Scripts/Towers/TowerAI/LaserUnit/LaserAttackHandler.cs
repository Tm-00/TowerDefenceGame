using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAttackHandler : MonoBehaviour, IAttackHandler, IRotatable
{
    [Header("Audio")]
    public AudioSource src;
    public AudioClip audioClip;
    
    [Header("Unit Values")] 
    public Transform shootLocation;

    [Header("Attack Foundations")]
    public LayerMask layerMask;
    private RaycastHit hit;
    private LaserStats laserStats;

    [Header("Attack Values")] 
    public readonly float range = 25f;
    public bool enemyKilled;

    [Header("Cooldowns")] 
    public float cooldown = 1f;
    private float cooldownTime;
    
    [Header("Animator")] 
    [SerializeField] private Animator anim;
    AnimatorStateInfo currentStateInfo;
    private int shootTriggerHash = Animator.StringToHash("Shoot");

    // Start is called before the first frame update
    private void Awake()
    {
        layerMask = LayerMask.GetMask("Enemies");
        laserStats = GetComponent<LaserStats>();
    }

    private void Update()
    {
        var state = anim.GetCurrentAnimatorStateInfo(0);
      //  Debug.Log("Current animation: " + state.fullPathHash);
        
        int shootStateHash = Animator.StringToHash("Base Layer.Shoot");
      //  Debug.Log("Shoot hash: " + shootStateHash);

      // if (state.fullPathHash == shootStateHash)
      // {
      //     Debug.Log(true);
      // }
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
                anim.SetTrigger(shootTriggerHash);
                cooldownTime = cooldown;
                targetStats?.ApplyDamage(laserStats.damageAmount);
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
    
