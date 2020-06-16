using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid : MonoBehaviour
{
    [SerializeField] MapSquareRow[] rows;
    int startingRow = 0;
    int startingSquare = 9;
    MapSquareImageHolder mapSquareImageHolder;

    public void SetupGrid(string mapType, int mapSize)
    {
        mapSquareImageHolder = FindObjectOfType<MapSquareImageHolder>();
        mapSquareImageHolder.Initialize(mapType);

        SetupSquareLevels();
        SetupMap(mapType, mapSize);
    }

    private void SetupMap(string mapType, int mapSize)
    {
        int activeSquares = 1;
        List<int> activeRows = new List<int>();

        activeRows.Add(0);
        rows[startingRow].GetSquareByRowPosition(startingSquare).InitializeSquare(mapSquareImageHolder.GetSquareImage());

        while (activeSquares < mapSize)
        {

            activeSquares++;
        }
    }

    private void SetupSquareLevels()
    {
        int currentNumber = 0;
        for (int i = 19; i > -1; i--)
        {
            rows[i].SetLayerOrder(currentNumber);
            currentNumber += 20;
        }
    }
}
