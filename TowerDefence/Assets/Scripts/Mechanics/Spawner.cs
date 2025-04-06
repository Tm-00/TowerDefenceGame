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
        StartCoroutine(SpawnEnemiesCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Spawns enemies based on the amount defined in amountToSpawn
    IEnumerator SpawnEnemiesCoroutine()
    {
        // Loop through each enemy type in the wave list
        for (int i = 0; i < wave.Count; i++)
        {
            // Get the spawn amount for the current enemy type
            int enemySpawnAmount = amountToSpawn[i];

            // Spawn the current enemy enemySpawnAmount times
            for (int j = 0; j < enemySpawnAmount; j++)
            {
                ObjectPoolManager.SpawnObject(wave[i], transform.position, Quaternion.identity, ObjectPoolManager.PoolType.enemyUnits);
                Enemy = wave[i];
                totalEnemies++;
                UnitTracker.currentEnemiesSpawned = totalEnemies;
                
                yield return new WaitForSeconds(1f);
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
        wave.Add(boss);
        
        amountToSpawn.Add(amount1);
        amountToSpawn.Add(amount2);
        amountToSpawn.Add(amount3);
        amountToSpawn.Add(amount4);
        amountToSpawn.Add(amount5);
    }
}