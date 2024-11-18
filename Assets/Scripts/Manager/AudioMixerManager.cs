using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider soundFXSlider;
    [SerializeField] private Slider musicSlider;

    private void Start()
    {
        LoadInitVolumeSetting();
    }

    private void LoadInitVolumeSetting()
    {
        if (PlayerPrefs.HasKey("masterVolumeSetting"))
        {
            masterSlider.value = PlayerPrefs.GetFloat("masterVolumeSetting");
        }
        if (PlayerPrefs.HasKey("soundFXVolumeSetting"))
        {
            soundFXSlider.value = PlayerPrefs.GetFloat("soundFXVolumeSetting");
        }
        if (PlayerPrefs.HasKey("musicVolumeSetting"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("musicVolumeSetting");
        }

        SetMasterVolume(masterSlider.value);
        SetSoundFXVolume(soundFXSlider.value);
        SetMusicVolume(musicSlider.value);
    }

    public void LoadVolumeSettings(Slider masterSlider, Slider soundFXSlider, Slider musicSlider)
    {
        this.masterSlider = masterSlider;
        this.soundFXSlider = soundFXSlider;
        this.musicSlider = musicSlider;

        if (PlayerPrefs.HasKey("masterVolumeSetting"))
        {
            masterSlider.value = PlayerPrefs.GetFloat("masterVolumeSetting");
        }
        if (PlayerPrefs.HasKey("soundFXVolumeSetting"))
        {
            soundFXSlider.value = PlayerPrefs.GetFloat("soundFXVolumeSetting");
        }
        if (PlayerPrefs.HasKey("musicVolumeSetting"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("musicVolumeSetting");
        }

        SetMasterVolume(masterSlider.value);
        SetSoundFXVolume(soundFXSlider.value);
        SetMusicVolume(musicSlider.value);
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat("masterVolumeSetting", volume);
        SoundFXManager.Instance.PlayChangeSelectionSound();
    }

    public void SetSoundFXVolume(float volume)
    {
        audioMixer.SetFloat("soundFXVolume", Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat("soundFXVolumeSetting", volume);
        SoundFXManager.Instance.PlayChangeSelectionSound();
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat("musicVolumeSetting", volume);
        SoundFXManager.Instance.PlayChangeSelectionSound();
    }
}
