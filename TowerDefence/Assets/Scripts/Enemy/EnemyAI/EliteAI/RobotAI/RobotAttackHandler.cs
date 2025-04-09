using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotAttackHandler : MonoBehaviour, IAttackHandler, IRotatable
{
    [Header("Robot Values")] 
    public Transform shootLocation;
    
    [Header("Attack Foundations")] 
    public LayerMask layerMask;
    private RaycastHit hit;
    private NavMeshAgent nav;
    
    [Header("Attack Values")]
    internal int damageAmount = 50;
    public readonly float range = 25f;
    private bool enemyKilled;
    
    [Header("Cooldowns")]
    private readonly float cooldown = 5f;
    private float cooldownTime;

    private float currentSpeed;

    [Header("Animator")] 
    //[SerializeField] private Animator anim;
    private Animator anim;
    AnimatorStateInfo currentStateInfo;
    private int shootTriggerHash = Animator.StringToHash("RobotShoot");
    private int speedHash = Animator.StringToHash("RobotSpeed");

    
    
    private void Awake()
    {
        layerMask = LayerMask.GetMask("Towers");
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        
    }
    

    // Update is called once per frame
    void Update()
    {
        currentSpeed = nav.velocity.magnitude;
    }

    // Implement Attack from IAttackHandler
    public void Attack(GameObject targetHit)
    {
        
        if (targetHit != null)
        {
            IUnitStats targetStats = targetHit.GetComponent<IUnitStats>();
            if (cooldownTime <= 0)
            {
                src.clip = audioClip;
                src.Play(); 
                anim.SetFloat(speedHash, currentSpeed);
                anim.SetTrigger(shootTriggerHash);
                cooldownTime = cooldown;
                targetStats?.ApplyDamage(damageAmount);
            }
            else
            {
               // anim.SetBool(reloadBoolHash, true);
                cooldownTime -= Time.deltaTime;
            }
            DeathCheck(targetHit);
        }
    }
    
    [Header("Audio")]
    public AudioSource src;
    public AudioClip audioClip;
    
    // Perform a death check and set enemyKilled to true if an enemy is killed
    public void DeathCheck(GameObject targethit)
    {
        IUnitStats targetHealth = targethit?.GetComponent<IUnitStats>();  
        
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
