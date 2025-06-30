using UnityEngine;
using UnityEngine.SceneManagement;

public class CategoryManager : MonoBehaviour
{
    public void OnCategoryButtonClick(string category)
    {
        // Set the selected category
        GameSettings.SelectedCategory = category;
        
        // Load the next scene directly
        SceneManager.LoadScene("CardCountScene");
    }
}