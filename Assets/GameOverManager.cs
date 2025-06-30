using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void OnRestartButtonClick()
    {
        // Restart the game with the same category and card count
        string sceneName = GameSettings.SelectedCategory + "_" + GameSettings.SelectedCardCount + "_CardsGameScene";
        SceneManager.LoadScene(sceneName);
    }

    public void OnBackToMainMenuButtonClick()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
	
	public void OnSelectGameModeButtonClick()
    {
        SceneManager.LoadScene("GameSelectScene");
    }

    public void OnBackToSelectNumberOfCardsButtonClick()
    {
        SceneManager.LoadScene("CardCountScene");
    }

    public void OnBackToSelectCategoryButtonClick()
    {
        SceneManager.LoadScene("CategoryScene");
    }
}
