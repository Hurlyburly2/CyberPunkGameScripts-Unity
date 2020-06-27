﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
    // config
    CharacterData runner;
    HackerData hacker;
    string mapType;
        // possible: "slums"
    MapConfig mapConfig;
    [SerializeField] float setupTimeInSeconds = 1f;
    MapGrid mapGrid;
    int mapSize;

    // state
    int securityLevel;

    // reward stuff
    int moneyEarned; // in match
    int moneyMultiplier; // percent multiplier added to money earned in round
    int goalMultiplier; // percent multiplier added to money earned at end of round

    private void Awake()
    {
        int count = FindObjectsOfType<MapData>().Length;
        moneyEarned = 0;
        moneyMultiplier = 0;
        goalMultiplier = 0;

        if (count > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void MovePlayer(MapSquare currentSquare, MapSquare targetSquare)
    {
        PlayerMarker playerMarker = FindObjectOfType<PlayerMarker>();
        playerMarker.SetTargetPosition(targetSquare.GetPlayerMarkerPosition());
        playerMarker.moveTowardTargetPosition();
        playerMarker.SetCurrentSquare(targetSquare);

        currentSquare.UnsetPlayerPosition();
        targetSquare.SetPlayerPosition();
    }

    public void PlayerFinishesMoving(MapSquare currentSquare)
    {
        TrapsSpring();
        if (currentSquare.GetEnemy() != null)
        {
            StartBattle(currentSquare);
        }
        PostMovementActions();
    }

    public void StartBattle(MapSquare currentSquare)
    {
        FindObjectOfType<SceneLoader>().LoadBattleFromMap(currentSquare);
    }

    private void PostMovementActions()
    {
        AttemptToSpawnEnemy();
        RaiseSecurityLevel();
    }

    private void TrapsSpring()
    {
        Debug.Log("Traps Spring!");
    }

    private void AttemptToSpawnEnemy()
    {
        Debug.Log("Attempt to Spawn an Enemy");
    }

    private void RaiseSecurityLevel()
    {
        Debug.Log("Raise Security Level");
    }

    public void SetUpMap()
    {
        SetupPlayerPortraits();
        runner.MapSetup();

        mapConfig = FindObjectOfType<MapConfig>();
        mapConfig.SetupPipManagers(runner, setupTimeInSeconds, securityLevel);

        mapGrid = FindObjectOfType<MapGrid>();
        mapGrid.SetupGrid(mapType, mapSize);
    }

    public void SetUpMapFromBattle()
    {
        SetupPlayerPortraits();
        runner.MapSetup();

        mapConfig = FindObjectOfType<MapConfig>();
        mapConfig.SetupPipManagers(runner, setupTimeInSeconds, securityLevel);

        MapSquare[] mapSquares = FindObjectsOfType<MapSquare>();
        foreach (MapSquare square in mapSquares)
        {
            if (square.GetIsPlayerPresent())
            {
                square.SetPlayerStart();
                break;
            }
        }
    }

    public void SetUpMapFromHack(MapSquare currentSquare, HackTarget currentHackTarget)
    {
        SetupPlayerPortraits();
        runner.MapSetup();

        mapConfig = FindObjectOfType<MapConfig>();
        mapConfig.SetupPipManagers(runner, setupTimeInSeconds, securityLevel);

        MapSquare[] mapSquares = FindObjectsOfType<MapSquare>();
        foreach (MapSquare square in mapSquares)
        {
            if (square.GetIsPlayerPresent())
            {
                square.SetPlayerStart();
                break;
            }
        }

        currentSquare.ReopenHackMenu(currentHackTarget);
    }

    public void SetMapData(CharacterData characterToSet, HackerData hackerToSet, string newMapType, int newSecurityLevel, int newMapSize)
    {
        runner = characterToSet;
        hacker = hackerToSet;
        mapType = newMapType;
        securityLevel = newSecurityLevel;
        mapSize = newMapSize;
    }

    private void SetupPlayerPortraits()
    {
        PlayerPortrait[] portraits = FindObjectsOfType<PlayerPortrait>();
        foreach (PlayerPortrait portrait in portraits)
        {
            if (portrait.name == "RunnerPortrait")
            {
                portrait.SetPortrait(runner.GetRunnerName());
            }
            else if (portrait.name == "HackerPortrait")
            {
                portrait.SetPortrait(hacker.GetName());
            }
        }
    }

    public void AdjustSecurityLevel(int adjustment)
    {
        securityLevel += adjustment;
        if (securityLevel < 0)
            securityLevel = 0;
        if (securityLevel > 100)
            securityLevel = 100;
        mapConfig.GetSecurityPipManager().ChangeValue(securityLevel);
    }

    public void ChangeMoney(int amount)
    {
        float percentMultiplier = moneyMultiplier / 100;
        Debug.Log("percent multiplier: " + percentMultiplier);
        float multipliedAmount = amount * percentMultiplier;
        Debug.Log("money multiplier: " + moneyMultiplier);
        Debug.Log("multiplied amount: " + multipliedAmount);
        moneyEarned += amount + Mathf.FloorToInt(multipliedAmount);
        Debug.Log("Earned total of " + moneyEarned + " money.");
    }

    public void ChangeMoneyMultiplier(int amount)
    {
        moneyMultiplier += amount;
    }

    public void ChangeGoalMultiplier(int amount)
    {
        goalMultiplier += amount;
    }

    private void CheckMapConfigExists()
    {
        if (mapConfig == null)
        {
            mapConfig = FindObjectOfType<MapConfig>();
        }
    }

    public float GetSetupTimeInSeconds()
    {
        return setupTimeInSeconds;
    }

    public string GetMapType()
    {
        return mapType;
    }
}
