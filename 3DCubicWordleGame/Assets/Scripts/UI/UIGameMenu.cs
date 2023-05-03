using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameMenu : MonoBehaviour
{
    [SerializeField] Button _buttonMainMenu;

    // Start is called before the first frame update
    void Start()
    {
        _buttonMainMenu.onClick.AddListener(LoadMainMenu);
    }

    private void LoadMainMenu()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.MainMenu);
    }
}
