﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanZoomMap : MonoBehaviour
{
    Vector3 touchStart;
    float zoomOutMin = 5;
    float zoomOutMax = 18f;

    float cameraLowerLimit = -15.96f;
    float cameraUpperLimit = 28f;
    float cameraLeftLimit = -10.4f;
    float cameraRightLimit = 23f;

    bool cameraMoveOk = true;
    MapConfig mapConfig;
    MapSquare targetSquare;

    string state = "normal";
    // possibilities: normal, moveTowardExtraction, returnToPlayerPos

    private void Start()
    {
        mapConfig = FindObjectOfType<MapConfig>();
    }

    private void Update()
    {
        if (!mapConfig.GetIsAMenuOpen() && state == "normal")
        {
            if (Input.GetMouseButtonDown(1))
            {
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
                else if (Input.GetMouseButton(1) || Input.touchCount == 3)
                {
                    Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Camera.main.transform.position += direction;
                }
                Zoom(Input.GetAxis("Mouse ScrollWheel"));
            }
            ClampCamera();
        } else if (state == "moveTowardExtraction")
        {
            Vector3 newPosition = new Vector3(targetSquare.transform.position.x, targetSquare.transform.position.y, transform.position.z);
            if (MoveTowardTarget(newPosition))
            {
                state = "normal";
                StartCoroutine(SpawnExtractionPoint());
            }
        } else if (state == "returnToPlayerPos")
        {
            Vector3 newPosition = new Vector3(targetSquare.transform.position.x, targetSquare.transform.position.y, transform.position.z);
            if (MoveTowardTarget(newPosition))
            {
                state = "normal";
                FindObjectOfType<MapConfig>().SetIsAMenuOpen(false);
            }
        }
    }

    private bool MoveTowardTarget(Vector3 targetPosition)
    {
        float step = 10f * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        if (transform.position == targetPosition)
        {
            return true;
        }
        return false;
    }

    private void ClampCamera()
    {
        Vector3 clampMovement = transform.position;
        float camSize = Camera.main.orthographicSize;
        float aspect = Camera.main.aspect;
        float cameraTotalHeight = camSize * 2;

        clampMovement.x = Mathf.Clamp(clampMovement.x, cameraLeftLimit + camSize * aspect, cameraRightLimit - camSize * aspect);
        clampMovement.y = Mathf.Clamp(clampMovement.y, cameraLowerLimit + camSize, cameraUpperLimit - camSize);
        transform.position = clampMovement;
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

    public void ShowExtractionSquare(MapSquare newTargetSquare)
    {
        targetSquare = newTargetSquare;
        state = "moveTowardExtraction";
    }

    private IEnumerator SpawnExtractionPoint()
    {
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(0.5f);
            if (i == 1)
            {
                targetSquare.SetExtractionSquare();
            }
        }

        targetSquare = FindObjectOfType<PlayerMarker>().GetCurrentSquare();
        state = "returnToPlayerPos";
    }
}
