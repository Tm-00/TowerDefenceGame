using System.Linq;
using UnityEngine;
using Pada1.BBCore; // Code attributes.
using Pada1.BBCore.Tasks; // TaskStatus.
using BBUnity.Actions;
using Pada1.BBCore.Framework; // Using GOAction - Gives us access to the gameobject.

[Action("MyActions/AttackOrHeal")]
[Help("decide whether to look to attack or to look to heal")]
public class AttackOrHeal : BasePrimitiveAction
{

    [InParam("UnitTracker")] 
    public UnitTracker unitTracker;
    
    [InParam("Boss")] 
    public GameObject boss; 

    [OutParam("Heal")] 
    public bool heal;
    
    [OutParam("Attack")] 
    public bool attack;

    private GameObject allies;
    private Transform units;

    public override TaskStatus OnUpdate()
    {
        
        units = unitTracker.FindClosestUnit(boss);
        
        if (units != null)
        {
            attack = true;
        }
        else
        {
            attack = false;
        }

        allies = unitTracker.FindClosestEnemy(boss);
        if (allies != null)
        {
            heal = true;
        }
        else if (allies != null && unitTracker.UnitTargets.Count <2)
        {
            heal = false;
        }
        else
        {
            heal = false;
        }
        
        Debug.Log(attack);
        Debug.Log(heal);
        
        return TaskStatus.COMPLETED;
    }
}
