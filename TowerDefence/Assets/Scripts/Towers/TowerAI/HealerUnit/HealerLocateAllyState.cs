using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HealerLocateAllyState : HealerBaseState
{
    private Vector3 closestTarget;
    private float speed = 1.0f;
    private GameObject closestAlly;
    
    public HealerLocateAllyState(GameObject go)
    {
     
    }
    public override void Enter(GameObject go)
    {
        Debug.Log("Turret: LocateEnemyState");
    }

    public override void Update(GameObject go)
    { 
        closestAlly = UnitTracker.FindClosestAlly(go);
        if (closestAlly != null)
        {
            closestTarget = UnitTracker.FindClosestAlly(go).transform.position;
            Vector3 targetDirection = closestTarget - go.transform.position;
            float singlestep = speed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(go.transform.forward, targetDirection, singlestep, 0.0f);
            go.transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }

    public override void Exit(GameObject go)
    {
        
    }

    public override HealerBaseState HandleInput(GameObject go)
    {
        // Move -> Heal
        if (closestAlly != null)
        {
            return new HealerHealState(go);
        }

        return null;
    }
}