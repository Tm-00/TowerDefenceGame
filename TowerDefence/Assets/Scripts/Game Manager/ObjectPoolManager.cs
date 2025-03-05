using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPoolManager : MonoBehaviour
{
    public static List<PooledObjectInfo> ObjectPools = new List<PooledObjectInfo>();

    private GameObject objectPoolEmptyHolder;

    private static GameObject playerUnitsEmpty;
    private static GameObject enemyUnitsEmpty;
    

    public enum PoolType
    {
        playerUnits,
        enemyUnits,
        None
    }

    public static PoolType PoolingType;

    private void Awake()
    {
        SetupEmpties();
    }

    private void SetupEmpties()
    {
        objectPoolEmptyHolder = new GameObject("Pooled Objects");

        playerUnitsEmpty = new GameObject("Player Units");
        playerUnitsEmpty.transform.SetParent(objectPoolEmptyHolder.transform);
        
        enemyUnitsEmpty = new GameObject("Enemy Units");
        enemyUnitsEmpty.transform.SetParent(objectPoolEmptyHolder.transform);
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, PoolType poolType = PoolType.None)
    {
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);
        
        
        // if the pool doesn't exist create it
        if (pool == null)
        {
            pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }

        // check for inactive objects in pool
        GameObject SpawnableObj = pool.InactiveObjects.FirstOrDefault();

        if (SpawnableObj == null)
        {
            // Find the parent of the empty object
            GameObject parentObject = SetParentObject(poolType);
            
            // if there are no inactive objects create a new one
            SpawnableObj = Instantiate(objectToSpawn, spawnPosition, spawnRotation);

            if (parentObject != null)
            {
                SpawnableObj.transform.SetParent(parentObject.transform);
            }
        }
        else
        {
            // if there is and inactive object reactivate it
            SpawnableObj.transform.position = spawnPosition;
            SpawnableObj.transform.rotation = spawnRotation;
            pool.InactiveObjects.Remove(SpawnableObj);
            SpawnableObj.SetActive(true);
        }
        return SpawnableObj;
    }
    
    // overload method, depending on the parameters passed in compiler will choose between either this or the prior method
    public static GameObject SpawnObject(GameObject objectToSpawn,  Transform parentTransform)
    {
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);
        
        
        // if the pool doesn't exist create it
        if (pool == null)
        {
            pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }

        // check for inactive objects in pool
        GameObject SpawnableObj = pool.InactiveObjects.FirstOrDefault();

        if (SpawnableObj == null)
        {
            // if there are no inactive objects create a new one
            SpawnableObj = Instantiate(objectToSpawn, parentTransform);
        }
        else
        {
            // if there is and inactive object reactivate it
            pool.InactiveObjects.Remove(SpawnableObj);
            SpawnableObj.SetActive(true);
        }
        return SpawnableObj;
    }

    public static void ReturnObjectToPool(GameObject obj)
    {
        // removes the clone from the name passed in obj
        string goName = obj.name.Replace("(Clone)", string.Empty);
        
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == goName);

        if (pool == null)
        {
            Debug.LogWarning("Trying to release an object that is not pooled: " + obj.name);
        }
        else
        {
            obj.SetActive(false);
            pool.InactiveObjects.Add(obj);
        }
    }

    private static GameObject SetParentObject(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.playerUnits:
                return playerUnitsEmpty;
            
            case PoolType.enemyUnits:
                return enemyUnitsEmpty;
            
            case PoolType.None:
                return null;
            default:
                return null;
        }
    }
}

public class PooledObjectInfo
{
    public string LookupString;
    public List<GameObject> InactiveObjects = new List<GameObject>();
}