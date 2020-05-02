using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridRowHolder : MonoBehaviour
{
    [SerializeField] GridRow[] rows;

    void InitializeGrid(int[] rowsToInit)
    {
        
    }

    public GridRow GetRowByNumber(int rowNumber)
    {
        return rows[rowNumber];
    }
}
