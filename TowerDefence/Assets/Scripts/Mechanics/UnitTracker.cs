using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitTracker : MonoBehaviour
{
    
    public static readonly List<GameObject> UnitTargets = new List<GameObject>();
    public static readonly List<GameObject> EnemyTargets = new List<GameObject>();
    [SerializeField] private GameObject coreNode;

    private int KnownUnitSpawns;
    private int knownEnemySpawns;
    public static int currentUnitsSpawned;
    public static int currentEnemiesSpawned;
    public static GameObject[] wallUnitArray;
    public static GameObject[] floorUnitArray;
    public static GameObject[] enemyArray;
    
   
    
    // Start is called before the first frame update
    void Start()
    {
        //coreNodePosition = coreNode.transform;
        UnitTargets.Insert(0, coreNode);
    }

    // Update is called once per frame
    void Update()
    {
        //TODO fix logic where if the player clicks even if the unit isn't deployed count goes up by one
        if (UnitsSpawned())
        {
            UnitTargets.Add(TowerPlacement.unit);
            //Debug.Log(UnitTargets.Count);
        }
        
        if (EnemiesSpawned())
        {
            EnemyTargets.Add(spawner.Enemy);
            Debug.Log(EnemyTargets.Count);
        }
    }

    private bool UnitsSpawned()
    {
        if (KnownUnitSpawns < currentUnitsSpawned)
        {
            KnownUnitSpawns = currentUnitsSpawned;
            return true;
        }
        return false;
    }
    
    private bool EnemiesSpawned()
    {
        if (knownEnemySpawns < currentEnemiesSpawned)
        {
            knownEnemySpawns = currentEnemiesSpawned;
            return true;
        }
        return false;
    }
    
    public static GameObject FindClosestWallUnit(NavMeshAgent nav)
    {
        wallUnitArray = GameObject.FindGameObjectsWithTag("WallUnit"); 
        GameObject closestTarget = null;
        float distance = Mathf.Infinity;
        Vector3 position = nav.transform.position;
        foreach (GameObject go in wallUnitArray)
        {
            IUnitStats targetIfPlaced = go.GetComponent<IUnitStats>();
            if (targetIfPlaced.hasBeenPlaced)
            {
                Vector3 distanceDifference = go.transform.position - position;
                float currentDistance = distanceDifference.sqrMagnitude;
                if (currentDistance < distance)
                {
                    closestTarget = go;
                    distance = currentDistance;
                }
            }
        }
        return closestTarget;
    }
    
    public static GameObject FindClosestFloorUnit(NavMeshAgent nav)
    {
        floorUnitArray = GameObject.FindGameObjectsWithTag("FloorUnit"); 
        GameObject closestTarget = null;
        float distance = Mathf.Infinity;
        Vector3 position = nav.transform.position;
        foreach (GameObject go in floorUnitArray)
        {
            Vector3 distanceDifference = go.transform.position - position;
            float currentDistance = distanceDifference.sqrMagnitude;
            if (currentDistance < distance)
            {
                closestTarget = go;
                distance = currentDistance;
            }
        }
        return closestTarget;
    }

    public static Transform FindClosestUnit(NavMeshAgent nav)
    {
        var object1 = FindClosestWallUnit(nav)?.transform;
        var object2 = FindClosestFloorUnit(nav)?.transform;
        Vector3 position = nav.transform.position;
        
        if (object1 != null)
        {
            Vector3 obj1distanceDifference = object1.position - position;
            if (object2 != null)
            {
                Vector3 obj2distanceDifference = object2.position - position;

                if (obj1distanceDifference.sqrMagnitude < obj2distanceDifference.sqrMagnitude)
                {
                    return object1;
                }
                return object2;
            }
            return object1;
        }
        return object2;
    }
    
    public static GameObject FindClosestAlly(GameObject nav)
    {
        wallUnitArray = GameObject.FindGameObjectsWithTag("WallUnit"); 
        GameObject closestTarget = null;
        float distance = Mathf.Infinity;
        Vector3 position = nav.transform.position;
        foreach (GameObject go in wallUnitArray)
        {
            Vector3 distanceDifference = go.transform.position - position;
            float currentDistance = distanceDifference.sqrMagnitude;
            if (currentDistance < distance)
            {
                closestTarget = go;
                distance = currentDistance;
            }
        }
        return closestTarget;
    }
    
    public static GameObject FindClosestEnemy(GameObject nav)
    {
        enemyArray = GameObject.FindGameObjectsWithTag("Enemy"); 
        GameObject closestTarget = null;
        float distance = Mathf.Infinity;
        Vector3 position = nav.transform.position;
        foreach (GameObject go in enemyArray)
        {
            Vector3 distanceDifference = go.transform.position - position;
            float currentDistance = distanceDifference.sqrMagnitude;
            if (currentDistance < distance)
            {
                closestTarget = go;
                distance = currentDistance;
            }
        }
        return closestTarget;
    }
    
}
