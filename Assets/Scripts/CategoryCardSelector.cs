using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryCardSelector : MonoBehaviour
{
    public Text categoryText;
    public Text cardCountText;

    public List<string> categories = new List<string> { "Animals", "Countries", "Food", "Fruits", "Letters", "Numbers", "Objects", "Shapes", "Vegetables", "Vehicles" };
    public List<string> cardCounts = new List<string> { "4", "8", "12", "16", "20", "24", "28", "32" };

    private int currentCategoryIndex = 0;
    private int currentCardCountIndex = 0;

    public List<GameObject> allPanels; // All possible panels like Animals_4_Cards_Panel etc.

    void Start()
    {
        UpdateTexts();
        ShowRelevantPanel();
    }

    public void OnCategoryLeft()
    {
        currentCategoryIndex = (currentCategoryIndex - 1 + categories.Count) % categories.Count;
        UpdateTexts();
        ShowRelevantPanel();
    }

    public void OnCategoryRight()
    {
        currentCategoryIndex = (currentCategoryIndex + 1) % categories.Count;
        UpdateTexts();
        ShowRelevantPanel();
    }

    public void OnCardCountLeft()
    {
        currentCardCountIndex = (currentCardCountIndex - 1 + cardCounts.Count) % cardCounts.Count;
        UpdateTexts();
        ShowRelevantPanel();
    }

    public void OnCardCountRight()
    {
        currentCardCountIndex = (currentCardCountIndex + 1) % cardCounts.Count;
        UpdateTexts();
        ShowRelevantPanel();
    }

    void UpdateTexts()
    {
        categoryText.text = categories[currentCategoryIndex];
        cardCountText.text = cardCounts[currentCardCountIndex] + " CARDS";
    }

    void ShowRelevantPanel()
    {
        string selectedPanelName = categories[currentCategoryIndex] + "_" + cardCounts[currentCardCountIndex] + "_Cards_Panel";

        foreach (GameObject panel in allPanels)
        {
            panel.SetActive(panel.name == selectedPanelName);
        }
    }
}
