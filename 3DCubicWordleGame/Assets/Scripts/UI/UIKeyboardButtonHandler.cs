using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIKeyboardButtonHandler : MonoBehaviour
{
    public static UIKeyboardButtonHandler Instance;

    private MainCube mainCube;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        mainCube = MainCube.Instance;
    }

    public void ButtonPress()
    {
        var button = EventSystem.current.currentSelectedGameObject;
        var text = button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text;

        EventSystem.current.SetSelectedGameObject(null);

        if (button.name == "Button Return")
        {
            if (!mainCube.currentFace.Done)
                mainCube.GuessWord();
        }
        else if (button.name == "Button Backspace")
        {
            mainCube.RemoveFromCurrentFace();

            if (UIKeyboardHandler.Input.text.Length > 0)
            {
                UIKeyboardHandler.Input.text = UIKeyboardHandler.Input.text.Remove(UIKeyboardHandler.Input.text.Length - 1, 1);
            }
        }
        else
        {
            mainCube.WriteToCurrentFace(text);

            //UIKeyboardHandler.Input.text += text;
        }
    }

    public void ChangeButtonColor(string letter, Color color)
    {
        var buttonGameObject = GameObject.Find($"Button {letter}");

        if (buttonGameObject == null) return;

        var button = buttonGameObject.GetComponent<Button>();

        var image = button.GetComponent<Image>();

        if (image.color == Color.green && color != Color.white) return;

        image.color = color;
    }

    public void ClearKeyboard()
    {
        for (char c = 'A'; c <= 'Z'; c++)
        {
            ChangeButtonColor(c.ToString(), Color.white);
        }
    }
}
