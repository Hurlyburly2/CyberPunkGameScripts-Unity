using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridRow : MonoBehaviour
{
    [SerializeField] HackGridSquare[] gridSquares;
    [SerializeField] int rowNumber;

    public void LogRowNumber()
    {
        Debug.Log("Row Number: " + rowNumber);
    }
}
