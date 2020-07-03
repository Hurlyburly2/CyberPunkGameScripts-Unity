﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterData : ScriptableObject
{
    // character info
    string runnerName;
        // "Runner" = First fighter
    int maximumHealth;
    int currentHealth;
    int maximumEnergy;
    int currentEnergy;
    int startingHandSize;

    Loadout loadout;

    // each health/energy pip is worth this much. Use as breakpoints when adding/subtracting
    float healthPipValue;
    float energyPipValue;
    List<GameObject> healthPips = new List<GameObject>();

    // config
    GameObject currentHealthText;
    GameObject currentEnergyText;
    CoroutinesForScripts coroutineObject;
    ConfigData configData;
    PipManager healthPipManager;
    PipManager energyPipManager;
    StatusEffectHolder statusEffectHolder;

    // map config
    MapConfig mapConfig;

    // Start is called before the first frame update
    void Start()
    {
        runnerName = "";
        maximumHealth = 0;
        currentHealth = 0;
        startingHandSize = 0;
    }

    // Setup Character for Test
    public void SetupCharacter(string newRunnerName, int newMaxHealth, int newCurrentHealth, int newMaxEnergy, int newCurrentEnergy, int newStartingHandSize)
    {
        runnerName = newRunnerName;

        maximumHealth = newMaxHealth;
        currentHealth = newCurrentHealth;
        maximumEnergy = newMaxEnergy;
        currentEnergy = newCurrentEnergy;

        startingHandSize = newStartingHandSize;

        loadout = CreateInstance<Loadout>();
        loadout.SetupInitialLoadout(runnerName);
    }

    public void BattleSetup(float setupTimeInSeconds)
    {
        configData = FindObjectOfType<ConfigData>();

        FindObjectOfType<PlayerPortrait>().SetPortrait(runnerName);

        configData.SetupPipManagers(this);
        SetupHealthAndEnergyText();
    }

    private void SetupHealthAndEnergyText()
    {
        currentHealthText = configData.GetHealthTextField();
        currentEnergyText = configData.GetEnergyTextField();

        SetHealthText();
        SetEnergyText();
    }

    public void MapSetup()
    {
        mapConfig = FindObjectOfType<MapConfig>();

        currentHealthText = mapConfig.GetHealthTextField();
        currentEnergyText = mapConfig.GetEnergyTextField();

        SetHealthText();
        SetEnergyText();
    }

    private void SetHealthText()
    {
        currentHealthText.GetComponent<TextMeshProUGUI>().text = currentHealth.ToString();
    }

    private void SetEnergyText()
    {
        Debug.Log(currentEnergy);
        currentEnergyText.GetComponent<TextMeshProUGUI>().text = currentEnergy.ToString();
    }

    public void GainHealth(int amountToGain)
    {
        if (currentHealth + amountToGain > maximumHealth)
        {
            currentHealth = maximumHealth;
        } else
        {
            currentHealth += amountToGain;
        }

        SetHealthText();
        healthPipManager = configData.GetPlayerHealthPipManager();
        healthPipManager.ChangeValue(currentHealth);
    }

    public void GainHealthOnMap(int amountToGain)
    {
        if (currentHealth + amountToGain > maximumHealth)
        {
            currentHealth = maximumHealth;
        }
        else
        {
            currentHealth += amountToGain;
        }

        MapConfig mapConfig = FindObjectOfType<MapConfig>();
        mapConfig.GetHealthPipManager().ChangeValue(currentHealth);
        SetHealthText();
    }

    public void GainEnergyOnMap(int amountToGain)
    {
        if (currentEnergy + amountToGain > maximumEnergy)
        {
            currentEnergy = maximumEnergy;
        } else
        {
            currentEnergy += amountToGain;
        }

        MapConfig mapConfig = FindObjectOfType<MapConfig>();
        mapConfig.GetEnergyPipManager().ChangeValue(currentEnergy);
        SetEnergyText();
    }

    public void GainEnergy(int amountToGain)
    {
        if (currentEnergy + amountToGain > maximumEnergy)
        {
            currentEnergy = maximumEnergy;
        } else
        {
            currentEnergy += amountToGain;
        }

        SetEnergyText();
        energyPipManager = configData.GetPlayerEnergyPipManager();
        energyPipManager.ChangeValue(currentEnergy);
    }

    public void TakeDamage(int amountToTake)
    {
        if (currentHealth - amountToTake > 0)
        {
            currentHealth -= amountToTake;
        } else
        {
            // Game over state here
            currentHealth = 0;
        }

        SetHealthText();
        healthPipManager = configData.GetPlayerHealthPipManager();
        healthPipManager.ChangeValue(currentHealth);
    }

    // Getters
    public string GetRunnerName()
    {
        return runnerName;
    }

    public int GetMaximumHealth()
    {
        return maximumHealth;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaximumEnergy()
    {
        return maximumEnergy;
    }

    public int GetCurrentEnergy()
    {
        return currentEnergy;
    }

    public int GetStartingHandSize()
    {
        return startingHandSize;
    }

    public Loadout GetLoadout()
    {
        return loadout;
    }

    // Debug Logging
    public void LogCharacterData()
    {
        Debug.Log("Name: " + runnerName + ".\n Max Health: " + maximumHealth + ".\n Current Health: " + currentHealth);
    }
}

