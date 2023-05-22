using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIAbout : MonoBehaviour
{

    [SerializeField] Button _buttonBackAbout;

    // Start is called before the first frame update
    void Start()
    {
        _buttonBackAbout.onClick.AddListener(BackToMainMenuAbout);
    }

    private void BackToMainMenuAbout()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.MainMenu);
    }

}