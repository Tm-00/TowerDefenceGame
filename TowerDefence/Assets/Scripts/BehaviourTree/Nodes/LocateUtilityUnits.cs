
using UnityEngine;
using Pada1.BBCore; // Code attributes.
using Pada1.BBCore.Tasks; // TaskStatus.
using BBUnity.Actions;
using Pada1.BBCore.Framework; // Using GOAction - Gives us access to the gameobject.
[Action("MyActions/LocateUtility")]
[Help("calls the FindClosestUnit method from unit tracker and from the list finds the utility units")]

public class LocateUtilityUnits : BasePrimitiveAction
{

    [InParam("Boss")] 
    public GameObject boss;  
    
    [OutParam("T closestTarget")] 
    public Transform closestTarget;
    
    [OutParam("V3 closestTarget")] 
    public Vector3 ct;

    [InParam("UnitTracker")] 
    public UnitTracker unitTracker;

    private Transform closestAlly;
    
    
    public override TaskStatus OnUpdate()
    {
        //Debug.Log("we here");
        if (unitTracker == null)
        {
            Debug.LogError("UnitTracker is null.");
            return TaskStatus.FAILED;
        }

        if (boss == null)
        {
            Debug.LogError("Boss is null.");
            return TaskStatus.FAILED;
        }
        
        closestAlly = unitTracker.FindClosestUnit(boss);
        if (closestAlly != null)
        {
            closestTarget = closestAlly;
            ct = closestTarget.position;
            return TaskStatus.COMPLETED;
        }
        return TaskStatus.FAILED;
    }
}




 
