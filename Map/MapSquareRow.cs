using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSquareRow : MonoBehaviour
{
    [SerializeField] int rowNumber;
    [SerializeField] MapSquare[] mapSquares;
    [SerializeField] MapGrid mapGrid;

    public MapSquare GetSquareByRowPosition(int rowPosition)
    {
        return mapSquares[rowPosition];
    }

    public void SetLayerOrder(int number)
    {
        foreach (MapSquare square in mapSquares)
        {
            square.GetComponent<SpriteRenderer>().sortingOrder = number;
            square.enabled = false;
            number++;
        }
    }
}
