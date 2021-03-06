﻿using System.Collections;
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
    Job.JobArea mapType;

    // state
    List<MapSquareRow> activeRows;
    bool isInitialized = false;
    int transportationNodeCount = 0; // amount of transportation nodes, needs to be either 0 or greater than 1
    int stealthMovement = 0;
        // each stack lets you avoid one combat, avoiding combat does not despawn enemy and moving
        // back to that space will trigger another stack loss (or combat if this is zero)

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetupGrid(Job.JobArea newMapType, int mapSize)
    {
        mapType = newMapType;
        isInitialized = true;
        activeRows = new List<MapSquareRow>();
        mapSquareImageHolder = FindObjectOfType<MapSquareImageHolder>();
        mapSquareImageHolder.InitializeMapSquareImageHolder(mapType);

        SetupSquareLevels();
        SetupMap(mapType, mapSize, GetVerticalModifier(mapType), GetInitialEnemySpawnChance(mapType));
    }

    private void SetupMap(Job.JobArea mapType, int mapSize, int mapSquareChanceForVerticalModifier, int percentEnemySpawn)
    {
        // initialize starting square in first row
        int activeSquares = 1;
        activeRows.Add(rows[startingRow]);
        MapSquare firstSquare = rows[startingRow].InitializeFirstSquare(startingSquare, mapSquareImageHolder.GetSquareImage(mapType));

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

        // This is just for debug purposes
        //CheckNumberOfTransportationHacks();

        SpawnGoal(firstSquare);
    }

    public void AttemptToSpawnAnEnemy(int securityLevel)
    {
        List<MapSquare> squares = new List<MapSquare>();
        foreach (MapSquareRow row in rows)
        {
            squares.AddRange(row.GetMapSquares());
        }

        List<MapSquare> unexploredEnemylessSquares = new List<MapSquare>();
        List<MapSquare> exploredEnemylessSquare = new List<MapSquare>();

        int enemyCount = 0;
        foreach (MapSquare square in squares)
        {
            if (square.IsActive())
            {
                if (square.GetEnemy() == null && square.GetIsExplored())
                {
                    exploredEnemylessSquare.Add(square);
                } else if (square.GetEnemy() == null && !square.GetIsExplored())
                {
                    unexploredEnemylessSquares.Add(square);
                } else
                {
                    enemyCount++;
                }
            }
        }

        // We add the unexplored ones twice, so there's double the chance of an unexplored
        // space spawning an enemy
        List<MapSquare> possibleGenerationSquares = new List<MapSquare>();
        possibleGenerationSquares.AddRange(exploredEnemylessSquare);
        possibleGenerationSquares.AddRange(unexploredEnemylessSquares);
        possibleGenerationSquares.AddRange(unexploredEnemylessSquares);

        if (possibleGenerationSquares.Count > 0)
        {
            MapSquare squareToSpawn = possibleGenerationSquares[Random.Range(0, possibleGenerationSquares.Count - 1)];
            MapData mapData = FindObjectOfType<MapData>();
            squareToSpawn.SpawnEnemy(mapType, mapData.GetSecurityLevel(), mapData.GetJob());
            Debug.Log("Spawned a new enemy");
        }
    }

    private void SpawnGoal(MapSquare firstSquare)
    {
        MapSquare[] allSquares = FindObjectsOfType<MapSquare>();
        List<MapSquare> activeSquares = new List<MapSquare>();

        // get the distance for each square
        foreach (MapSquare square in allSquares)
        {
            if (square.IsActive())
            {
                square.SetTemporaryPositionMeasurement(firstSquare.GetDistanceToTargetSquare(square));
                activeSquares.Add(square);
            }
        }

        activeSquares.Sort(SortByDistance);
        int longestDistance = activeSquares[activeSquares.Count - 1].GetDistanceMeasurement();

        // then narrow down our options to distance = the most, or distance = the most - 1
        List<MapSquare> potentialGoalSquares = new List<MapSquare>();
        foreach (MapSquare square in activeSquares)
        {
            if (square.GetDistanceMeasurement() >= longestDistance - 1)
            {
                potentialGoalSquares.Add(square);
            }
        }

        potentialGoalSquares[Random.Range(0, potentialGoalSquares.Count)].SetGoalSquare();
    }

    private int SortByDistance(MapSquare square1, MapSquare square2)
    {
        return square1.GetDistanceMeasurement().CompareTo(square2.GetDistanceMeasurement());
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

    private int GetVerticalModifier(Job.JobArea mapType)
    {
        switch (mapType)
        {
            case Job.JobArea.Slums:
                return 35;
            case Job.JobArea.Downtown:
                return 75;
            default:
                return 35;
        }
    }

    private int GetInitialEnemySpawnChance(Job.JobArea mapType)
    {
        switch (mapType) {
            case Job.JobArea.Slums:
                return 50;
            case Job.JobArea.Downtown:
                return 50;
        }
        return 0;
    }

    public bool GetIsInitialized()
    {
        return isInitialized;
    }

    public Job.JobArea GetMapType()
    {
        return mapType;
    }

    public void AddATransportationNode()
    {
        transportationNodeCount++;
    }

    public void RaiseStealthMovement(int amount)
    {
        stealthMovement += amount;
    }

    public void UseAStealthCharge()
    {
        stealthMovement--;
    }

    public int GetStealthMovement()
    {
        return stealthMovement;
    }
}
