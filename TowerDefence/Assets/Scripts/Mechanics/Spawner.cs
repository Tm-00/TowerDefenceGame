using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{

    [SerializeField] private GameObject common1, common2, elite1, elite2, boss;
    [SerializeField] private int amount1, amount2, amount3, amount4, amount5;
    public int numberOfWaves = 5;
    private List<GameObject> enemyTypes  = new List<GameObject>();
    private List<int> amountToSpawn = new List<int>();
    public static GameObject Enemy;
    private int totalEnemies = 0;
    private float timeTillNextWave = 30;
    private GameOver gameOver;
    private CNHealth cnHealth;
    
    
    // Start is called before the first frame update
    void Start()
    {
        populateList();
        StartCoroutine(SpawnEnemiesCoroutine());
        gameOver = FindObjectOfType<GameOver>();
        cnHealth = FindObjectOfType<CNHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Spawns enemies based on the amount defined in amountToSpawn
    IEnumerator SpawnEnemiesCoroutine()
    {
        for (int waveCount = 0; waveCount < numberOfWaves; waveCount++)
        {
            // Loop through each enemy type in the wave list
            for (int i = 0; i < enemyTypes.Count; i++)
            {
                // Get the spawn amount for the current enemy type
                int enemySpawnAmount = amountToSpawn[i];

                // Spawn the current enemy enemySpawnAmount times
                for (int j = 0; j < enemySpawnAmount; j++)
                {
                    ObjectPoolManager.SpawnObject(enemyTypes[i], transform.position, Quaternion.identity, ObjectPoolManager.PoolType.enemyUnits);
                    Enemy = enemyTypes[i];
                    totalEnemies++;
                    UnitTracker.currentEnemiesSpawned = totalEnemies;
                    yield return new WaitForSeconds(4f);
                }
            }
            
            // check if to spawn enemies based on every 5 waves
            if ((waveCount + 1) % 5 == 0)
            {
                for (int k = 0; k < amount5; k++) 
                {
                    ObjectPoolManager.SpawnObject(boss, transform.position, Quaternion.identity, ObjectPoolManager.PoolType.enemyUnits);
                    Enemy = boss;
                    totalEnemies++;
                    UnitTracker.currentEnemiesSpawned = totalEnemies;
                    yield return new WaitForSeconds(2f);
                }
            }
            
            yield return new WaitForSeconds(timeTillNextWave);
            timeTillNextWave = Mathf.Clamp(timeTillNextWave -= 5, 10, 30);
        }

        if (cnHealth.currentHealth > 0)
        {
            gameOver.LevelComplete();
        }
    }

    

    // takes the values from the inspector and assigns it to the local variables
    void populateList()
    {
        enemyTypes.Add(common1);
        enemyTypes.Add(common2);
        enemyTypes.Add(elite1);
        enemyTypes.Add(elite2);
        //enemyTypes.Add(boss);
        
        amountToSpawn.Add(amount1);
        amountToSpawn.Add(amount2);
        amountToSpawn.Add(amount3);
        amountToSpawn.Add(amount4);
        //amountToSpawn.Add(amount5);
    }
}