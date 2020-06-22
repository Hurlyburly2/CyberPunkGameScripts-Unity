using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMarker : MonoBehaviour
{
    Vector3 targetPosition;
    string state;
        // normal, moving
    MapSquare currentSquare;

    private void Start()
    {
        SetState("normal");
    }

    private void Update()
    {
        if (state == "moving")
        {
            float step = 5 * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
            if (transform.position == targetPosition)
            {
                SetState("normal");
                FindObjectOfType<MapData>().PlayerFinishesMoving(currentSquare);
            }
        }
    }

    public void SetTargetPosition(Vector3 newTargetPosition)
    {
        targetPosition = newTargetPosition;
    }

    public void moveTowardTargetPosition()
    {
        SetState("moving");
    }

    private void SetState(string newState)
    {
        state = newState;
    }

    public void SetCurrentSquare(MapSquare newCurrentSquare)
    {
        currentSquare = newCurrentSquare;
    }
}
