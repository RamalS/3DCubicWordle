using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIKeyboardButtonHandler : MonoBehaviour
{
    public void ButtonPress()
    {
        var button = EventSystem.current.currentSelectedGameObject;
        var text = button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text;

        EventSystem.current.SetSelectedGameObject(null);

        if (button.name == "Button Return")
        {
            Debug.Log($"Word: {UIKeyboardHandler.Input.text}");
        }
        else if (button.name == "Button Backspace")
        {
            if (UIKeyboardHandler.Input.text.Length > 0)
            {
                UIKeyboardHandler.Input.text = UIKeyboardHandler.Input.text.Remove(UIKeyboardHandler.Input.text.Length - 1, 1);
            }
        }
        else
        {
            UIKeyboardHandler.Input.text += text;
        }

        

        //Debug.Log($"Button click invoked {text}");

        //UIKeyboardHandler.Input.text += text;
    }
}
