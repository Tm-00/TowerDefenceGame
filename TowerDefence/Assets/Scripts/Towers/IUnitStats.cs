using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStats
{
    void ApplyDamage(float amount);
    void ApplyHeal(float amount);
    bool IsDead();
    void ApplyBuff(int amount);
    void Die();
}


