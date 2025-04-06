using UnityEngine;
using Pada1.BBCore; // Code attributes.
using Pada1.BBCore.Tasks; // TaskStatus.
using BBUnity.Actions;
using Pada1.BBCore.Framework; // Using GOAction - Gives us access to the gameobject.
[Action("MyActions/Death")]
[Help("gracefully kill the unit")]

public class Death : GOAction
{
    public override void OnStart()
    {
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
