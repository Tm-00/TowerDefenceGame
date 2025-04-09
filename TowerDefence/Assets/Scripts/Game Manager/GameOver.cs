using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverMenu;
    public GameObject levelCompleteMenu;
    public TextMeshProUGUI score;
    public TextMeshProUGUI hiScore;
    private ScoreManager scoreManager;
    private PauseManager pauseManager;
    
    
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        gameOverMenu.SetActive(false);
        levelCompleteMenu.SetActive(false);
        scoreManager = FindObjectOfType<ScoreManager>();
        pauseManager = FindObjectOfType<PauseManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RoundFinished()
    {
        pauseManager.playerUI.SetActive(false);
        pauseManager.pauseMenu.SetActive(false);
        gameOverMenu.SetActive(true);
        levelCompleteMenu.SetActive(false);
        score.text = "Score: " + scoreManager.score;
        hiScore.text = "Score: " + scoreManager.highScore;
        Time.timeScale = 0;
    }
    
    public void LevelComplete()
    {
        pauseManager.playerUI.SetActive(false);
        pauseManager.pauseMenu.SetActive(false);
        levelCompleteMenu.SetActive(true);
        gameOverMenu.SetActive(false);
        score.text = "Score: " + scoreManager.score;
        hiScore.text = "Score: " + scoreManager.highScore;
        Time.timeScale = 0;
    }

    public void Retry()
    {
        SceneManager.LoadSceneAsync(1);
    }
    
    public void Quit()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
