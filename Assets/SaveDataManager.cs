using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SaveDataManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject confirmPanel;
    public GameObject overlayPanel;
    public Animator panelAnimator; // Assign your panel's Animator

    [Header("Animation Settings")]
    public float exitAnimationTime = 1f; // Match your exit animation length

    [Header("Scene Management")]
    public string mainMenuScene = "MainMenuScene";

    private bool isPanelVisible = false;

    void Start()
    {
        confirmPanel.SetActive(false);
        if (overlayPanel != null)
            overlayPanel.SetActive(false);
    }

    public void OnClearButtonClick()
    {
        if (isPanelVisible) return;

        ShowConfirmationPanel();
    }

    public void OnYesClicked()
    {
        if (!isPanelVisible) return;

        StartCoroutine(HidePanelAndClearData());
    }

    public void OnNoClicked()
    {
        if (!isPanelVisible) return;

        StartCoroutine(HidePanelWithAnimation());
    }

    private void ShowConfirmationPanel()
    {
        confirmPanel.SetActive(true);
        if (overlayPanel != null)
            overlayPanel.SetActive(true);
        
        // Reset triggers and play entrance animation
        panelAnimator.ResetTrigger("HideInfo");
        panelAnimator.SetTrigger("ShowInfo");
        isPanelVisible = true;
    }

    private IEnumerator HidePanelWithAnimation()
    {
        // Play exit animation
        panelAnimator.ResetTrigger("ShowInfo");
        panelAnimator.SetTrigger("HideInfo");
        
        // Wait for animation to complete
        yield return new WaitForSeconds(exitAnimationTime);
        
        // Then disable the panel
        confirmPanel.SetActive(false);
        if (overlayPanel != null)
            overlayPanel.SetActive(false);
        isPanelVisible = false;
    }

    private IEnumerator HidePanelAndClearData()
    {
        // Play exit animation
        panelAnimator.ResetTrigger("ShowInfo");
        panelAnimator.SetTrigger("HideInfo");
        
        // Wait for animation to complete
        yield return new WaitForSeconds(exitAnimationTime);
        
        // Clear all game data
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        
        // Reset cursor to default
        CursorManager.ClearAllCursorData();
        
        // Load main menu
        SceneManager.LoadScene(mainMenuScene);
    }
}