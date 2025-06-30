using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ContinueManager : MonoBehaviour
{
    public Button continueButton; // Assign the Continue button in the Inspector

    void Start()
    {
        continueButton.gameObject.SetActive(false); // Hide the button initially
        continueButton.onClick.AddListener(LoadGameOverScene); // Add click event
    }

    public void ShowContinueButton()
    {
        continueButton.gameObject.SetActive(true); // Show the button when game ends
    }

    public void LoadGameOverScene()
    {
        SceneManager.LoadScene("GameOverScene"); // Load GameOverScene when clicked
    }
}
