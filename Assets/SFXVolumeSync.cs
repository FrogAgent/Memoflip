using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXVolumeSync : MonoBehaviour
{
    private AudioSource sfxSource;

    void Start()
    {
        sfxSource = GetComponent<AudioSource>();
        sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }

    // Optional: keep volume updated live (e.g., while slider moves)
    void Update()
    {
        sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }
}
