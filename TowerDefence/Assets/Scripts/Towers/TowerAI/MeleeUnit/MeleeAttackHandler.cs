using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackHandler : MonoBehaviour
{
    [Header("Unit Values")] 
    public Transform shootLocation;
    
    [Header("Target Values")] 
    // private Transform closestTarget;

    [Header("Attack Foundations")] 
    public LayerMask layerMask;
    private RaycastHit hit;
    
    [Header("Attack Values")]
    private readonly int damageAmount = 35;
    public readonly float range = 10f;
    private readonly float aoeRadius = 5f;
    public bool enemyKilled;
    
    [Header("Cooldowns")]
    private float cooldown = 3;
    private float cooldownTime;
    
    // Start is called before the first frame update
    void Start()
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
    
    public void UnitAoeAttack(GameObject targethit)
    {
        if (targethit != null)
        {
            FlightStats flightStats = targethit.GetComponent<FlightStats>();
            RobotStats robotStats = targethit.GetComponent<RobotStats>();
            ScoutStats scoutStats = targethit.GetComponent<ScoutStats>();
            RifleStats rifleStats = targethit.GetComponent<RifleStats>();
            
            cooldownTime = cooldown;
            flightStats?.EnemyTakeDamage(damageAmount);
            robotStats?.EnemyTakeDamage(damageAmount);
            scoutStats?.EnemyTakeDamage(damageAmount);
            rifleStats?.EnemyTakeDamage(damageAmount);
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
        //rays for seeing the direction the object is facing and shooting towards
        Debug.DrawRay(shootLocation.position, targetDirection * 10f, Color.red);  // Red line pointing towards target
        Debug.DrawRay(shootLocation.transform.position, go.transform.forward * 10f, Color.green); // Green line showing current forward direction
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
}
