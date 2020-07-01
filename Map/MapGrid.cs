using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid : MonoBehaviour
{
    public static MapGrid instance { get; private set; }

    //config
    [SerializeField] MapSquareRow[] rows;
    [SerializeField] PlayerMarker playerMarkerPrefab;
    int startingRow = 0;
    int startingSquare = 9;
    MapSquareImageHolder mapSquareImageHolder;
    int percentChanceForSpawn = 10;
    string mapType;

    // state
    List<MapSquareRow> activeRows;
    bool isInitialized = false;
    int transportationNodeCount = 0; // amount of transportation nodes, needs to be either 0 or greater than 1

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetupGrid(string newMapType, int mapSize)
    {
        mapType = newMapType;
        isInitialized = true;
        activeRows = new List<MapSquareRow>();
        mapSquareImageHolder = FindObjectOfType<MapSquareImageHolder>();
        mapSquareImageHolder.InitializeMapSquareImageHolder(mapType);

        SetupSquareLevels();
        SetupMap(mapType, mapSize, GetVerticalModifier(mapType), GetInitialEnemySpawnChance(mapType));
    }

    private void SetupMap(string mapType, int mapSize, int mapSquareChanceForVerticalModifier, int percentEnemySpawn)
    {
        // initialize starting square in first row
        int activeSquares = 1;
        activeRows.Add(rows[startingRow]);
        rows[startingRow].InitializeFirstSquare(startingSquare, mapSquareImageHolder.GetSquareImage(mapType));

        // then start spawning the rest
        int currentRow = startingRow;
        ConsolidateNewActiveSquaresInAllRows();
        while (activeSquares < mapSize)
        {
            // go through active rows and spawn squares around each active square
            for (int i = activeRows.Count - 1; i >= 0; i--)
            {
                int amountOfNewSquares = activeRows[i].AttemptToSpawnSquares(mapType, percentChanceForSpawn, mapSize - activeSquares, mapSquareChanceForVerticalModifier, percentEnemySpawn);
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

        CheckNumberOfTransportationHacks();
    }

    private void CheckNumberOfTransportationHacks()
    {
        if (transportationNodeCount == 0)
        {
            Debug.Log("None spawned");
            return;
        } else if (transportationNodeCount > 1)
        {
            Debug.Log("Many spawned");
            return;
        } else
        {
            Debug.Log("One spawned");
            MapSquare[] squares = FindObjectsOfType<MapSquare>();
            List<MapSquare> potentialSpawns = new List<MapSquare>();
            foreach (MapSquare square in squares)
            {
                if (square.IsActive() && !square.DoesSquareHaveTransportationNode())
                {
                    potentialSpawns.Add(square);
                }
            }

            Debug.Log("Found " + potentialSpawns.Count + " squares without transportation");
            MapSquare squareToSpawnTransportation = potentialSpawns[Random.Range(0, potentialSpawns.Count)];
            squareToSpawnTransportation.SpawnTransportationNode();
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

    private int GetInitialEnemySpawnChance(string mapType)
    {
        switch (mapType) {
            case "slums":
                return 66;
        }
        return 0;
    }

    public bool GetIsInitialized()
    {
        return isInitialized;
    }

    public string GetMapType()
    {
        return mapType;
    }

    public void AddATransportationNode()
    {
        transportationNodeCount++;
    }
}
