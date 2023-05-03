using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] Button _buttonSingleplayer;

    // Start is called before the first frame update
    void Start()
    {
        _buttonSingleplayer.onClick.AddListener(StartSinglePlayer);
    }

    private void StartSinglePlayer()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.Singleplayer);
    }
}
