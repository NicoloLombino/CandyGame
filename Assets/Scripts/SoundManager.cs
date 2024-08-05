using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    AudioSource audioSource;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip[] PopSounds;
    [SerializeField]
    private AudioClip effectSound;

    [Header("UI")]
    [SerializeField]
    private Slider SliderVolume;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        SliderVolume.value = PlayerPrefs.GetFloat("SoundVolume");
        audioSource.volume = PlayerPrefs.GetFloat("SoundVolume");
    }

    public void PlayRandomPopSound()
    {
        if(audioSource)
        {
            audioSource.PlayOneShot(PopSounds[Random.Range(0, PopSounds.Length)]);
        }
    }

    public void PlayEffectSound()
    {
        audioSource.PlayOneShot(effectSound);
    }

    public void SaveVolumeValue()
    {
        PlayerPrefs.SetFloat("SoundVolume", audioSource.volume);
    }
}
