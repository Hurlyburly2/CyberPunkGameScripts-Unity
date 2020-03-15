using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : ScriptableObject
{
    [SerializeField] string characterName;
    [SerializeField] int maximumHealth;
    [SerializeField] int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        characterName = "";
        maximumHealth = 0;
        currentHealth = 0;
    }

    // Setup Character for Test
    public void SetupCharacter(string newName, int newMaxHealth, int newCurrentHealth)
    {
        characterName = newName;
        maximumHealth = newMaxHealth;
        currentHealth = newCurrentHealth;
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

    // Debug Logging
    public void LogCharacterData()
    {
        Debug.Log("Name: " + characterName + ".\n Max Health: " + maximumHealth + ".\n Current Health: " + currentHealth);
    }
}
