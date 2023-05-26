using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIKeyboardHandler : MonoBehaviour
{
    public GameObject InputFieldGameObject;
    public static TMPro.TMP_InputField Input;

    private bool spawned = false;
    private float decay;
    private KeyCode? lastKey;
    private int counter;
    private float delay = 0.04f;
    private float nextTime = 0f;

    void Start()
    {
        UIKeyboardHandler.Input = InputFieldGameObject.GetComponent<TMPro.TMP_InputField>();
    }

    void Update()
    {
        Reset();

        var key = InputExtensions.GetCurrentKeyDown();

        if (Time.time >= nextTime)
        {
            if (key != null)
            {
                if (key != lastKey)
                    counter = 0;

                if (!spawned || key != lastKey)
                {
                    decay = 0.5f;
                    spawned = true;

                    InvokeButtonPress(key);

                    lastKey = key;
                }
            }

            nextTime += delay;
        }
    }

    private void Reset()
    {
        if (counter > 0)
            decay = -1;

        if (spawned && decay > 0)
            decay -= Time.deltaTime;

        if (decay < 0)
        {
            decay = 0;
            spawned = false;
            counter += 1;
        }
    }

    private void InvokeButtonPress(KeyCode? key)
    {
        var buttonGameObject = GameObject.Find($"Button {key.ToString()}");

        if (buttonGameObject == null) return;

        var button = buttonGameObject.GetComponent<Button>();

        button.Select();
        button.onClick.Invoke();

        /*ExecuteEvents.Execute(buttonGameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
        ExecuteEvents.Execute(buttonGameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
        ExecuteEvents.Execute(buttonGameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);*/

    }
}
