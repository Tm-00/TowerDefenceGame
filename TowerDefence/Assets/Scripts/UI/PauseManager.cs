using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject playerUI;
    public float totalCd = 3f;
    private bool isPaused;
    private bool isCountingDown;
    private CameraSwitcher cameraSwitcher;

    void Start()
    {
        cameraSwitcher = FindObjectOfType<CameraSwitcher>();
        pauseMenu.SetActive(false);
        isCountingDown = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!isPaused && !isCountingDown)
            {
                StartCoroutine(PauseCountdown());
            }
            else if (isPaused)
            {
                ResumeGame();
            }
        }
    }

    IEnumerator PauseCountdown()
    {
        isCountingDown = true;

        cameraSwitcher.ShowPauseCamera();
        pauseMenu.SetActive(true);
        playerUI.SetActive(false);
        
        float countdown = totalCd;
        while (countdown > 0)
        {
            countdown -= Time.unscaledDeltaTime;
            yield return null;
        }
        
        Time.timeScale = 0f;
        isPaused = true;
        isCountingDown = false;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        
        pauseMenu.SetActive(false);
        playerUI.SetActive(true);
        cameraSwitcher.ShowPlayerCamera();
    }
}