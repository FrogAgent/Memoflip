using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BrightnessController : MonoBehaviour
{
    private Slider brightnessSlider;
    private Image brightnessOverlay;

    private const string BrightnessKey = "Brightness";
    private float currentBrightness;

    private static BrightnessController instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        currentBrightness = PlayerPrefs.GetFloat(BrightnessKey, 1f);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        // Nothing needed here, everything is handled in OnSceneLoaded
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find the brightness overlay in the current scene
        brightnessOverlay = FindOverlay();
        ApplyBrightness(currentBrightness);

        // Try to find the brightness slider in the current scene (including pause menu panels)
        brightnessSlider = FindSlider();
        if (brightnessSlider != null)
        {
            brightnessSlider.onValueChanged.RemoveAllListeners();
            brightnessSlider.SetValueWithoutNotify(currentBrightness);
            brightnessSlider.onValueChanged.AddListener(OnBrightnessChanged);
        }
        else
        {
            brightnessSlider = null;
            // Optional: Debug.Log("BrightnessSlider not found in scene: " + scene.name);
        }
    }

    private Image FindOverlay()
    {
        GameObject overlayGO = GameObject.Find("BrightnessOverlay");
        if (overlayGO != null)
        {
            return overlayGO.GetComponent<Image>();
        }
        Debug.LogWarning("BrightnessOverlay not found in scene: " + SceneManager.GetActiveScene().name);
        return null;
    }

    private Slider FindSlider()
    {
        GameObject sliderGO = GameObject.Find("BrightnessSlider");
        if (sliderGO != null)
        {
            return sliderGO.GetComponent<Slider>();
        }
        return null;
    }

    public void OnBrightnessChanged(float value)
    {
        currentBrightness = value;
        PlayerPrefs.SetFloat(BrightnessKey, value);
        ApplyBrightness(value);
    }

    private void ApplyBrightness(float value)
    {
        if (brightnessOverlay != null)
        {
            // Invert the value because alpha = 1 means black/dark overlay
            float alpha = 1f - value;
            Color c = brightnessOverlay.color;
            c.a = alpha;
            brightnessOverlay.color = c;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
