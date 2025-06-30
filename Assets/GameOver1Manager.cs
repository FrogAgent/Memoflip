using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver1Manager : MonoBehaviour
{
    public void OnRestartButtonClick()
    {
        // Reload the GuessInfoScene to restart the game
        SceneManager.LoadScene("GuessInfoScene");
    }

    public void OnSelectGameModeButtonClick()
    {
        // Go back to the game selection screen
        SceneManager.LoadScene("GameSelectScene");
    }

    public void OnMainMenuButtonClick()
    {
        // Return to the main menu
        SceneManager.LoadScene("MainMenuScene");
    }
}
