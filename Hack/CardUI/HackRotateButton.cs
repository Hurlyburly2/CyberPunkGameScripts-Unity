using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackRotateButton : MonoBehaviour
{
    [SerializeField] HackCardUIHolder hackCardUIHolder;
    [SerializeField] string leftOrRight;
        // determines which direction it rotates

    int currentRotationAmount;

    private void Start()
    {
        string parentName = transform.parent.gameObject.transform.parent.gameObject.name;
        Debug.Log("arrow start");
        Debug.Log("parent card name: " + parentName);
    }

    public void SetRotationAmount(int rotationAmount)
    {
        currentRotationAmount = rotationAmount;
    }

    private void OnMouseUp()
    {
        hackCardUIHolder.SendCardRotationToSquare(currentRotationAmount, leftOrRight);
    }
}
