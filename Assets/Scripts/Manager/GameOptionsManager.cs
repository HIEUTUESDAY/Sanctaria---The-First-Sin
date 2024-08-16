using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOptionsManager : MonoBehaviour
{
    public SceneField mainMenuScene;

    public void OpenSoundsSetting()
    {
        Debug.Log("Opening Sounds Settings...");

    }

    public void OpenControlsSetting()
    {
        Debug.Log("Opening Controls Settings...");
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
