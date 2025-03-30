using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyStats
{
    void ApplyDamage(float amount);
    void ApplyHeal(float amount);
    bool IsDead();
    void ApplyBuff(int amount);
    void Die();
}

public interface IUnitStats
{
    bool hasBeenPlaced { set; get; }
    void ApplyDamage(float amount);
    void ApplyHeal(float amount);
    bool IsDead();
    void ApplyBuff(int amount);
    void OnPlacement();
    void Die();
}

public interface IStats
{
    bool CanSpawn();
    void OnSpawn();
}


