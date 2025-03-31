using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class PauseManager : MonoBehaviour
{
    [Header("Resources")] 
    public GameObject pauseMenu;
    public GameObject playerUI;
    public static bool isPaused;
    private CameraSwitcher cameraSwitcher;
    
    // Start is called before the first frame update
    void Start()
    {
        cameraSwitcher = FindObjectOfType<CameraSwitcher>();
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        playerUI.SetActive(false);
        cameraSwitcher.ShowPauseCamera();
        Time.timeScale = 1f;
        isPaused = true;
    }
    
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        playerUI.SetActive(true);
        cameraSwitcher.ShowPlayerCamera();
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
}
