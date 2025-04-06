using UnityEngine;
using Pada1.BBCore; // Code attributes.
using Pada1.BBCore.Tasks; // TaskStatus.
using BBUnity.Actions;
using Pada1.BBCore.Framework; // Using GOAction - Gives us access to the gameobject.

[Action("MyActions/LocateAllies")]
[Help("calls the FindClosestEnemy method from unit tracker and from the list finds the units")]
public class LocateAllies : BasePrimitiveAction
{
    [InParam("Boss")] 
    public GameObject boss;  
    
    [OutParam("T closestAlly")] 
    public Transform closestAlly;
    
    [OutParam("V3 closestAlly")] 
    public Vector3 ca;

    [InParam("UnitTracker")] 
    public UnitTracker unitTracker;

    private GameObject closestAllygo;
    
    public override TaskStatus OnUpdate()
    {
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
        
        closestAllygo = unitTracker.FindClosestEnemy(boss);
        if (closestAlly != null)
        {
            Debug.Log("doesn't equal null");
            closestAlly = closestAlly.transform;
            ca = closestAlly.position;
            return TaskStatus.COMPLETED;
        }
        return TaskStatus.FAILED;
    }
}
