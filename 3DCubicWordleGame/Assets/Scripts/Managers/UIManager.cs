using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private ScenesManager sceneManager;

    [SerializeField] GameObject PanelGameOver;
    [SerializeField] TMP_Text TextScore;
    [SerializeField] Button ButtonRestart;
    [SerializeField] Button ButtonMainMenu;

    [SerializeField] GameObject PanelHints;

    bool isGameOver = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        sceneManager = ScenesManager.Instance;
        PanelGameOver.SetActive(false);
        PanelHints.SetActive(false);

        ButtonRestart.onClick.AddListener(ButtonPressRestart);
        ButtonMainMenu.onClick.AddListener(ButtonPressMainMenu);


        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0) && !isGameOver)
        {
            isGameOver = true;
            PanelGameOver.SetActive(true);
        }
    }

    void ButtonPressRestart()
    {
        sceneManager.LoadScene(Scene.Singleplayer);
    }

    void ButtonPressMainMenu()
    {
        sceneManager.LoadMainMenu();
    }

    public void ActivateGameOver(int score)
    {
        isGameOver = true;
        TextScore.SetText($"Score: {score}");
        PanelGameOver.SetActive(true);
    }
}
