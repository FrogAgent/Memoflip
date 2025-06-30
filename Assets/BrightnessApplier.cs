using UnityEngine;
using UnityEngine.UI;

public class BrightnessApplier : MonoBehaviour
{
    private Image brightnessOverlay;
    private const string BrightnessKey = "Brightness";

    void Start()
    {
        brightnessOverlay = GetComponent<Image>();
        float brightness = PlayerPrefs.GetFloat(BrightnessKey, 1f);
        ApplyBrightness(brightness);
    }

    private void ApplyBrightness(float value)
    {
        if (brightnessOverlay != null)
        {
            Color color = brightnessOverlay.color;
            color.a = 1f - value;  // invert because alpha=1 is black screen
            brightnessOverlay.color = color;
        }
    }
}
