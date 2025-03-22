using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{

    [SerializeField] private GameObject common1, common2, elite1, elite2, boss;
    [SerializeField] private int amount1, amount2, amount3, amount4, amount5;
    private List<GameObject> wave = new List<GameObject>();
    private List<int> amountToSpawn = new List<int>();
    public static GameObject Enemy;
    private int totalEnemies = 0;
    
    
    // Start is called before the first frame update
    void Start()
    {
        populateList();
        Debug.Log(wave[0]);
        SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Spawns enemies based on the amount defined in amountToSpawn
    void SpawnEnemies()
    {
        // loops through and by consequence gets the index value of each object in the list
        for (int i = 0; i < wave.Count; i++)
        {
            //Debug.Log(i);
            // gets the values of the amount to spawn from i 
            int enemySpawnAmount = amountToSpawn[i];
            //Debug.Log(enemySpawnAmount);

            // Spawn the current enemy enemySpawnAmount of times
            for (int j = 0; j < enemySpawnAmount; j++)
            {
                ObjectPoolManager.SpawnObject(wave[i], transform.position, Quaternion.identity, ObjectPoolManager.PoolType.enemyUnits);
                // add here
                Enemy = wave[i];
                totalEnemies++;
                UnitTracker.currentEnemiesSpawned = totalEnemies;
            }
        }
    }
    

    // takes the values from the inspector and assigns it to the local variables
    void populateList()
    {
        wave.Add(common1);
        wave.Add(common2);
        wave.Add(elite1);
        wave.Add(elite2);
        
        amountToSpawn.Add(amount1);
        amountToSpawn.Add(amount2);
        amountToSpawn.Add(amount3);
        amountToSpawn.Add(amount4);
        amountToSpawn.Add(amount5);
    }
}