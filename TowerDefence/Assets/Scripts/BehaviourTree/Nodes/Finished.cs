using UnityEngine;
using Pada1.BBCore; // Code attributes.
using Pada1.BBCore.Tasks; // TaskStatus.
using BBUnity.Actions;
using Pada1.BBCore.Framework; // Using GOAction - Gives us access to the gameobject.

[Action("MyActions/Finished")]
[Help("handles the boss reaching the core node")]
public class Finished : GOAction
{
    [InParam("BossStats")] 
    public BossStats bossStats;

    public override void OnStart()
    {
        bossStats.Finished();
    }
}
