using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    public UnityEvent backToMainMenu;
    private AudioMixerManager audioMixerManager;

    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider soundFXSlider;
    [SerializeField] private Slider musicSlider;

    private void Start()
    {
        audioMixerManager = FindObjectOfType<AudioMixerManager>();
        audioMixerManager.LoadVolumeSettings(masterSlider, soundFXSlider, musicSlider);

        masterSlider.onValueChanged.AddListener(audioMixerManager.SetMasterVolume);
        soundFXSlider.onValueChanged.AddListener(audioMixerManager.SetSoundFXVolume);
        musicSlider.onValueChanged.AddListener(audioMixerManager.SetMusicVolume);
    }

    private void Update()
    {
        BackToMainMenu();
    }

    private void BackToMainMenu()
    {
        if (gameObject.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            backToMainMenu.Invoke();
            SoundFXManager.Instance.PlayChangeTabSound();
        }
    }
}
