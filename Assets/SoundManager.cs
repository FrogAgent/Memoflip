using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private AudioSource musicSource;

    void Start()
    {
        // Get music source from MusicManager
        if (MusicManager.Instance != null)
        {
            musicSource = MusicManager.Instance.GetComponent<AudioSource>();
        }

        // Load saved values or set defaults
        float savedMusicVol = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float savedSFXVol = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

        musicSlider.value = savedMusicVol;
        sfxSlider.value = savedSFXVol;

        if (musicSource != null)
            musicSource.volume = savedMusicVol;
    }

    public void OnMusicVolumeChange()
    {
        float volume = musicSlider.value;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        if (musicSource != null)
            musicSource.volume = volume;
    }

    public void OnSFXVolumeChange()
    {
        float volume = sfxSlider.value;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}
