using UnityEngine;
using Pada1.BBCore;         
using Pada1.BBCore.Tasks;
using BBUnity.Actions;

[Action("MyActions/HealUnit")]
[Help("use the closest target found in the previous action and heal it")]
public class HealUnit : GOAction
{
    [InParam("shootLocation")]
    private Transform shootLocation;
    
    [InParam("closestAlly")] 
    public Transform closestAlly;
    
    private GameObject targetToShoot;
    
    private RaycastHit hit;
    private LayerMask allyLayerMask;
    
    // private IRotatable rotatable;
    // private IAttackHandler attackHandler;

    private BossAttackHandler bossAttackHandler;
    private BossStats bossStats;
    
    private bool enemyKilled;
    private float range;
    
    public override void OnStart()
    {
        
        // attackHandler = gameObject.GetComponent<IAttackHandler>();
        // rotatable = gameObject.GetComponent<IRotatable>();
        bossAttackHandler = gameObject.GetComponent<BossAttackHandler>();
        bossStats = gameObject.GetComponent<BossStats>();

        allyLayerMask = bossAttackHandler.allyLayerMask;
        range = bossAttackHandler.healRange;
        
        if (shootLocation == null)
        {
            shootLocation = gameObject.transform.Find("BossShootLocation");
            if (shootLocation == null)
            {
                Debug.LogWarning("shoot point not specified. ShootOnce will not work " +
                                 "for " + gameObject.name);
            }
        }
        base.OnStart();
    }
    
    public override TaskStatus OnUpdate()
    {     
        if (shootLocation == null)
        {
            Debug.LogWarning("shoot point not specified. ShootOnce will not work " +
                             "for " + gameObject.name);
            return TaskStatus.FAILED;
        }
        
        //rotatable.RotateToTarget(gameObject, closestTarget, rotationSpeed);
        
        if (shootLocation != null)
        {
            if (Physics.Raycast(shootLocation.position, gameObject.transform.TransformDirection(Vector3.forward), out hit, range, allyLayerMask))
            {
                // confirm a target was hit then store it as a gameobject 
                var targetHit = hit.collider.gameObject;
                // check that the target hit was the cloest target then perform attack methods
                if (targetHit != null && targetHit.transform == closestAlly)
                {
                    bossAttackHandler.Heal(targetHit);
                }
            }
        }
        return TaskStatus.COMPLETED;
    } 
}
