using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scene
{
    MainMenu,
    Singleplayer,
    Settings,
    About,
}

public class ScenesManager : MonoBehaviour
{
    // Static Properties
    public static ScenesManager Instance;

    

    void Awake()
    {
        Instance = this;
    }

    public void LoadScene(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(Scene.MainMenu.ToString());
    }

    // LoadSingleplayer, LoadMultiplayer, LoadCredits
}
