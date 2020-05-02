using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridRow : MonoBehaviour
{
    [SerializeField] HackGridSquare[] gridSquares;
    [SerializeField] int rowNumber;

    public int GetRowNumber()
    {
        return rowNumber;
    }

    public HackGridSquare GetSquareByNumber(int squareNumber)
    {
        return gridSquares[squareNumber];
    }

    public void LogRowNumber()
    {
        Debug.Log("Row Number: " + rowNumber);
    }
}
