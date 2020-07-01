using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetroButton : MonoBehaviour
{
    [SerializeField] MapSquare square;

    private void OnMouseUp()
    {
        Debug.Log("Clicked Metro Button");
    }
}
