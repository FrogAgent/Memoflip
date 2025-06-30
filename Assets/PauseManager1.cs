using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager1 : MonoBehaviour
{
    public GameObject pauseMenu;      // Assign the Pause Menu Panel in Inspector
    public GameObject darkOverlay;    // Assign a dark background overlay
    private bool isPaused = false;

    void Start()
    {
        pauseMenu.SetActive(false);
        darkOverlay.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        darkOverlay.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        darkOverlay.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnRestartButtonClick()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GuessInfoScene");
    }

    public void OnBackToMainMenuButtonClick()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }

    public void OnSelectGameModeButtonClick()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameSelectScene");
    }

    public void OnBackToSelectNumberOfCardsButtonClick()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("CardCountScene");
    }

    public void OnBackToSelectCategoryButtonClick()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("CategoryScene");
    }
}
