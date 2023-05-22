using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UISettings : MonoBehaviour
{
    [SerializeField] Slider _sliderMusic, _sliderSound;
    [SerializeField] Button _buttonBack;

    // Start is called before the first frame update
    void Start()
    {
        _buttonBack.onClick.AddListener(BackToMainMenu);
    }

    private void BackToMainMenu()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.MainMenu);
    }

}
