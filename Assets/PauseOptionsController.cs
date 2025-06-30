using UnityEngine;
using UnityEngine.UI;

public class PauseOptionsController : MonoBehaviour
{
    [Header("Options Panel")]
    public GameObject optionsPanel; // Assign the Options Panel in Inspector

    [Header("Buttons")]
    public Button applyButton; // Assign Apply button from Options Panel
    public Button backButton;  // Assign Back button from Options Panel

    void Start()
    {
        if (optionsPanel != null)
            optionsPanel.SetActive(false); // Hide Options panel at start

        if (applyButton != null)
            applyButton.onClick.AddListener(OnApplyClick);

        if (backButton != null)
            backButton.onClick.AddListener(OnBackClick);
    }

    public void OnSettingsClick()
    {
        // Just show the Options panel without hiding Pause Menu
        if (optionsPanel != null)
            optionsPanel.SetActive(true);
    }

    public void OnApplyClick()
    {
        // Hide Options panel after applying
        if (optionsPanel != null)
            optionsPanel.SetActive(false);
        
        // Add actual apply logic here if needed
    }

    public void OnBackClick()
    {
        // Just hide the Options panel
        if (optionsPanel != null)
            optionsPanel.SetActive(false);
    }
}
