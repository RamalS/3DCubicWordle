using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIKeyboardButtonHandler : MonoBehaviour
{
    private MainCube mainCube;

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

            UIKeyboardHandler.Input.text += text;
        }
    }
}
