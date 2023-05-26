using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeClosestFace : MonoBehaviour
{
    // Static Properties
    public static CubeClosestFace Instance;

    // Properties
    public CubeFace CurrentFace = CubeFace.None;

    // Serialize Fields
    [SerializeField] Camera Camera;
    [SerializeField] GameObject ReferenceCube;

    // Fields
    private MainCube mainCube;
    private Quaternion startRotation;
    private Quaternion endRotation;
    private float rotationProgress = -1;

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

    void Awake()
    {
        Instance = this;
        mainCube = MainCube.Instance;
    }

    void Update()
    {
        if (!Input.GetMouseButton(0))
        {
            RaycastHit hit;

            var origin = Camera.transform.position;
            var direction = transform.position - Camera.transform.position;

            Debug.DrawRay(origin, direction, Color.red);

            if (Physics.Raycast(origin, direction, out hit, Mathf.Infinity))
            {
                var face = GetHitFace(hit);
                SetTargetAngle(face);

                if (face != CurrentFace)
                {
                    CurrentFace = face;
                    mainCube.SetCurrentFace();
                }
            }

            Rotate();
        }   
    }

    public CubeFace GetHitFace(RaycastHit hit)
    {
        CubeFace face = CubeFace.None;

        var ti = hit.triangleIndex;

        if (ti == 4 || ti == 5)        // X 
            face = CubeFace.Front;
        else if (ti == 0 || ti == 1)   // X.
            face = CubeFace.Back;
        else if (ti == 2 || ti == 3)   // Z
            face = CubeFace.Top;
        else if (ti == 6 || ti == 7)   // Z.
            face = CubeFace.Bottom;
        else if (ti == 8 || ti == 9)   // Y.
            face = CubeFace.Left;
        else if (ti == 10 || ti == 11) // Y
            face = CubeFace.Right;
        else
            face = CubeFace.None;

        return face;
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

        startRotation = transform.rotation;
        rotationProgress = 0;
    }

    private void Rotate()
    {
        if (CubeRotation.Instance == null) return;

        if (!CubeRotation.Instance.IsRotating && rotationProgress < 1 && rotationProgress >= 0)
        {
            rotationProgress += Time.deltaTime * 1;

            transform.rotation = Quaternion.Lerp(startRotation, endRotation, rotationProgress);
            ReferenceCube.transform.rotation = Quaternion.Lerp(startRotation, endRotation, rotationProgress);
        }
    }

}
