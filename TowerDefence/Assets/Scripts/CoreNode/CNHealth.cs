using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CNHealth : MonoBehaviour
{
    [SerializeField] private GameObject coreNode;
    private readonly int maxHealth = 10;
    public int currentHealth = 10;
    //private readonly int amount = 1;

    private GameOver gameOver;
    
    [Header("UI")] 
    public TextMeshProUGUI currentHealthAmount;

    private void Start()
    {
        gameOver = FindObjectOfType<GameOver>();
    }

    private void Update()
    {
        currentHealthAmount.text = "Core Node Health: "+ currentHealth + "/" +  maxHealth;
        
        if (currentHealth <= 0)
        {
           gameOver.RoundFinished();
        }
    }

    public void HealthHandler(int amount)
    {
        currentHealth -= amount;
        // Debug.Log("Core Node Health: "+ currentHealth + "/" +  maxHealth);
    }
    
}
