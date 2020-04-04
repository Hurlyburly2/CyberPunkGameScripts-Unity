using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class Enemy : MonoBehaviour
{
    // stats
    [SerializeField] int id;
    [SerializeField] string enemyName;
    [SerializeField] int maxHealth;
    [SerializeField] int currentHealth;
    [SerializeField] int maxEnergy;
    [SerializeField] int currentEnergy;
    [SerializeField] int handSize;

    // config
    GameObject currentHealthText;
    GameObject currentEnergyText;
    ConfigData configData;
    PipManagerEnemy healthPipManager;
    PipManagerEnemy energyPipManager;
    EnemyDeck enemyDeck;

    public void BattleSetup(float setupTimeInSeconds)
    {
        configData = FindObjectOfType<ConfigData>();
        configData.SetupPipManagers(this);
        healthPipManager = configData.GetEnemyHealthPipManager();
        energyPipManager = configData.GetEnemyEnergyPipManager();
        SetupHealthAndEnergyText();

        enemyDeck = FindObjectOfType<EnemyDeck>();
        enemyDeck.SetupDeck();
    }

    public void TakeDamage(int damageInflicted)
    {
        // TODO: CALCULATE ALL MODIFIERS FOR DAMAGE TAKEN
        if (currentHealth - damageInflicted < 1)
        {
            currentHealth = 0;
            healthPipManager.ChangeValue(currentHealth);
            // TODO: LOGIC FOR KILLING ENEMY HERE
        } else
        {
            currentHealth -= damageInflicted;
            UpdateHealthText();
            healthPipManager.ChangeValue(currentHealth);
        }
    }

    private void SetupHealthAndEnergyText()
    {
        currentHealthText = configData.GetEnemyHealthTextField();
        currentEnergyText = configData.GetEnemyEnergyTextField();
        UpdateHealthText();
        UpdateEnergyText();
    }

    private void UpdateHealthText()
    {
        currentHealthText.GetComponent<TextMeshProUGUI>().text = currentHealth.ToString();
    }

    private void UpdateEnergyText()
    {
        currentEnergyText.GetComponent<TextMeshProUGUI>().text = currentEnergy.ToString();
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
