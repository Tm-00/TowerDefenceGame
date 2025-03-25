using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffHandler : MonoBehaviour
{
    [Header("Unit Values")] 
    public Transform shootLocation;
    
    [Header("Target Values")] 
    // private Transform closestTarget;

    [Header("Attack Foundations")] 
    public LayerMask layerMask;
    private RaycastHit hit;
    
    [Header("Attack Values")]
    private readonly int buffAmount = 15;
    public readonly float range = 50f;
    private readonly float aoeRadius = 5f;
    public bool enemyKilled;
    
    [Header("Cooldowns")]
    private float cooldown = 3;
    private float cooldownTime;
    
    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Towers");
    }

    // Update is called once per frame
    void Update()
    {
        
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
    
    public void UnitAoeBuff(GameObject targethit)
    {
        if (targethit != null)
        {
            TurretStats turretStats = targethit.GetComponent<TurretStats>();
            MeleeStats meleeStats = targethit.GetComponent<MeleeStats>();
            MissileStats missileStats = targethit.GetComponent<MissileStats>();
            LaserStats laserStats = targethit.GetComponent<LaserStats>();
            HealerStats healerStats = targethit.GetComponent<HealerStats>();
            LaneStats laneStats = targethit.GetComponent<LaneStats>();
            //BuffStats buffStats = targethit.GetComponent<BuffStats>();
            
            cooldownTime = cooldown;
            turretStats?.UnitBuffed(buffAmount);
            meleeStats?.UnitBuffed(buffAmount);
            missileStats?.UnitBuffed(buffAmount);
            laserStats?.UnitBuffed(buffAmount);
            healerStats?.UnitBuffed(buffAmount);
            laneStats?.UnitBuffed(buffAmount);
            //buffStats?.UnitBuffed(buffAmount);
        }
    }
    
    public void ApplyAoeBuff(Vector3 aoeCenter)
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
                UnitAoeBuff(targetHit);
                //DeathCheck(targetHit);
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
