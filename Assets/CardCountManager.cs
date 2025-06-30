using UnityEngine;
using UnityEngine.SceneManagement;

public class CardCountManager : MonoBehaviour
{
    public void OnCardCountButtonClick(int cardCount)
    {
        string category = GameSettings.SelectedCategory;

        if (IsValidCategoryAndCardCount(category, cardCount))
        {
            string sceneName = category + "_" + cardCount + "_CardsGameScene";
            GameSettings.SelectedCardCount = cardCount.ToString();
            Debug.Log("Loading " + sceneName + "...");
            
            // Load scene directly without animations
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Invalid category or card count selected.");
        }
    }

    public void SetDifficulty(int difficultyLevel)
    {
        PlayerPrefs.SetInt("SelectedDifficulty", difficultyLevel);
        PlayerPrefs.Save();
        Debug.Log("Difficulty set to " + difficultyLevel);
    }

    private bool IsValidCategoryAndCardCount(string category, int cardCount)
    {
        int[] validCardCounts = { 4, 8, 12, 16, 20, 24, 28, 32 };

        if (string.IsNullOrEmpty(category))
        {
            Debug.LogError("Category is not selected.");
            return false;
        }

        foreach (int count in validCardCounts)
        {
            if (cardCount == count)
                return true;
        }

        return false;
    }
}