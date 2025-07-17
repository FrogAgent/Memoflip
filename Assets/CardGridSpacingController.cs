using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CardGridSpacingController : MonoBehaviour
{
    [Header("Grid References")]
    public RectTransform cardGridContainer;
    public GridLayoutGroup gridLayoutGroup;

    [Header("UI Buttons")]
    public Button enlargeButton;
    public Button shrinkButton;
    public Button resetButton;

    [Header("Adjustment Settings")]
    public float spacingStep = 5f;
    public float maxSpacing = 50f;
    public float minSpacing = 0f;
    public float scaleStep = 0.1f;
    public float maxScale = 2f;
    public float minScale = 0.5f;

    [Header("Aspect Ratio Settings")]
    public float aspectRatio16_9 = 16f/9f;
    public float aspectRatio21_9 = 21f/9f;
    public float aspectRatio16_10 = 16f/10f;
    public float aspectRatioTolerance = 0.05f;

    private Vector2 originalSpacing;
    private Vector3 originalScale;
    private string sceneSpecificSpacingKey;
    private string sceneSpecificScaleKey;
    private int maxEnlargeClicks;
    private int currentEnlargeClicks;
    private bool isAtOriginalState;
    private bool isBelowOriginalState;

    private void Awake()
    {
        if (gridLayoutGroup != null)
            originalSpacing = gridLayoutGroup.spacing;

        if (cardGridContainer != null)
            originalScale = cardGridContainer.localScale;

        string sceneName = SceneManager.GetActiveScene().name;
        sceneSpecificSpacingKey = $"{sceneName}_Spacing";
        sceneSpecificScaleKey = $"{sceneName}_Scale";

        float currentAspect = (float)Screen.width / Screen.height;

        if (Mathf.Abs(currentAspect - aspectRatio16_9) < aspectRatioTolerance)
        {
            maxEnlargeClicks = 1; // 16:9 - 1 click allowed above original
        }
        else if (Mathf.Abs(currentAspect - aspectRatio21_9) < aspectRatioTolerance)
        {
            maxEnlargeClicks = 0; // 21:9 - no clicks allowed above original
        }
        else if (Mathf.Abs(currentAspect - aspectRatio16_10) < aspectRatioTolerance)
        {
            maxEnlargeClicks = 2; // 16:10 - 2 clicks allowed above original
        }
        else
        {
            maxEnlargeClicks = 1; // Default
        }

        currentEnlargeClicks = 0;
        isAtOriginalState = true;
        isBelowOriginalState = false;
    }

    private void Start()
    {
        LoadSceneSettings();
        
        enlargeButton?.onClick.AddListener(EnlargeGrid);
        shrinkButton?.onClick.AddListener(ShrinkGrid);
        resetButton?.onClick.AddListener(ResetToDefault);

        UpdateEnlargeButtonState();
    }

    public void EnlargeGrid()
    {
        if (gridLayoutGroup == null || cardGridContainer == null) return;

        float newScale = Mathf.Min(cardGridContainer.localScale.y + scaleStep, maxScale);
        cardGridContainer.localScale = new Vector3(originalScale.x, newScale, originalScale.z);

        float newSpacing = Mathf.Min(gridLayoutGroup.spacing.y + spacingStep, maxSpacing);
        gridLayoutGroup.spacing = new Vector2(originalSpacing.x, newSpacing);

        currentEnlargeClicks++;

        // Update state flags
        if (newScale < originalScale.y)
        {
            isBelowOriginalState = true;
            isAtOriginalState = false;
        }
        else if (Mathf.Approximately(newScale, originalScale.y))
        {
            isAtOriginalState = true;
            isBelowOriginalState = false;
        }
        else
        {
            isAtOriginalState = false;
            isBelowOriginalState = false;
        }

        SaveCurrentSettings();
        UpdateEnlargeButtonState();
    }

    public void ShrinkGrid()
    {
        if (gridLayoutGroup == null || cardGridContainer == null) return;

        float newScale = Mathf.Max(cardGridContainer.localScale.y - scaleStep, minScale);
        cardGridContainer.localScale = new Vector3(originalScale.x, newScale, originalScale.z);

        float newSpacing = Mathf.Max(gridLayoutGroup.spacing.y - spacingStep, minSpacing);
        gridLayoutGroup.spacing = new Vector2(originalSpacing.x, newSpacing);

        currentEnlargeClicks = Mathf.Max(-1, currentEnlargeClicks - 1);

        // Update state flags
        if (newScale < originalScale.y)
        {
            isBelowOriginalState = true;
            isAtOriginalState = false;
        }
        else if (Mathf.Approximately(newScale, originalScale.y))
        {
            isAtOriginalState = true;
            isBelowOriginalState = false;
        }
        else
        {
            isAtOriginalState = false;
            isBelowOriginalState = false;
        }

        SaveCurrentSettings();
        UpdateEnlargeButtonState();
    }

    public void ResetToDefault()
    {
        if (gridLayoutGroup != null)
            gridLayoutGroup.spacing = originalSpacing;

        if (cardGridContainer != null)
            cardGridContainer.localScale = originalScale;

        currentEnlargeClicks = 0;
        isAtOriginalState = true;
        isBelowOriginalState = false;
        SaveCurrentSettings();
        UpdateEnlargeButtonState();
    }

    private void UpdateEnlargeButtonState()
    {
        if (enlargeButton == null) return;

        if (isBelowOriginalState)
        {
            // Below original: unlimited clicks until reaching original
            enlargeButton.interactable = true;
        }
        else if (isAtOriginalState)
        {
            // At original: allow clicks based on aspect ratio
            enlargeButton.interactable = maxEnlargeClicks > 0;
        }
        else
        {
            // Above original: check remaining allowed clicks
            int remainingClicks = maxEnlargeClicks - currentEnlargeClicks;
            enlargeButton.interactable = remainingClicks > 0;
        }
    }

    private void LoadSceneSettings()
    {
        if (PlayerPrefs.HasKey(sceneSpecificSpacingKey))
        {
            float savedSpacing = PlayerPrefs.GetFloat(sceneSpecificSpacingKey);
            if (gridLayoutGroup != null)
            {
                savedSpacing = Mathf.Clamp(savedSpacing, minSpacing, maxSpacing);
                gridLayoutGroup.spacing = new Vector2(originalSpacing.x, savedSpacing);
            }
        }

        if (PlayerPrefs.HasKey(sceneSpecificScaleKey))
        {
            float savedScale = PlayerPrefs.GetFloat(sceneSpecificScaleKey);
            if (cardGridContainer != null)
            {
                savedScale = Mathf.Clamp(savedScale, minScale, maxScale);
                cardGridContainer.localScale = new Vector3(originalScale.x, savedScale, originalScale.z);
            }
        }

        if (cardGridContainer != null && gridLayoutGroup != null)
        {
            float scaleDifference = cardGridContainer.localScale.y - originalScale.y;
            currentEnlargeClicks = Mathf.RoundToInt(scaleDifference / scaleStep);
            currentEnlargeClicks = Mathf.Max(-1, currentEnlargeClicks);

            isAtOriginalState = Mathf.Approximately(cardGridContainer.localScale.y, originalScale.y);
            isBelowOriginalState = cardGridContainer.localScale.y < originalScale.y;
        }
    }

    private void SaveCurrentSettings()
    {
        if (gridLayoutGroup != null)
            PlayerPrefs.SetFloat(sceneSpecificSpacingKey, gridLayoutGroup.spacing.y);

        if (cardGridContainer != null)
            PlayerPrefs.SetFloat(sceneSpecificScaleKey, cardGridContainer.localScale.y);

        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        enlargeButton?.onClick.RemoveListener(EnlargeGrid);
        shrinkButton?.onClick.RemoveListener(ShrinkGrid);
        resetButton?.onClick.RemoveListener(ResetToDefault);
    }
}