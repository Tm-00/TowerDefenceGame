using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlightAttackHandler : MonoBehaviour
{
    [Header("Flight Values")] 
    private NavMeshAgent agent;
    public Transform shootLocation;
    
    [Header("Target Values")] 
    private Transform closestTarget;

    [Header("Attack Foundations")] 
    public LayerMask layerMask;
    private RaycastHit hit;
    
    [Header("Attack Values")] public int damageAmount = 50;
    public readonly float range = 100f;
    private bool enemyKilled;
    
    [Header("Cooldowns")]
    private readonly float cooldown = 5f;
    private float cooldownTime;
    
    
    // Start is called before the first frame update
    private void Awake()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        layerMask = LayerMask.GetMask("Towers");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void UnitDeathCheck(GameObject targethit)
    {
        LaserStats laserStats = targethit?.GetComponent<LaserStats>();
        if (laserStats != null && laserStats.UnitDeath())
        {
            ObjectPoolManager.ReturnObjectToPool(targethit);
            enemyKilled = true;
        }   
        
        TurretStats turretStats  = targethit?.GetComponent<TurretStats>();
        if (turretStats != null && turretStats.UnitDeath())
        {
            ObjectPoolManager.ReturnObjectToPool(targethit);
            enemyKilled = true;
        }  
        
        MissileStats missileStats = targethit?.GetComponent<MissileStats>();
        if (missileStats != null && missileStats.UnitDeath())
        {
            ObjectPoolManager.ReturnObjectToPool(targethit);
            enemyKilled = true;
        }   
        
        MeleeStats meleeStats = targethit?.GetComponent<MeleeStats>();
        if (meleeStats != null && meleeStats.UnitDeath())
        {
            ObjectPoolManager.ReturnObjectToPool(targethit);
            enemyKilled = true;
        } 
        
        BuffStats buffStats = targethit?.GetComponent<BuffStats>();
        if (buffStats != null && buffStats.UnitDeath())
        {
            ObjectPoolManager.ReturnObjectToPool(targethit);
            enemyKilled = true;
        }
        
        HealerStats healerStats = targethit?.GetComponent<HealerStats>();
        if (healerStats != null && healerStats.UnitDeath())
        {
            ObjectPoolManager.ReturnObjectToPool(targethit);
            enemyKilled = true;
        }
        
        LaneStats laneStats = targethit?.GetComponent<LaneStats>();
        if (laneStats != null && laneStats.UnitDeath())
        {
            ObjectPoolManager.ReturnObjectToPool(targethit);
            enemyKilled = true;
        }
    }

    public void EnemyAttack(GameObject targetHit)
    {
        if (targetHit != null)
        {
            LaserStats laserStats = targetHit?.GetComponent<LaserStats>();
            TurretStats turretStats  = targetHit?.GetComponent<TurretStats>();
            MissileStats missileStats = targetHit?.GetComponent<MissileStats>();
            MeleeStats meleeStats = targetHit?.GetComponent<MeleeStats>();
            BuffStats buffStats = targetHit?.GetComponent<BuffStats>();
            HealerStats healerStats = targetHit?.GetComponent<HealerStats>();
            LaneStats laneStats = targetHit?.GetComponent<LaneStats>();

            if (cooldownTime <= 0)
            {
                cooldownTime = cooldown;
                laserStats?.UnitTakeDamage(damageAmount);
                turretStats?.UnitTakeDamage(damageAmount);
                missileStats?.UnitTakeDamage(damageAmount);
                meleeStats?.UnitTakeDamage(damageAmount);
                buffStats?.UnitTakeDamage(damageAmount);
                healerStats?.UnitTakeDamage(damageAmount);
                laneStats?.UnitTakeDamage(damageAmount);
            }
            else
            {
                cooldownTime -= Time.deltaTime;
            }
            UnitDeathCheck(targetHit);
        }
    }
    
    public void RotateUnitToTarget(GameObject go, Transform ct,float rotationSpeed)
    {
        Vector3 targetDirection = new Vector3(closestTarget.position.x - go.transform.position.x, 0,
            closestTarget.position.z - go.transform.position.z).normalized;
        float singlestep = rotationSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(go.transform.forward, targetDirection, singlestep, 0.0f);
        go.transform.localRotation = Quaternion.LookRotation(newDirection);
        DrawRay(targetDirection, go);
    }
    
    private void DrawRay(Vector3 targetDirection, GameObject go)
    {
        Debug.DrawRay(shootLocation.position, targetDirection * 10f, Color.red);  // Red line pointing towards target
        Debug.DrawRay(shootLocation.transform.position, go.transform.forward * 10f, Color.green); // Green line showing current forward direction
    }
}
