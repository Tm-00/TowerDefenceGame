using Pada1.BBCore.Framework;
using Pada1.BBCore;
using UnityEngine;


[Condition("MyConditions/IsDead")]
[Help("checks hp")]
public class IsDead : ConditionBase
{
    [InParam("BossStats")] 
    public BossStats bossStats;

    public override bool Check()
    {
        if (bossStats.currentHealth > 0)
        {
            return false;
        }
        return true;
    }
}
