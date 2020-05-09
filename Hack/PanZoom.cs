using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanZoom : MonoBehaviour
{
    Vector3 touchStart;
    [SerializeField] float zoomOutMin = 5;
    [SerializeField] float zoomOutMax = 20.5f;
    CheckClickController checkClickController;

    bool cameraMoveOk = true;

    private void Start()
    {
        checkClickController = FindObjectOfType<CheckClickController>();
    }

    private void Update()
    {
        if (checkClickController.GetState() != "draggingDeck")
        {
            if (Input.GetMouseButtonDown(0))
            {
                checkClickController.ListenForClickResults();
                checkClickController.SetCameraDragResult("attemptpan");
                touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            if (cameraMoveOk)
            {
                if (Input.touchCount == 2)
                {
                    Touch touchZero = Input.GetTouch(0);
                    Touch touchOne = Input.GetTouch(1);

                    Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                    Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                    float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                    float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                    float difference = currentMagnitude - prevMagnitude;
                    Zoom(difference * 0.01f);
                }
                else if (Input.GetMouseButton(0))
                {
                    Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Camera.main.transform.position += direction;
                }
                Zoom(Input.GetAxis("Mouse ScrollWheel"));
            }
        }
    }

    private void Zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
    }

    public void AllowCameraMovement()
    {
        cameraMoveOk = true;
    }

    private void StopCameraMovement()
    {
        cameraMoveOk = false;
    }
}
