using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid : MonoBehaviour
{
    //config
    [SerializeField] MapSquareRow[] rows;
    [SerializeField] PlayerMarker playerMarkerPrefab;
    int startingRow = 0;
    int startingSquare = 9;
    MapSquareImageHolder mapSquareImageHolder;
    int percentChanceForSpawn = 10;

    // state
    List<MapSquareRow> activeRows;

    public void SetupGrid(string mapType, int mapSize)
    {
        activeRows = new List<MapSquareRow>();
        mapSquareImageHolder = FindObjectOfType<MapSquareImageHolder>();
        mapSquareImageHolder.Initialize(mapType);

        SetupSquareLevels();
        SetupMap(mapType, mapSize, GetVerticalModifier(mapType));
    }

    private void SetupMap(string mapType, int mapSize, int mapSquareChanceForVerticalModifier)
    {
        // initialize starting square in first row
        int activeSquares = 1;
        activeRows.Add(rows[startingRow]);
        rows[startingRow].InitializeFirstSquare(startingSquare, mapSquareImageHolder.GetSquareImage());

        // then start spawning the rest
        int currentRow = startingRow;
        ConsolidateNewActiveSquaresInAllRows();
        while (activeSquares < mapSize)
        {
            // go through active rows and spawn squares around each active square
            for (int i = activeRows.Count - 1; i >= 0; i--)
            {
                int amountOfNewSquares = activeRows[i].AttemptToSpawnSquares(percentChanceForSpawn, mapSize - activeSquares, mapSquareChanceForVerticalModifier);
                activeSquares += amountOfNewSquares;
            }

            ConsolidateNewActiveSquaresInAllRows();

            // then look for active rows and add them to the active rows list, setting them up for spawns as well
            foreach (MapSquareRow row in rows)
            {
                if (row.ContainsActiveSquares() && !activeRows.Contains(row))
                {
                    activeRows.Add(row);
                }
            }
        }
        int activeSquareAccurateCount = 0;
        foreach(MapSquareRow row in rows)
        {
            activeSquareAccurateCount += row.CountActiveSquares();
        }
        Debug.Log("We counted " + activeSquareAccurateCount + " squares.");
        Debug.Log("it thinks we made " + activeSquares + " squares.");
    }

    private int GetVerticalModifier(string mapType)
    {
        switch (mapType)
        {
            case "slums":
                return 35;
            default:
                return 35;
        }
    }

    public void ConsolidateNewActiveSquaresInAllRows()
    {
        foreach (MapSquareRow row in rows)
        {
            row.ConsolidateNewActiveSquares();
        }
    }

    public MapSquareRow GetRowByNumber(int rowNumber)
    {
        return rows[rowNumber];
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

    public PlayerMarker GetPlayerMarkerPrefab()
    {
        return playerMarkerPrefab;
    }
}
