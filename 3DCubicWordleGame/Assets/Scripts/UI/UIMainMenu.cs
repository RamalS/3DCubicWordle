using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] Button ButtonSingleplayer, ButtonExitGame, ButtonSettings, ButtonAbout;

    // Start is called before the first frame update
    void Start()
    {
        ButtonSingleplayer.onClick.AddListener(StartSinglePlayer);
        ButtonSettings.onClick.AddListener(Settings);
        ButtonAbout.onClick.AddListener(About);
        ButtonExitGame.onClick.AddListener(ExitGame);
    }

    private void StartSinglePlayer()
    {
        ScenesManager.Instance.LoadScene(Scene.Singleplayer);
    }

    private void Settings()
    {
        ScenesManager.Instance.LoadScene(Scene.Settings);
    }

    private void About()
    {
        ScenesManager.Instance.LoadScene(Scene.About);
    }

    private void ExitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        //Application.Quit()
    }
}
