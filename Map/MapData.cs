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
    int enemyHindrance = 0;
        // enemyHindrance helps to hinder enemy spawn, each tick lowers the level by one
    int enemySpawnBlock = 0;
        // enemySpawnBlock blocks a successful spawn, then goes down by one. If zero- doesn't function
    int playerHealthRegenDuration = 0;
    int playerEnergyRegenDuration = 0;

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
        CheckMapGridExists();
        TrapsSpring();

        PostMovementActions(currentSquare);

        if (currentSquare.GetEnemy() != null && mapGrid.GetStealthMovement() < 1)
        {
            StartBattle(currentSquare);
        } else if (currentSquare.GetEnemy() != null && mapGrid.GetStealthMovement() > 0)
        {
            mapGrid.UseAStealthCharge();
        }
    }

    private void CheckMapGridExists()
    {
        if (mapGrid == null)
            mapGrid = FindObjectOfType<MapGrid>();
    }

    public void StartBattle(MapSquare currentSquare)
    {
        FindObjectOfType<SceneLoader>().LoadBattleFromMap(currentSquare);
    }

    private void PostMovementActions(MapSquare currentSquare)
    {
        CheckRegen();
        AttemptToSpawnEnemy();
        RaiseSecurityLevel(currentSquare);
    }

    private void CheckRegen()
    {
        if (playerHealthRegenDuration > 0)
        {
            float maxHealth = runner.GetMaximumHealth();
            int amountToHeal = Mathf.FloorToInt(maxHealth * 0.10f);
            if (amountToHeal < 1)
            {
                amountToHeal = 1;
            }

            runner.GainHealthOnMap(amountToHeal);
            playerHealthRegenDuration--;
        }

        if (playerEnergyRegenDuration > 0)
        {
            float maxEnergy = runner.GetMaximumEnergy();
            int amountToGain = Mathf.FloorToInt(maxEnergy * 0.05f);
            if (amountToGain < 1)
            {
                amountToGain = 1;
            }

            runner.GainEnergyOnMap(amountToGain);
            playerEnergyRegenDuration--;
        }
    }

    private void TrapsSpring()
    {
        Debug.Log("Traps Spring!");
    }

    private void AttemptToSpawnEnemy()
    {
        if (enemyHindrance > 0)
        {
            Debug.Log("Enemy percent spawn hindrance");
            enemyHindrance--;
        }
        if (enemySpawnBlock > 0)
        {
            enemySpawnBlock--;
            Debug.Log("Blocked spawning an enemy");
        } else
        {
            Debug.Log("Spawn an Enemy");
        }
    }

    private void RaiseSecurityLevel(MapSquare currentSquare)
    {
        if (currentSquare.GetIsVentilationMapped())
        {
            Debug.Log("Player Using Vents, Less or No Security Penalty");
        } else
        {
            Debug.Log("Raise Security Level");
        }
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
        float multipliedAmount = amount * percentMultiplier;
        moneyEarned += amount + Mathf.FloorToInt(multipliedAmount);
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

    public void RaiseEnemyHindrance(int amount)
    {
        enemyHindrance += amount;
    }

    public void RaiseBlockEnemySpawn(int amount)
    {
        enemySpawnBlock += amount;
    }

    public void AddDurationToHealthRegen(int amount)
    {
        playerHealthRegenDuration += amount;
    }

    public void AddDurationToEnergyRegen(int amount)
    {
        playerEnergyRegenDuration += amount;
    }

    public void GainHealth(int percentToGain)
    {
        float maxHealth = runner.GetMaximumHealth();
        float amount = (float)percentToGain / 100;
        int amountToHeal = Mathf.FloorToInt(maxHealth * amount);
        if (amountToHeal < 1)
            amountToHeal = 1;
        runner.GainHealthOnMap(amountToHeal);
    }
}
