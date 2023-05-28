using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIAbout : MonoBehaviour
{

    [SerializeField] Button ButtonBackAbout;

    // Start is called before the first frame update
    void Start()
    {
        ButtonBackAbout.onClick.AddListener(BackToMainMenuAbout);
    }

    private void BackToMainMenuAbout()
    {
        ScenesManager.Instance.LoadScene(Scene.MainMenu);
    }

}