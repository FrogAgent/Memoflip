using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivityController : MonoBehaviour
{
    public Slider mouseSensitivitySlider;               // assign in Inspector
    public CustomCursorController customCursorController;  // assign in Inspector

    private const string SensitivityKey = "MouseSensitivity";

    private void Start()
    {
        // Load saved sensitivity or default to 1
        float savedSensitivity = PlayerPrefs.GetFloat(SensitivityKey, 1f);

        // Set slider value without triggering listeners
        mouseSensitivitySlider.SetValueWithoutNotify(savedSensitivity);

        // Apply loaded sensitivity to cursor speed
        UpdateSensitivity(savedSensitivity);

        // Add listener for real-time changes
        mouseSensitivitySlider.onValueChanged.AddListener(UpdateSensitivity);
    }

    private void UpdateSensitivity(float value)
    {
        // Save new sensitivity setting
        PlayerPrefs.SetFloat(SensitivityKey, value);

        // Update cursor speed
        if (customCursorController != null)
        {
            customCursorController.cursorSpeed = value;
        }
    }
}
