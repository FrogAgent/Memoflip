using UnityEngine;
using UnityEngine.UI; // Add this line for Button class
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour 
{
    [Header("Animation Control")]
    [SerializeField] private Animator panelAnimator;
    [SerializeField] private string entranceTrigger = "PanelEnter";
    [SerializeField] private string exitTrigger = "PanelExit";
    [SerializeField] private float animationExitDelay = 1f;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip exitSound;
    private AudioSource audioSource;

    [Header("Scene Names")] 
    [SerializeField] private string gameScene = "GameSelectScene";
    [SerializeField] private string optionsScene = "OptionsScene";

    private void Start()
    {
        // Initialize audio source
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Play entrance animation when scene loads
        if (panelAnimator != null)
        {
            panelAnimator.SetTrigger(entranceTrigger);
        }
    }

    public void OnPlayButton()
    {
        StartCoroutine(LoadSceneAfterAnimation(gameScene));
    }

    public void OnOptionsButton() 
    {
        StartCoroutine(LoadSceneAfterAnimation(optionsScene));
    }

    public void OnExitButton()
    {
        StartCoroutine(QuitAfterAnimation());
    }

    private IEnumerator LoadSceneAfterAnimation(string sceneName)
    {
        // Trigger exit animation
        if (panelAnimator != null)
        {
            panelAnimator.SetTrigger(exitTrigger);
        }
        
        // Wait for animation to complete
        yield return new WaitForSeconds(animationExitDelay);
        
        // Load target scene
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator QuitAfterAnimation()
    {
        // Disable all buttons to prevent multiple clicks
        SetButtonsInteractable(false);

        // Play exit sound if available
        if (exitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(exitSound);
        }

        // Trigger exit animation
        if (panelAnimator != null)
        {
            panelAnimator.SetTrigger(exitTrigger);
        }
        
        // Wait for animation to complete
        yield return new WaitForSeconds(animationExitDelay);
        
        // Quit application
        Application.Quit();
        
        // If running in editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    // Helper method to enable/disable all buttons
    private void SetButtonsInteractable(bool state)
    {
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button button in buttons)
        {
            button.interactable = state;
        }
    }
}