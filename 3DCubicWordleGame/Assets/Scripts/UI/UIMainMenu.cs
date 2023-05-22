using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] Button _buttonSingleplayer, _buttonExitGame, _buttonSettings, _buttonAbout;

    // Start is called before the first frame update
    void Start()
    {
        _buttonSingleplayer.onClick.AddListener(StartSinglePlayer);
        _buttonSettings.onClick.AddListener(Settings);
        _buttonAbout.onClick.AddListener(About);
        _buttonExitGame.onClick.AddListener(ExitGame);
    }

    private void StartSinglePlayer()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.Singleplayer);
    }

    private void Settings()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.Settings);
    }

    private void About()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.About);
    }

    private void ExitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        //Application.Quit()
    }
}
