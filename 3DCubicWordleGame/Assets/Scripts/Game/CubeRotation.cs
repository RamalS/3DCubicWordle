using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CubeRotation : MonoBehaviour
{
    public static CubeRotation Instance;

    public bool IsRotating = false;

    Vector3 mousePrevPos = Vector3.zero;
    Vector3 mousePosDelta = Vector3.zero;

    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        IsRotating = Input.GetMouseButton(0) && EventSystem.current.currentSelectedGameObject == null;

        if (IsRotating)
        {
            mousePosDelta = Input.mousePosition - mousePrevPos;

            float upDotProd = Vector3.Dot(transform.up, Vector3.up);
            float mouseRightDotProd = Vector3.Dot(mousePosDelta, Camera.main.transform.right);
            float mouseUpDotProd = Vector3.Dot(mousePosDelta, Camera.main.transform.up);

            if (upDotProd >= 0)
            {
                transform.Rotate(transform.up, -mouseRightDotProd, Space.World);
            }
            else
            {
                transform.Rotate(transform.up, mouseRightDotProd, Space.World);
            }

            transform.Rotate(Camera.main.transform.right, mouseUpDotProd, Space.World);
        }

        mousePrevPos = Input.mousePosition;




    }
}
