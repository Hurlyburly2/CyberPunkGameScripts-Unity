using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitBackground : MonoBehaviour
{
    public void UpdateBackgroundPos()
    {
        float cameraSize = Camera.main.orthographicSize;
        transform.localScale = new Vector3(cameraSize, cameraSize, cameraSize);
        transform.position = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
    }
}
