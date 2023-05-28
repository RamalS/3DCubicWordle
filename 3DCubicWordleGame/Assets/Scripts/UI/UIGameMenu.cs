using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameMenu : MonoBehaviour
{
    [SerializeField] Button ButtonMainMenu;

    // Start is called before the first frame update
    void Start()
    {
        ButtonMainMenu.onClick.AddListener(LoadMainMenu);
    }

    private void LoadMainMenu()
    {
        ScenesManager.Instance.LoadScene(Scene.MainMenu);
    }
}
