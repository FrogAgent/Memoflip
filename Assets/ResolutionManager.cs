using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionManager : MonoBehaviour
{
    [Header("UI References")]
    public Dropdown resolutionDropdown;
    public Dropdown fullscreenDropdown;
    public Button applyButton;

    private Resolution[] availableResolutions;
    private List<Resolution> uniqueResolutions = new List<Resolution>();
    private Resolution selectedResolution;
    private FullScreenMode selectedFullscreenMode = FullScreenMode.ExclusiveFullScreen;

    void Start()
    {
        // Initialize audio/listeners
        if (applyButton != null)
        {
            applyButton.onClick.AddListener(ApplySettings);
        }
        else
        {
            Debug.LogError("Apply button not assigned in ResolutionManager!");
        }

        LoadSettings();
        LoadResolutions();
        LoadFullscreenModes();
    }

    void LoadSettings()
    {
        // Load saved resolution
        if (PlayerPrefs.HasKey("ResolutionWidth") && PlayerPrefs.HasKey("ResolutionHeight"))
        {
            selectedResolution = new Resolution()
            {
                width = PlayerPrefs.GetInt("ResolutionWidth"),
                height = PlayerPrefs.GetInt("ResolutionHeight"),
                refreshRateRatio = new RefreshRate()
                {
                    numerator = (uint)PlayerPrefs.GetInt("RefreshRateNumerator", 60),
                    denominator = (uint)PlayerPrefs.GetInt("RefreshRateDenominator", 1)
                }
            };
        }
        else
        {
            selectedResolution = Screen.currentResolution;
        }

        // Load fullscreen mode
        if (PlayerPrefs.HasKey("FullscreenMode"))
        {
            selectedFullscreenMode = (FullScreenMode)PlayerPrefs.GetInt("FullscreenMode");
        }
		else
        {
            selectedFullscreenMode = FullScreenMode.FullScreenWindow;
        }

        // Apply loaded settings
        ApplyResolution(selectedResolution, selectedFullscreenMode);
    }

    void LoadResolutions()
    {
        if (resolutionDropdown == null)
        {
            Debug.LogError("Resolution dropdown not assigned!");
            return;
        }

        resolutionDropdown.ClearOptions();
        availableResolutions = Screen.resolutions;
        uniqueResolutions.Clear();
        List<string> options = new List<string>();

        // Filter duplicates
        foreach (var res in availableResolutions)
        {
            bool exists = false;
            foreach (var uniqueRes in uniqueResolutions)
            {
                if (res.width == uniqueRes.width && 
                    res.height == uniqueRes.height &&
                    Mathf.Approximately(
                        (float)res.refreshRateRatio.numerator/res.refreshRateRatio.denominator,
                        (float)uniqueRes.refreshRateRatio.numerator/uniqueRes.refreshRateRatio.denominator
                    ))
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                uniqueResolutions.Add(res);
            }
        }

        // Sort by width (descending), then height (descending)
        uniqueResolutions.Sort((a, b) => 
        {
            if (b.width != a.width) return b.width.CompareTo(a.width);
            return b.height.CompareTo(a.height);
        });

        // Populate dropdown
        int currentIndex = 0;
        for (int i = 0; i < uniqueResolutions.Count; i++)
        {
            var res = uniqueResolutions[i];
            float refreshRate = (float)res.refreshRateRatio.numerator / res.refreshRateRatio.denominator;
            options.Add($"{res.width} × {res.height} @ {refreshRate:0.##} Hz");

            // Check if this is the current resolution
            if (res.width == selectedResolution.width &&
                res.height == selectedResolution.height &&
                Mathf.Approximately(
                    (float)res.refreshRateRatio.numerator/res.refreshRateRatio.denominator,
                    (float)selectedResolution.refreshRateRatio.numerator/selectedResolution.refreshRateRatio.denominator
                ))
            {
                currentIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentIndex;
        resolutionDropdown.RefreshShownValue();
        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
    }

    void LoadFullscreenModes()
    {
        if (fullscreenDropdown == null)
        {
            Debug.LogError("Fullscreen dropdown not assigned!");
            return;
        }

        fullscreenDropdown.ClearOptions();
        List<string> options = new List<string>
        {
            "Fullscreen Window",
            "Exclusive Fullscreen",
            "Maximized Window",
            "Windowed"
        };

        fullscreenDropdown.AddOptions(options);
        fullscreenDropdown.value = (int)selectedFullscreenMode;
        fullscreenDropdown.RefreshShownValue();
        fullscreenDropdown.onValueChanged.AddListener(OnFullscreenModeChanged);
    }

    void OnResolutionChanged(int index)
    {
        if (index >= 0 && index < uniqueResolutions.Count)
        {
            selectedResolution = uniqueResolutions[index];
        }
    }

    void OnFullscreenModeChanged(int index)
    {
        if (System.Enum.IsDefined(typeof(FullScreenMode), index))
        {
            selectedFullscreenMode = (FullScreenMode)index;
        }
    }

    void ApplySettings()
    {
        ApplyResolution(selectedResolution, selectedFullscreenMode);

        // Save settings
        PlayerPrefs.SetInt("ResolutionWidth", selectedResolution.width);
        PlayerPrefs.SetInt("ResolutionHeight", selectedResolution.height);
        PlayerPrefs.SetInt("RefreshRateNumerator", (int)selectedResolution.refreshRateRatio.numerator);
        PlayerPrefs.SetInt("RefreshRateDenominator", (int)selectedResolution.refreshRateRatio.denominator);
        PlayerPrefs.SetInt("FullscreenMode", (int)selectedFullscreenMode);
        PlayerPrefs.Save();

        Debug.Log($"Resolution set to: {selectedResolution.width}×{selectedResolution.height} " +
                 $"@ {(float)selectedResolution.refreshRateRatio.numerator/selectedResolution.refreshRateRatio.denominator:0.##}Hz " +
                 $"| Mode: {selectedFullscreenMode}");
    }

    void ApplyResolution(Resolution resolution, FullScreenMode mode)
    {
        Screen.SetResolution(
            resolution.width,
            resolution.height,
            mode,
            resolution.refreshRateRatio
        );
    }
}