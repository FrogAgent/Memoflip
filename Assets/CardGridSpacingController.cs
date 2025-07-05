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

    private Vector2 originalSpacing;
    private Vector3 originalScale;
    private string sceneSpecificSpacingKey;
    private string sceneSpecificScaleKey;

    private void Awake()
    {
        // Store original values
        if (gridLayoutGroup != null)
            originalSpacing = gridLayoutGroup.spacing;

        if (cardGridContainer != null)
            originalScale = cardGridContainer.localScale;

        // Create scene-specific save keys
        string sceneName = SceneManager.GetActiveScene().name;
        sceneSpecificSpacingKey = $"{sceneName}_Spacing";
        sceneSpecificScaleKey = $"{sceneName}_Scale";
    }

    private void Start()
    {
        // Load settings for this specific scene (if they exist)
        LoadSceneSettings();

        // Setup button listeners
        if (enlargeButton != null)
            enlargeButton.onClick.AddListener(EnlargeGrid);

        if (shrinkButton != null)
            shrinkButton.onClick.AddListener(ShrinkGrid);

        if (resetButton != null)
            resetButton.onClick.AddListener(ResetToDefault);
    }

    public void EnlargeGrid()
    {
        if (gridLayoutGroup == null || cardGridContainer == null) return;

        // Modify Y scale only
        float newScale = Mathf.Min(cardGridContainer.localScale.y + scaleStep, maxScale);
        cardGridContainer.localScale = new Vector3(originalScale.x, newScale, originalScale.z);

        // Modify vertical spacing only
        float newSpacing = Mathf.Min(gridLayoutGroup.spacing.y + spacingStep, maxSpacing);
        gridLayoutGroup.spacing = new Vector2(originalSpacing.x, newSpacing);

        SaveCurrentSettings();
    }

    public void ShrinkGrid()
    {
        if (gridLayoutGroup == null || cardGridContainer == null) return;

        // Modify Y scale only
        float newScale = Mathf.Max(cardGridContainer.localScale.y - scaleStep, minScale);
        cardGridContainer.localScale = new Vector3(originalScale.x, newScale, originalScale.z);

        // Modify vertical spacing only
        float newSpacing = Mathf.Max(gridLayoutGroup.spacing.y - spacingStep, minSpacing);
        gridLayoutGroup.spacing = new Vector2(originalSpacing.x, newSpacing);

        SaveCurrentSettings();
    }

    public void ResetToDefault()
    {
        if (gridLayoutGroup != null)
            gridLayoutGroup.spacing = originalSpacing;

        if (cardGridContainer != null)
            cardGridContainer.localScale = originalScale;

        SaveCurrentSettings();
    }

    private void LoadSceneSettings()
    {
        // Only load if saved settings exist for this scene
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
        // Clean up listeners
        if (enlargeButton != null)
            enlargeButton.onClick.RemoveListener(EnlargeGrid);

        if (shrinkButton != null)
            shrinkButton.onClick.RemoveListener(ShrinkGrid);

        if (resetButton != null)
            resetButton.onClick.RemoveListener(ResetToDefault);
    }
}