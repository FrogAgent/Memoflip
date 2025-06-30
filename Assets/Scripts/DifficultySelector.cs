using UnityEngine;
using UnityEngine.UI;

public class DifficultySelector : MonoBehaviour
{
    public Text difficultyText;

    private string[] difficulties = { "EASY", "NORMAL", "HARD" };
    private int currentIndex = 0;

    void Start()
    {
        // Load saved difficulty index, default to 0 (EASY)
        currentIndex = PlayerPrefs.GetInt("SelectedDifficultyIndex", 0);
        UpdateDifficultyText();
    }

    public void OnRightClick()
    {
        currentIndex = (currentIndex + 1) % difficulties.Length;
        UpdateDifficultyText();
    }

    public void OnLeftClick()
    {
        currentIndex = (currentIndex - 1 + difficulties.Length) % difficulties.Length;
        UpdateDifficultyText();
    }

    private void UpdateDifficultyText()
    {
        difficultyText.text = difficulties[currentIndex];
        
        // Save the current selection in PlayerPrefs
        PlayerPrefs.SetInt("SelectedDifficultyIndex", currentIndex);
        PlayerPrefs.SetString("SelectedDifficulty", difficulties[currentIndex]);
        PlayerPrefs.Save();
    }

    // Optional: call this from other scripts to get the selected difficulty
    public string GetCurrentDifficulty()
    {
        return difficulties[currentIndex];
    }
}
