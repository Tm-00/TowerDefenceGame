using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAttackHandler : MonoBehaviour
{
    [Header("Unit Values")] 
    public Transform shootLocation;
    
    [Header("Target Values")] 
   // private Transform closestTarget;

    [Header("Attack Foundations")] 
    public LayerMask layerMask;
    private RaycastHit hit;
    
    [Header("Attack Values")] public int damageAmount = 50;
    public readonly float range = 10f;
    public bool enemyKilled;
    
    [Header("Cooldowns")]
    public float cooldown = 5f;
    private float cooldownTime;
    
    
    // Start is called before the first frame update
    private void Awake()
    {
        layerMask = LayerMask.GetMask("Enemies");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    public void DeathCheck(GameObject targethit)
    {
        FlightStats flightStats = targethit?.GetComponent<FlightStats>();
        if (flightStats != null && flightStats.EnemyDeath())
        {
            ObjectPoolManager.ReturnObjectToPool(targethit);
            enemyKilled = true;
        }
                    
        RobotStats robotStats = targethit?.GetComponent<RobotStats>();
        if (robotStats != null && robotStats.EnemyDeath())
        {
            ObjectPoolManager.ReturnObjectToPool(targethit);
            enemyKilled = true;
        }       
                    
        RifleStats rifleStats = targethit?.GetComponent<RifleStats>();
        if (rifleStats != null && rifleStats.EnemyDeath())
        {
            ObjectPoolManager.ReturnObjectToPool(targethit);
            enemyKilled = true;
        } 
                    
        ScoutStats scoutStats = targethit?.GetComponent<ScoutStats>();
        if (scoutStats != null && scoutStats.EnemyDeath())
        {
            ObjectPoolManager.ReturnObjectToPool(targethit);
            enemyKilled = true;
        }
    }
    

    public void UnitAttack(GameObject targetHit)
    {
        if (targetHit != null)
        {
            FlightStats flightStats = targetHit.GetComponent<FlightStats>();
            RobotStats robotStats = targetHit.GetComponent<RobotStats>();
            ScoutStats scoutStats = targetHit.GetComponent<ScoutStats>();
            RifleStats rifleStats = targetHit.GetComponent<RifleStats>();
         
            if (cooldownTime <= 0)
            {
                cooldownTime = cooldown;
                flightStats?.EnemyTakeDamage(damageAmount);
                robotStats?.EnemyTakeDamage(damageAmount);
                scoutStats?.EnemyTakeDamage(damageAmount);
                rifleStats?.EnemyTakeDamage(damageAmount);
            }
            else
            {
                cooldownTime -= Time.deltaTime;
            }
            DeathCheck(targetHit);
        }
    }
    
    public void RotateUnitToTarget(GameObject go, Transform ct,float rotationSpeed)
    {
        Vector3 targetDirection = new Vector3(ct.position.x - go.transform.position.x, 0,
            ct.position.z - go.transform.position.z).normalized;
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
