using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pauseMenu;
    public GameObject darkOverlay;
    public GameObject scoreLabelGO;
    public GameObject coinImageGO;
    public GameObject pointsTextGO;
    public GameObject timeLimitTextGO;
    
    [Header("Card Grid Buttons")]
    public Button enlargeGridButton;
    public Button shrinkGridButton;
    public Button resetGridButton;

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
        SetScoreUIVisible(false);
        
        // Hide grid control buttons when paused
        SetGridButtonsVisible(false);
        
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

        // Show grid control buttons when resuming
        SetGridButtonsVisible(true);
        
        Time.timeScale = 1f;
    }

    private void SetScoreUIVisible(bool isVisible)
    {
        if (scoreLabelGO != null) scoreLabelGO.SetActive(isVisible);
        if (coinImageGO != null) coinImageGO.SetActive(isVisible);
        if (pointsTextGO != null) pointsTextGO.SetActive(isVisible);
        if (timeLimitTextGO != null) timeLimitTextGO.SetActive(isVisible);
    }

    private void SetGridButtonsVisible(bool isVisible)
    {
        if (enlargeGridButton != null) enlargeGridButton.gameObject.SetActive(isVisible);
        if (shrinkGridButton != null) shrinkGridButton.gameObject.SetActive(isVisible);
        if (resetGridButton != null) resetGridButton.gameObject.SetActive(isVisible);
    }

    public bool IsGamePaused()
    {
        return isPaused;
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