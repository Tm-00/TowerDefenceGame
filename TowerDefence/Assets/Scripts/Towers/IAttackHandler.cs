using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackHandler
{
    void Attack(GameObject targetHit);
    bool IsEnemyKilled();     
    void ResetEnemyKilledStatus();  
    void DeathCheck(GameObject targetHit); 
}

public interface IHealsAndBuffs
{

}

public interface IRotatable
{
    void RotateToTarget(GameObject go, Transform target, float rotationSpeed)
    {
        Vector3 targetDirection = new Vector3(target.position.x - go.transform.position.x, 0,
            target.position.z - go.transform.position.z).normalized;
        float singleStep = rotationSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(go.transform.forward, targetDirection, singleStep, 0.0f);
        go.transform.localRotation = Quaternion.LookRotation(newDirection);
    }
    
    // private void DrawSTRay(Vector3 targetDirection, GameObject go, Transform shootLocation)
    // {
    //     Debug.DrawRay(shootLocation.position, targetDirection * 10f, Color.red);  // Red line pointing towards target
    //     Debug.DrawRay(shootLocation.transform.position, go.transform.forward * 10f, Color.green); // Green line showing current forward direction
    // }
}