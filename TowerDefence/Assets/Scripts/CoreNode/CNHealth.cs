using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CNHealth : MonoBehaviour
{
    [SerializeField] private GameObject coreNode;
    private readonly int maxHealth = 10;
    public int currentHealth = 10;
    private readonly int amount = 10;


    private void Update()
    {
        if (currentHealth <= 0)
        {
            SceneManager.LoadSceneAsync(0);
        }
    }

    public void HealthHandler()
    {
        currentHealth -= amount;
        Debug.Log("Core Node Health: "+ currentHealth + "/" +  maxHealth);
    }
    
}
