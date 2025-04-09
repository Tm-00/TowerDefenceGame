using TMPro;
using UnityEngine;


public class ScoreManager : MonoBehaviour
{
    [Header("Scores")] 
    public float score;
    public float highScore;
    
    [Header("UI")] 
    public TextMeshProUGUI scoreDisplay;
    public TextMeshProUGUI highScoreDisplay;
    
    void Awake()
    {
        score = 0;
        highScore = PlayerPrefs.GetFloat("HighScore", 0);
        highScoreDisplay.text = "High-Score: " + highScore;
        scoreDisplay.text = "Score: " + score;
    }
    
    public void AddScore(float amount)
    {
        score += amount;
        scoreDisplay.text = "Score: " + score;

        if (score > PlayerPrefs.GetFloat("HighScore"))
        {
            PlayerPrefs.SetFloat("HighScore", score);
        }
    }
    
    public void RemoveScore(float amount)
    {
        score -= amount;
        scoreDisplay.text = "Score: " + score;
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("HighScore", score);
    }
}
