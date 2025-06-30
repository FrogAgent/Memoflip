using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class OptionsManager : MonoBehaviour 
{
    [Header("Animation Settings")]
    [SerializeField] private Animator panelAnimator;
    [SerializeField] private string entranceTrigger = "PanelEnter";
    [SerializeField] private string exitTrigger = "PanelExit";
    [SerializeField] private float exitAnimationDuration = 0.5f;

    private bool isTransitioning = false;

    private void Start()
    {
        // Play entrance animation automatically when scene opens
        PlayEntranceAnimation();
    }

    public void OnBackButtonClick()
    {
        if (!isTransitioning)
        {
            StartCoroutine(PlayExitAnimationAndReturn());
        }
    }

    private IEnumerator PlayExitAnimationAndReturn()
    {
        isTransitioning = true;
        
        // Play exit animation
        if (panelAnimator != null)
        {
            panelAnimator.SetTrigger(exitTrigger);
            yield return new WaitForSeconds(exitAnimationDuration);
        }
        
        // Return to MainMenuScene
        SceneManager.LoadScene("MainMenuScene");
    }

    private void PlayEntranceAnimation()
    {
        if (panelAnimator != null && !isTransitioning)
        {
            panelAnimator.SetTrigger(entranceTrigger);
        }
    }
}