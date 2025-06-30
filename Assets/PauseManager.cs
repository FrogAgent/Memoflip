using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;        // Assign Pause Menu Panel in Inspector
    public GameObject darkOverlay;      // Assign dark background overlay
    public GameObject scoreLabelGO;     // Assign Score Text GameObject
    public GameObject coinImageGO;      // Assign Coin Image GameObject
    public GameObject pointsTextGO;     // Assign Points Text GameObject
    public GameObject timeLimitTextGO;  // Assign TimeLimit Text GameObject

    private bool isPaused = false;

    void Start()
    {
        pauseMenu.SetActive(false);
        darkOverlay.SetActive(false);

        // Get difficulty from PlayerPrefs
        string difficulty = PlayerPrefs.GetString("SelectedDifficulty", "EASY");

        // Show or hide score UI depending on difficulty
        if (difficulty != "EASY")
        {
            SetScoreUIVisible(true);
        }
        else
        {
            SetScoreUIVisible(false);
        }
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
        SetScoreUIVisible(false); // Always hide score UI when paused
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        darkOverlay.SetActive(false);

        string difficulty = PlayerPrefs.GetString("SelectedDifficulty", "EASY");

        if (difficulty != "EASY")
        {
            SetScoreUIVisible(true);
        }

        Time.timeScale = 1f;
    }

    private void SetScoreUIVisible(bool isVisible)
    {
        if (scoreLabelGO != null) scoreLabelGO.SetActive(isVisible);
        if (coinImageGO != null) coinImageGO.SetActive(isVisible);
        if (pointsTextGO != null) pointsTextGO.SetActive(isVisible);
        if (timeLimitTextGO != null) timeLimitTextGO.SetActive(isVisible);
    }

    public void OnRestartButtonClick()
    {
        string sceneName = GameSettings.SelectedCategory + "_" + GameSettings.SelectedCardCount + "_CardsGameScene";
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public void OnBackToSettingsButtonClick()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("OptionsScene");
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
