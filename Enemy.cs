using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField] int id;
    [SerializeField] string enemyName;
    [SerializeField] int maxHealth;
    [SerializeField] int currentHealth;
    [SerializeField] int maxEnergy;
    [SerializeField] int currentEnergy;

    ConfigData configData;

    public void BattleSetup(float setupTimeInSeconds)
    {
        configData = FindObjectOfType<ConfigData>();

        configData.SetupPipManagers(this);
        //SetupHealthAndEnergyText();
    }

    public int GetEnemyId()
    {
        return id;
    }

    public string GetEnemyName()
    {
        return enemyName;
    }

    public int GetMaximumHealth()
    {
        return maxHealth;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaximumEnergy()
    {
        return maxEnergy;
    }

    public int GetCurrentEnergy()
    {
        return currentEnergy;
    }
}
