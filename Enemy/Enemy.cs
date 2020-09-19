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
    [SerializeField] GameObject playCardZone;
    [SerializeField] Sprite tinyMapImage;
    [SerializeField] int starRating;
        // star rating represents enemy difficulty and ranges from 0-5
    [SerializeField] List<Job.EnemyType> enemyTypes;

    // config
    GameObject currentHealthText;
    GameObject currentEnergyText;
    ConfigData configData;
    PipManagerEnemy healthPipManager;
    PipManagerEnemy energyPipManager;
    StatusEffectHolder statusEffectHolder;

    EnemyDeck enemyDeck;
    EnemyHand enemyHand;
    EnemyDiscard enemyDiscard;

    public void BattleSetup(float setupTimeInSeconds)
    {
        configData = FindObjectOfType<ConfigData>();
        configData.SetupPipManagers(this);
        healthPipManager = configData.GetEnemyHealthPipManager();
        energyPipManager = configData.GetEnemyEnergyPipManager();
        SetupHealthAndEnergyText();

        enemyDeck = FindObjectOfType<EnemyDeck>();
        enemyDeck.SetupDeck();

        enemyDiscard = FindObjectOfType<EnemyDiscard>();
        enemyHand = FindObjectOfType<EnemyHand>();
    }

    public void StartTurn()
    {
        TakeDotDamageFromMap(FindObjectOfType<BattleData>().GetMapDotDamage());
        enemyHand.DrawInitialHand(handSize);
    }

    public void PlayCards()
    {
        enemyHand.PlayAllCards();
    }

    public void FinishTurn()
    {
        enemyHand.ClearPlayedCards();
    }

    public void TakePercentDamageFromMap(int percent)
    {
        float percentageAmount = percent / 100f;
        int amountOfDamage = Mathf.FloorToInt(maxHealth * percentageAmount);
        if (currentHealth - amountOfDamage > 0)
        {
            currentHealth -= amountOfDamage;
        } else
        {
            currentHealth = 0;
        }
    }

    public void TakeDotDamageFromMap(int percent)
    {
        int minimumDamage = percent / 2;

        float percentAgeAmount = percent / 100f;
        int percentDamage = Mathf.FloorToInt(maxHealth * percentAgeAmount);

        if (percentDamage > minimumDamage)
        {
            TakeDamage(percentDamage);
        } else
        {
            TakeDamage(minimumDamage);
        }
    }

    public void TakeDamage(int damageInflicted)
    {
        if (currentHealth - damageInflicted < 1)
        {
            currentHealth = 0;
            UpdateHealthText();
            healthPipManager.ChangeValue(currentHealth);
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

    public Sprite GetThumbnailImage()
    {
        return tinyMapImage;
    }

    public int GetStarRating()
    {
        return starRating;
    }

    public List<Job.EnemyType> GetEnemyTypes()
    {
        return enemyTypes;
    }
}
