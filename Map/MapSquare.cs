using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSquare : MonoBehaviour
{
    [SerializeField] int rowPosition; // 0-19 position in row
    [SerializeField] MapSquareRow parentRow;

    public void InitializeSquare(Sprite newImage)
    {
        GetComponent<SpriteRenderer>().sprite = newImage;
    }

    private void OnMouseUp()
    {
        Debug.Log("clicked");
    }
}
