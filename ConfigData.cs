using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigData : MonoBehaviour
{
    // Screen measurements
    float halfHeight;
    float halfWidth;

    // Hand config
    float handStartPos;
    float handEndPos;
    float handMiddlePos;
    float cardSizeMultiplier = 1;
    float cardWidth;

    // Health/Energy Zone Config
    [SerializeField] int maximumNumberOfPips = 28;
    [SerializeField] float distanceBetweenPips = 18f;
    [SerializeField] float pipScale;
    [SerializeField] string healthPipManagerName = "HealthPipManager";
    [SerializeField] string energyPipManagerName = "EnergyPipManager";
    PipManager healthPipManager;
    PipManager energyPipManager;

    // Enemy Health/Energy Zone Config
    string enemyHealthPipManagerName = "EnemyHealthPipManager";
    string enemyEnergyPipManagerName = "EnemyEnergyPipManager";
    string enemyHealthText = "EnemyHealthText";
    string enemyEnergyText = "EnemyEnergyText";
    PipManagerEnemy enemyHealthPipManager;
    PipManagerEnemy enemyEnergyPipManager;

    // Cards are played when dragged above this y axis
    float cardPlayLine;

    // Seriealized fields
    [SerializeField] float handDrawSpeed = 20f;
    [SerializeField] float handAdjustSpeed = 5f;
    [SerializeField] float maxCardInHandAngle = 20f;
    [SerializeField] float curvedHandMaxVerticalOffset = .75f;
    [SerializeField] string healthTextFieldName = "HealthText";
    [SerializeField] string energyTextFieldName = "EnergyText";

    // Deck Fields
    [SerializeField] string cardsInDeckTextFieldName = "CardsInDeck";

    // Objects
    StatusEffectHolder playerStatusEffects;
    StatusEffectHolder enemyStatusEffects;

    AllCards allCards;

    // Start is called before the first frame update
    void Start()
    {
        SetupConfig();
    }

    private void SetupConfig()
    {
        allCards = FindObjectOfType<AllCards>();

        halfHeight = Camera.main.orthographicSize;
        halfWidth = Camera.main.aspect * halfHeight;

        cardPlayLine = halfHeight + halfHeight / 9;
        cardSizeMultiplier = halfWidth * 0.19f;
        handMiddlePos = halfHeight / 1.48f;

        cardWidth = allCards.GetRandomCard().GetComponentInChildren<SpriteRenderer>().bounds.size.x * cardSizeMultiplier;

        float margin = halfWidth / 15.72f;
        handStartPos = Camera.main.transform.position.x - halfWidth + margin + (cardWidth / cardSizeMultiplier / 2);
        handEndPos = Camera.main.transform.position.x + halfWidth - margin - (cardWidth / cardSizeMultiplier / 2);
        handMiddlePos = halfHeight / 1.48f;

        // Tweak for narrow screens
        if (handStartPos - (Camera.main.transform.position.x - halfWidth) > halfWidth / 4)
        {
            handStartPos = Camera.main.transform.position.x - halfWidth * .55f;
            handEndPos = Camera.main.transform.position.x + halfWidth * .55f;
        }
    }

    public void SetupStatusEffectHolders()
    {
        StatusEffectHolder[] statusEffectHolders = FindObjectsOfType<StatusEffectHolder>();
        foreach(StatusEffectHolder statusEffectHolder in statusEffectHolders)
        {
            if (statusEffectHolder.IsPlayerOrEnemy() == "Player")
            {
                Debug.Log("Found player status effect holder");
                playerStatusEffects = statusEffectHolder;
            } else if (statusEffectHolder.IsPlayerOrEnemy() == "Enemy")
            {
                Debug.Log("Found enemy status effect holder");
                enemyStatusEffects = statusEffectHolder;
            }
        }
    }

    public void SetupPipManagers(CharacterData character)
    {
        PipManager[] pipManagers = FindObjectsOfType<PipManager>();

        float maxX = GameObject.Find(healthTextFieldName).transform.position.x;
        float maxWidth = maxX - pipManagers[0].transform.position.x;

        foreach (PipManager pipManager in pipManagers)
        {
            if (pipManager.name == healthPipManagerName)
            {
                healthPipManager = pipManager;
            } else if (pipManager.name == energyPipManagerName)
            {
                energyPipManager = pipManager;
            }
        }

        healthPipManager.Setup(this, character.GetMaximumHealth(), character.GetCurrentHealth());
        energyPipManager.Setup(this, character.GetMaximumEnergy(), character.GetCurrentEnergy());
    }

    public void SetupPipManagers(Enemy enemy)
    {
        PipManagerEnemy[] pipManagers = FindObjectsOfType<PipManagerEnemy>();

        float maxX = GameObject.Find(enemyHealthPipManagerName).transform.position.x;
        float maxWidth = maxX - pipManagers[0].transform.position.x;

        foreach (PipManagerEnemy pipManager in pipManagers)
        {
            if (pipManager.name == enemyHealthPipManagerName)
            {
                enemyHealthPipManager = pipManager;
            }
            else if (pipManager.name == enemyEnergyPipManagerName)
            {
                enemyEnergyPipManager = pipManager;
            }
        }

        enemyHealthPipManager.Setup(this, enemy.GetMaximumHealth(), enemy.GetCurrentHealth());
        enemyEnergyPipManager.Setup(this, enemy.GetMaximumEnergy(), enemy.GetCurrentEnergy());
    }

    public float GetCardWidth()
    {
        return cardWidth;
    }

    public float GetCardSizeMultiplier()
    {
        return cardSizeMultiplier;
    }

    public float GetHandStartPos()
    {
        return handStartPos;
    }

    public float GetHandEndPos()
    {
        return handEndPos;
    }

    public float GetHandMiddlePos()
    {
        return handMiddlePos;
    }

    public float GetHalfHeight()
    {
        return halfHeight;
    }

    public float GetHalfWidth()
    {
        return halfWidth;
    }

    public float GetHandAdjustSpeed()
    {
        return handAdjustSpeed;
    }

    public float GetHandDrawSpeed()
    {
        return handDrawSpeed;
    }

    public float MaxCardInHandAngle()
    {
        return maxCardInHandAngle;
    }

    public float GetHandMaxVerticalOffset()
    {
        return curvedHandMaxVerticalOffset;
    }

    public float GetCardPlayedLine()
    {
        return cardPlayLine;
    }

    public GameObject GetHealthTextField()
    {
        return GameObject.Find(healthTextFieldName);
    }

    public GameObject GetEnergyTextField()
    {
        return GameObject.Find(energyTextFieldName);
    }

    public GameObject GetEnemyHealthTextField()
    {
        return GameObject.Find(enemyHealthText);
    }

    public GameObject GetEnemyEnergyTextField()
    {
        return GameObject.Find(enemyEnergyText);
    }

    public float GetDistanceBetweenPips()
    {
        return distanceBetweenPips;
    }

    public int GetMaximumNumberOfPips()
    {
        return maximumNumberOfPips;
    }

    public GameObject GetCardsInDeckTextField()
    {
        return GameObject.Find(cardsInDeckTextFieldName);
    }

    public PipManagerEnemy GetEnemyHealthPipManager()
    {
        return enemyHealthPipManager;
    }

    public PipManagerEnemy GetEnemyEnergyPipManager()
    {
        return enemyEnergyPipManager;
    }

    public PipManager GetPlayerHealthPipManager()
    {
        return healthPipManager;
    }

    public PipManager GetPlayerEnergyPipManager()
    {
        return energyPipManager;
    }

    public StatusEffectHolder GetPlayerStatusEffects()
    {
        return playerStatusEffects;
    }

    public StatusEffectHolder GetEnemyStatusEffects()
    {
        return enemyStatusEffects;
    }
}
