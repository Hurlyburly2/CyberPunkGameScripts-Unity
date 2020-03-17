﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterData : ScriptableObject
{
    // character info
    string characterName;
    int maximumHealth;
    int currentHealth;
    int maximumEnergy;
    int currentEnergy;
    int startingHandSize;

    // each health/energy pip is worth this much. Use as breakpoints when adding/subtracting
    float healthPipValue;
    float energyPipValue;
    List<GameObject> healthPips = new List<GameObject>();

    // config
    GameObject currentHealthText;
    GameObject currentEnergyText;
    CoroutinesForScripts coroutineObject;
    ConfigData configData;

    // Start is called before the first frame update
    void Start()
    {
        characterName = "";
        maximumHealth = 0;
        currentHealth = 0;
        startingHandSize = 0;
    }

    // Setup Character for Test
    public void SetupCharacter(string newName, int newMaxHealth, int newCurrentHealth, int newMaxEnergy, int newCurrentEnergy, int newStartingHandSize)
    {
        characterName = newName;

        maximumHealth = newMaxHealth;
        currentHealth = newCurrentHealth;
        maximumEnergy = newMaxEnergy;
        currentEnergy = newCurrentEnergy;

        startingHandSize = newStartingHandSize;
    }

    public void BattleSetup(float setupTimeInSeconds)
    {
        configData = FindObjectOfType<ConfigData>();
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

    private void SetHealthText()
    {
        currentHealthText.GetComponent<TextMeshProUGUI>().text = currentHealth.ToString();
    }

    private void SetEnergyText()
    {
        currentEnergyText.GetComponent<TextMeshProUGUI>().text = currentEnergy.ToString();
    }

    // Getters
    public string GetCharacterName()
    {
        return characterName;
    }

    public int GetMaximumHealth()
    {
        return maximumHealth;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetStartingHandSize()
    {
        return startingHandSize;
    }

    // Debug Logging
    public void LogCharacterData()
    {
        Debug.Log("Name: " + characterName + ".\n Max Health: " + maximumHealth + ".\n Current Health: " + currentHealth);
    }
}

