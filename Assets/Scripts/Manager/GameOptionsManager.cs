using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameOptionsManager : MonoBehaviour
{
    public SceneField mainMenuScene;

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

    public void OpenSettingsMenu()
    {
        Time.timeScale = 0;
        UIManager.Instance.optionsMenu.SetActive(false);
        UIManager.Instance.settingsMenu.SetActive(true);
        UIManager.Instance.menuActivated = true;
    }

    public void SaveAndLoadMainMenu()
    {
        Time.timeScale = 1;
        SceneChangerManager.Instance.ChangeSceneToMainMenu(mainMenuScene);
    }

    public void SaveAndQuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
