using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitTracker : MonoBehaviour 
{
    
    public readonly List<GameObject> UnitTargets = new List<GameObject>();
    public readonly List<GameObject> EnemyTargets = new List<GameObject>();
    private GameObject coreNode;

    private int KnownUnitSpawns;
    private int knownEnemySpawns;
    public static int currentUnitsSpawned;
    public static int currentEnemiesSpawned;
    private List<GameObject> wallUnitList;
    private List<GameObject> floorUnitList;    
    private List<GameObject> enemyList;
    private GameObject[] wallUnitArray;
    private GameObject[] floorUnitArray;
    private GameObject[] enemyUnitArray;
    private GameObject gos;

    private void Awake()
    {
        wallUnitList = new List<GameObject>();
        floorUnitList = new List<GameObject>();
        enemyList = new List<GameObject>();
        coreNode = GameObject.Find("CoreNode");
    }


    // Start is called before the first frame update
    void Start()
    {
        //coreNodePosition = coreNode.transform;
        UnitTargets.Insert(0, coreNode);
     
    }

    // Update is called once per frame
    void Update()
    {
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
    
    public GameObject FindClosestWallUnit(GameObject nav)
    {
        wallUnitList.Clear();
        
        wallUnitArray = GameObject.FindGameObjectsWithTag("WallUnit");
        
        foreach (var t in wallUnitArray)
        {
            if (t != nav)
            {
                wallUnitList.Add(t);
            }
        }
        
        GameObject closestTarget = null;
        float minDistance = Mathf.Infinity;
        Vector3 navPosition = nav.transform.position;
        
        foreach (var go in wallUnitList)
        {
            IUnitStats targetIfPlaced = go.GetComponent<IUnitStats>();
            
            if (targetIfPlaced != null && targetIfPlaced.hasBeenPlaced)
            {
                float currentDistance = (go.transform.position - navPosition).sqrMagnitude;
                if (currentDistance < minDistance)
                {
                    closestTarget = go;
                    minDistance = currentDistance;
                }
            }
        }
        return closestTarget;
    }
    
    public GameObject FindClosestFloorUnit(GameObject nav)
    {
        floorUnitList.Clear();
        floorUnitArray = GameObject.FindGameObjectsWithTag("FloorUnit");
        
        foreach (var t in floorUnitArray)
        {
            if (t != nav)
            {
                floorUnitList.Add(t);
            }
        }
        
        GameObject closestTarget = null;
        float minDistance = Mathf.Infinity;
        Vector3 navPosition = nav.transform.position;
        
        foreach (GameObject go in floorUnitList)
        {
            IUnitStats targetIfPlaced = go.GetComponent<IUnitStats>();
            if (targetIfPlaced != null && targetIfPlaced.hasBeenPlaced)
            {
                float currentDistance = (go.transform.position - navPosition).sqrMagnitude;
                if (currentDistance < minDistance)
                {
                    closestTarget = go;
                    minDistance = currentDistance;
                }
            }
        }
        return closestTarget;
    }

    public Transform FindClosestUnit(GameObject nav)
    {
        var object1 = FindClosestWallUnit(nav)?.transform;
        var object2 = FindClosestFloorUnit(nav)?.transform;
        Vector3 position = nav.transform.position;

        // Validate that object1 is not dead
        if (object1 != null)
        {
            IUnitStats stats1 = object1.GetComponent<IUnitStats>();
            if (stats1 == null || stats1.IsDead())  // Skip if dead or missing IUnitStats
            {
                object1 = null;
            }
        }

        // Validate that object2 is not dead
        if (object2 != null)
        {
            IUnitStats stats2 = object2.GetComponent<IUnitStats>();
            if (stats2 == null || stats2.IsDead())  // Skip if dead or missing IUnitStats
            {
                object2 = null;
            }
        }

        // If both objects are null, return null
        if (object1 == null && object2 == null)
        {
            return null;
        }

        // If only one object is valid, return it
        if (object1 != null && object2 == null)
        {
            return object1;
        }
        if (object2 != null && object1 == null)
        {
            return object2;
        }

        // If both objects are valid, return the closest one
        Vector3 obj1distanceDifference = object1.position - position;
        Vector3 obj2distanceDifference = object2.position - position;

        return (obj1distanceDifference.sqrMagnitude < obj2distanceDifference.sqrMagnitude) ? object1 : object2;
    }
    
    public GameObject FindClosestEnemy(GameObject nav)
    {
        enemyList.Clear();
        enemyUnitArray = GameObject.FindGameObjectsWithTag("Enemy");
        
        foreach (var t in enemyUnitArray)
        {
            if (t != nav)
            {
                enemyList.Add(t);
            }
        }
        
        GameObject closestTarget = null;
        float distance = Mathf.Infinity;
        Vector3 position = nav.transform.position;
        foreach (GameObject go in enemyList)
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
