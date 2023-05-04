using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRayCast : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] GameObject _mainCube;
    [SerializeField] GameObject _referenceCube;

    Quaternion startRotation;
    Quaternion endRotation;
    float rotationProgress = -1;

    public enum CubeFace
    {
        None,
        Front,
        Back,
        Top,
        Bottom,
        Left,
        Right
    }

    public CubeFace GetHitFace(RaycastHit hit)
    {
        CubeFace face = CubeFace.None;

        var ti = hit.triangleIndex;


        if (ti == 4 || ti == 5)
            face = CubeFace.Front;
        else if (ti == 0 || ti == 1)
            face = CubeFace.Back;
        else if (ti == 2 || ti == 3)
            face = CubeFace.Top;
        else if (ti == 6 || ti == 7)
            face = CubeFace.Bottom;
        else if (ti == 8 || ti == 9)
            face = CubeFace.Left;
        else if (ti == 10 || ti == 11)
            face = CubeFace.Right;
        else
            face = CubeFace.None;

        return face;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, 5);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(ray.origin, ray.direction);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                SetTargetAngle(GetHitFace(hit));
            else
                Debug.Log("No object found");
        }


        RotateMainCube();
    }

    private void SetTargetAngle(CubeFace face)
    {
        switch (face)
        {
            case CubeFace.Front:
                endRotation = Quaternion.Euler(0, 0, 0);
                break;
            case CubeFace.Back:
                endRotation = Quaternion.Euler(0, 180, 0);
                break;
            case CubeFace.Top:
                endRotation = Quaternion.Euler(-90, 0, 0);
                break;
            case CubeFace.Bottom:
                endRotation = Quaternion.Euler(90, 0, 0);
                break;
            case CubeFace.Left:
                endRotation = Quaternion.Euler(0, -90, 0);
                break;
            case CubeFace.Right:
                endRotation = Quaternion.Euler(0, 90, 0);
                break;
            default:
                endRotation = Quaternion.Euler(0, 0, 0);
                break;
        }

        startRotation = _mainCube.transform.rotation;
        rotationProgress = 0;
    }

    private void RotateMainCube()
    {
        if (!CubeRotation.Instance.IsRotating && rotationProgress < 1 && rotationProgress >= 0)
        {
            rotationProgress += Time.deltaTime * 1;

            _mainCube.transform.rotation = Quaternion.Lerp(startRotation, endRotation, rotationProgress);
            _referenceCube.transform.rotation = Quaternion.Lerp(startRotation, endRotation, rotationProgress);
        }
    }
}
