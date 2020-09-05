using System.Collections;
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
    int id;
        // id should be used in lieu of runnerName, and is used to determine which runner class is created
            // 0: Basic first Runner
    string bio;
    bool locked = true; // Is true if the player has unlocked this character

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

    public void CreateNewRunnerByClassId(int newId)
    {
        switch(newId)
        {
            case 0:
                id = newId;
                string newRunnerName = "FirstRunner";
                int newMaxHealth = 30;
                int newCurrentHealth = 30;
                int newMaxEnergy = 10;
                int newCurrentEnergy = 0;
                int newStartingHandSize = 3;
                string newBio = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";
                SetupCharacter(newRunnerName, newMaxHealth, newCurrentHealth, newMaxEnergy, newCurrentEnergy, newStartingHandSize, newBio);
                break;
            case 1:
                id = newId;
                newRunnerName = "Locked Runner1";
                newMaxHealth = 100;
                newCurrentHealth = 100;
                newMaxEnergy = 100;
                newCurrentEnergy = 100;
                newStartingHandSize = 9;
                newBio = "Character 1 not implemented.";
                SetupCharacter(newRunnerName, newMaxHealth, newCurrentHealth, newMaxEnergy, newCurrentEnergy, newStartingHandSize, newBio);
                break;
            case 2:
                id = newId;
                newRunnerName = "Locked Runner2";
                newMaxHealth = 100;
                newCurrentHealth = 100;
                newMaxEnergy = 100;
                newCurrentEnergy = 100;
                newStartingHandSize = 9;
                newBio = "Character 2 not implemented.";
                SetupCharacter(newRunnerName, newMaxHealth, newCurrentHealth, newMaxEnergy, newCurrentEnergy, newStartingHandSize, newBio);
                break;
        }
    }

    // Setup Character
    private void SetupCharacter(string newRunnerName, int newMaxHealth, int newCurrentHealth, int newMaxEnergy, int newCurrentEnergy, int newStartingHandSize, string newBio)
    {
        runnerName = newRunnerName;

        maximumHealth = newMaxHealth;
        currentHealth = newCurrentHealth;
        maximumEnergy = newMaxEnergy;
        currentEnergy = newCurrentEnergy;
        bio = newBio;

        startingHandSize = newStartingHandSize;

        loadout = CreateInstance<Loadout>();
        loadout.SetupInitialLoadout(id);
    }

    public void BattleSetup(float setupTimeInSeconds)
    {
        configData = FindObjectOfType<ConfigData>();

        FindObjectOfType<PlayerPortrait>().SetRunnerPortrait(id);

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

    public void TakeDamageFromMap(int amountToTake)
    {
        if (currentHealth - amountToTake < 0)
        {
            currentHealth = 0;
        } else
        {
            currentHealth -= amountToTake;
        }

        MapConfig mapConfig = FindObjectOfType<MapConfig>();
        mapConfig.GetHealthPipManager().ChangeValue(currentHealth);
        SetHealthText();
        // TODO IF PLAYER HEALTH <= 0 THEN PLAYER LOSES
        if (currentHealth <= 0)
        {
            Debug.Log("Player is dead");
        }
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

    public int getId()
    {
        return id;
    }

    public List<Item> GetListOfEquippedItems()
    {
        return loadout.GetModsAsItems();
    }

    public string GetBio()
    {
        return bio;
    }

    public void UnlockRunner()
    {
        locked = false;
    }

    // Debug Logging
    public void LogCharacterData()
    {
        Debug.Log("Name: " + runnerName + ".\n Max Health: " + maximumHealth + ".\n Current Health: " + currentHealth);
    }
}

