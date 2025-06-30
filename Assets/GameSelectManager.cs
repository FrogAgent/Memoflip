using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameSelectManager : MonoBehaviour 
{
    [Header("Animation Settings")]
    [SerializeField] private Animator panelAnimator;
    [SerializeField] private string entranceTrigger = "PanelEnter";
    [SerializeField] private string exitTrigger = "PanelExit";
    [SerializeField] private float exitAnimationDuration = 0.5f;

    private FadeController fadeController;
    private bool isTransitioning = false;

    private void Start()
    {
        fadeController = FindObjectOfType<FadeController>();
        PlayEntranceAnimation();
    }

    public void OnClickFirstButton()
    {
        if (!isTransitioning)
        {
            StartCoroutine(PlayExitAnimationAndLoad("CategoryScene"));
        }
    }

    public void OnClickGuessInfoButton()
    {
        if (!isTransitioning)
        {
            StartCoroutine(PlayExitAnimationAndLoad("GuessInfoScene"));
        }
    }

    private IEnumerator PlayExitAnimationAndLoad(string sceneName)
    {
        isTransitioning = true;
        
        // Play exit animation
        if (panelAnimator != null)
        {
            panelAnimator.SetTrigger(exitTrigger);
            yield return new WaitForSeconds(exitAnimationDuration);
        }
        
        // Load target scene
        if (fadeController != null)
        {
            fadeController.FadeToScene(sceneName);
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    private void PlayEntranceAnimation()
    {
        if (panelAnimator != null && !isTransitioning)
        {
            panelAnimator.SetTrigger(entranceTrigger);
        }
    }
}