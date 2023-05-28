using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UISettings : MonoBehaviour
{
    [SerializeField] Slider SliderMusic, SliderSound;
    [SerializeField] Button ButtonBack;

    // Start is called before the first frame update
    void Start()
    {
        ButtonBack.onClick.AddListener(BackToMainMenu);
    }

    private void BackToMainMenu()
    {
        ScenesManager.Instance.LoadScene(Scene.MainMenu);
    }

}
