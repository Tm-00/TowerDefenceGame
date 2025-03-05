using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    private void Start()
    {
        
    }

    // if core node dies preset game over event 
    public void NodeDeath()
    {
        if (CNHealth.CoreNodeDead())
        {
            Debug.Log("game over");
        }
    }
}
