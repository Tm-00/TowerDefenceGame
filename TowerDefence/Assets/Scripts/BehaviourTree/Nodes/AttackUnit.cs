using UnityEngine;
using Pada1.BBCore;         
using Pada1.BBCore.Tasks;
using BBUnity.Actions;

[Action("MyActions/AttackUnit")]
[Help("use the closest target found in the previous action and shoot at it")]
public class AttackUnit : GOAction
{
    [InParam("shootLocation")]
    private Transform shootLocation;
    
    [InParam("closestTarget")] 
    public Transform closestTarget;
    
    private GameObject targetToShoot;
    
    private RaycastHit hit;
    private LayerMask unitLayerMask;
    
    private IRotatable rotatable;
    private IAttackHandler attackHandler;

    private BossAttackHandler bossAttackHandler;
    private BossStats bossStats;
    
    private bool enemyKilled;
    private float range;
    
    private readonly float rotationSpeed = 1.0f;
    
    
    public override void OnStart()
    {
        
        attackHandler = gameObject.GetComponent<IAttackHandler>();
        rotatable = gameObject.GetComponent<IRotatable>();
        bossAttackHandler = gameObject.GetComponent<BossAttackHandler>();
        bossStats = gameObject.GetComponent<BossStats>();

        unitLayerMask = bossAttackHandler.unitLayerMask;
        range = bossAttackHandler.range;
        
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
            if (Physics.Raycast(shootLocation.position, gameObject.transform.TransformDirection(Vector3.forward), out hit, range, unitLayerMask))
            {
                // confirm a target was hit then store it as a gameobject 
                var targetHit = hit.collider.gameObject;
                // check that the target hit was the cloest target then perform attack methods
                if (targetHit != null && targetHit.transform == closestTarget)
                {
                    bossAttackHandler.Attack(targetHit);
                }
            }
        }
        return TaskStatus.COMPLETED;
    } 
}

