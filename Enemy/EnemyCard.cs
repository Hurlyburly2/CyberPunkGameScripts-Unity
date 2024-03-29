﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyCard : MonoBehaviour
{
    [SerializeField] int cardId;
    [SerializeField] bool isTrap = false;
    EnemyDeck enemyDeck;
    EnemyDiscard enemyDiscard;
    TextMeshProUGUI cardText;

    public enum EnemyCardKeyword { Bio, Tech, Mech, Cyber, Virus };
    [SerializeField] List<EnemyCardKeyword> enemyCardKeywords;
    [SerializeField] AudioClip soundEffect;
        
    Sprite cardImage;
    SpriteRenderer cardBackImage;
    StatusEffectHolder playerCurrentStatusEffects;
    StatusEffectHolder enemyCurrentStatusEffects;

    string state = "hand";
    // hand, playing, played
    bool destroyOnPlay = false;

    float xPos;
    float yPos;
    float zPos;
    float handAdjustSpeed = 5f;
    string playerOrEnemy = "Enemy";
    float critModifier = 2;

    public void SetupCard(int count)
    {
        // do something with sprite layers (over everything including player hand)
        // save the card image and replace it with nothing (back of card)
        GetComponent<Animator>().Play("CardSpin");
        SpriteRenderer imageComponent = GetImageComponentByName("CardImage");

        // Rework this if more than one canvas is ever on the card
        cardText = GetComponentInChildren<TextMeshProUGUI>();
        cardText.gameObject.SetActive(false);

        cardImage = imageComponent.sprite;
        imageComponent.sprite = null;
        SetSpriteLayers(count);

        enemyDeck = FindObjectOfType<EnemyDeck>();
        enemyDiscard = FindObjectOfType<EnemyDiscard>();
        xPos = transform.position.x;
        yPos = transform.position.y;
    }

    public void SetPos(float newXPos)
    {
        xPos = newXPos;
    }

    public void SetPos(float newXPos, float newYPos)
    {
        xPos = newXPos;
        yPos = newYPos;
    }

    public void SetPos(float newXPos, float newYPos, float newZPos)
    {
        xPos = newXPos;
        yPos = newYPos;
        zPos = newZPos;
    }

    public SpriteRenderer GetImageComponentByName(string name)
    {
        SpriteRenderer[] imageComponents = GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer image in imageComponents)
        {
            if (image.name == name)
            {
                return image;
            } else if (image.name == "Background")
            {
                cardBackImage = image;
            }
        }
        throw new System.Exception("Card Image not found");
    }

    private void SetSpriteLayers(int cardCount)
    {
        SpriteRenderer[] imageComponents = GetComponentsInChildren<SpriteRenderer>();
        Canvas[] textCanvases = GetComponentsInChildren<Canvas>();
        int layerCount = 0;

        int sortType = 2000;
        if (state == "playing")
        {
            sortType = 3000;
        }

        foreach(SpriteRenderer spriteRenderer in imageComponents)
        {
            spriteRenderer.sortingOrder = sortType + (cardCount * 10) + layerCount;
            layerCount++;
        }
        foreach(Canvas canvas in textCanvases)
        {
            canvas.sortingOrder = sortType + (cardCount * 10) + layerCount;
            layerCount++;
        }
    }

    public float GetWidth(float cardInHandScale)
    {
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        return boxCollider2D.size.x * cardInHandScale;
    }

    public void PlayCard(int count)
    {
        SetSpriteLayers(count);
        GetComponent<Animator>().Play("PlayEnemyCard");
    }

    public int GetCardId()
    {
        return cardId;
    }

    public void MoveTowardTarget()
    {
        float step = handAdjustSpeed * Time.deltaTime;

        Vector3 newPosition = new Vector3(xPos, yPos, transform.position.z);

        transform.position = Vector3.MoveTowards(transform.position, newPosition, step);

        if (state == "playing")
        {
            Vector3 newScale = new Vector3(1, 1, 1);
            transform.localScale = Vector3.MoveTowards(transform.localScale, newScale, step);
            DoesCardReachTarget();
        }
    }

    private void DoesCardReachTarget()
    {
        if (transform.position.x == xPos && transform.position.y == yPos && state == "playing")
        {
            state = "played";
            PlayCardEffect();
        }
    }

    private void Update()
    {
        MoveTowardTarget();
    }

    public float GetXTarget()
    {
        return xPos;
    }

    public void SetState(string newState)
    {
        if (newState == "playing")
        {
            handAdjustSpeed = 10f;
        }
        state = newState;
    }

    public void FlipOver()
    {
        cardText.gameObject.SetActive(true);
        GetImageComponentByName("CardImage").sprite = cardImage;
    }

    private void PlayCardEffect()
    {
        if (PercentChance(FindObjectOfType<BattleData>().GetEnemyFizzleChance()))
        {
            Debug.Log("Fizzled!");
        } else
        {
            switch (cardId)
            {
                case 0:
                    Debug.LogError("This is not a real card");
                    break;
                case 1: // Ambush
                    DealDamage(5);
                    destroyOnPlay = true;
                    break;
                case 2: // Stab
                    DealDamage(2);
                    break;
                case 3: // MINOR TRAP
                    GainStatus(StatusEffect.StatusType.Vulnerable, 1);
                    SelfDamage(1);
                    AlterHandSize(1);
                    destroyOnPlay = true;
                    break;
                case 4: // Lit Fuse
                    break;
                case 5: // Burning Fuse
                    break;
                case 6: // BOOM
                    DealDamage(15);
                    SelfDamage(15);
                    break;
                case 7: // Charged Assault
                    int increment = GetIncrementor(1);
                    DealDamage(1 + increment);
                    ChangeIncrementor(1, 1);
                    break;
                case 8: // High Voltage
                    // Deal 3 damage, 6 if enemy is vulnerable
                    int damage = 3;
                    if (FindObjectOfType<ConfigData>().GetPlayerStatusEffects().GetVulnerableStacks() > 0)
                        damage = 6;
                    DealDamage(damage);
                    break;
                case 9: // Underhanded
                    InflictStatus(StatusEffect.StatusType.Vulnerable, 1);
                    break;
                case 10: // Punch
                    DealDamage(2);
                    break;
                case 11: // Brutalize
                    DealDamage(3);
                    InflictStatus(StatusEffect.StatusType.Vulnerable, 1);
                    break;
                case 12: // Reflexes
                    DealDamage(1);
                    GainStatus(StatusEffect.StatusType.Dodge, 2);
                    break;
                case 13: // Overpower
                    DealDamage(2);
                    InflictStatus(StatusEffect.StatusType.FizzleChance, 15);
                    break;
                case 14: // Riot Shield
                    GainStatus(StatusEffect.StatusType.DamageResist, 3);
                    break;
                case 15: // Shoot
                    DealDamage(2);
                    break;
                case 16: // Homing Shot
                    DealDamage(2, 50);
                    break;
                case 17: // Concussion Grenade
                    DealDamage(6);
                    destroyOnPlay = true;
                    break;
                case 18: // Call for Backup
                    GainStatus(StatusEffect.StatusType.Momentum, 1);
                    GainPermaBuff(StatusEffect.PermaBuffType.HandSize, 1);
                    break;
                case 19: // Smoke Grenade
                    GainStatus(StatusEffect.StatusType.Dodge, 4, 2);
                    destroyOnPlay = true;
                    break;
                case 20: // Crush
                    DealDamage(4);
                    break;
                case 21: // Deflection Plate
                    GainStatus(StatusEffect.StatusType.DamageResist, 2);
                    break;
                case 22: // Electrified Plate
                    GainStatus(StatusEffect.StatusType.Retaliate, 2);
                    break;
                case 23: // Flashbang
                    DealDamage(2);
                    InflictStatus(StatusEffect.StatusType.FizzleChance, 25);
                    break;
                case 24: // Recharge
                    AlterHandSize(-2);
                    Heal(10);
                    GainStatus(StatusEffect.StatusType.Vulnerable, 3);
                    break;
                default:
                    Debug.Log("Card not implemented");
                    break;
            }
        }
    }

    private void InflictStatus(StatusEffect.StatusType whichStatus, int amountOfStacks)
    {
        StatusEffectHolder playerStatusEffects = FindObjectOfType<ConfigData>().GetPlayerStatusEffects();
        playerStatusEffects.InflictStatus(whichStatus, amountOfStacks, playerOrEnemy);
    }

    private int GetIncrementor(int whichIncrementor)
    {
        return FindObjectOfType<Enemy>().GetIncrementor(1);
    }

    private void ChangeIncrementor(int whichIncrementor, int amount)
    {
        FindObjectOfType<Enemy>().AddToIncrementor(whichIncrementor, amount);
    }

    private void SelfDamage(int amount)
    {
        FindObjectOfType<Enemy>().TakeDamage(amount);
    }

    private void AlterHandSize(int buffAmount)
    {
        FindObjectOfType<EnemyHand>().AlterHandSize(buffAmount);
    }

    private void Heal(int amount)
    {
        FindObjectOfType<Enemy>().Heal(amount);
    }

    private void DealDamage(int damageAmount, int critChance = 0)
    {
        int modifiedDamage = Mathf.Clamp(CalculateModifiedDamage(damageAmount, critChance), 0, 999999);
        BattleData battleData = FindObjectOfType<BattleData>();
        CharacterData character = battleData.GetCharacter();

        if (battleData.GetPersonalShieldStacks() > 0)
            battleData.ConsumePersonalShieldStack();
        else
            character.TakeDamage(modifiedDamage);


        if (modifiedDamage > 0)
        {
            SelfDamage(FindObjectOfType<ConfigData>().GetPlayerStatusEffects().GetRetaliateStacks());
        }
    }

    private int CalculateModifiedDamage(int damageAmount, int critChance)
    {
        ConfigData configData = FindObjectOfType<ConfigData>();
        BattleData battleData = FindObjectOfType<BattleData>();
        playerCurrentStatusEffects = configData.GetPlayerStatusEffects();
        enemyCurrentStatusEffects = configData.GetEnemyStatusEffects();
        
        int dodgeChance = Mathf.Clamp(playerCurrentStatusEffects.GetDodgeChance() + FindObjectOfType<BattleData>().GetPlayerDodgeMapBuff(), 0, 80);
        if (PercentChance(dodgeChance))
        {
            Debug.Log("Dodged!");
            // Counterattack
            List<PowerUp> counterattacks = battleData.GetPowerUpsOfType(PowerUp.PowerUpType.Counterattack);
            foreach (PowerUp powerUp in counterattacks)
            {
                SelfDamage(powerUp.GetAmount());
            }
            return 0;
        }

        damageAmount += enemyCurrentStatusEffects.GetMomentumStacks();
        damageAmount += playerCurrentStatusEffects.GetVulnerableStacks();
        damageAmount -= playerCurrentStatusEffects.GetDamageResistStacks();
        damageAmount -= enemyCurrentStatusEffects.GetWeaknessStacks();

        damageAmount -= battleData.GetEnemyDamageDebuff();

        // Minimum damage = 1 is probably not necessary any more...
        //if (damageAmount < 1)
        //    damageAmount = 1;

        damageAmount -= battleData.GetPlayerDefenseBuff();

        damageAmount = CheckAndApplyCritical(damageAmount, critChance);

        if (damageAmount < 0)
        {
            return 0;
        }
        return damageAmount;
    }

    private int CheckAndApplyCritical(int damageAmount, int critChance)
    {
        // Check for crit
        bool criticalHit = false;
        if (PercentChance(critChance))
        {
            criticalHit = true;
        }
        else if (enemyCurrentStatusEffects.GetCritUpStacks() > 0)
        {
            criticalHit = true;
            GainStatus(StatusEffect.StatusType.AutoCrit, -1);
        }

        // Apply crit
        int calculatedDamage = damageAmount;
        if (criticalHit)
        {
            calculatedDamage = Mathf.FloorToInt(critModifier * calculatedDamage);
        }
        return calculatedDamage;
    }

    private void GainStatus(StatusEffect.StatusType statusType, int stacks)
    {
        ConfigData configData = FindObjectOfType<ConfigData>();
        enemyCurrentStatusEffects = configData.GetEnemyStatusEffects();
        enemyCurrentStatusEffects.InflictStatus(statusType, stacks, playerOrEnemy);
    }

    private void GainStatus(StatusEffect.StatusType statusType, int stacks, int duration)
    {
        ConfigData configData = FindObjectOfType<ConfigData>();
        enemyCurrentStatusEffects = configData.GetEnemyStatusEffects();
        enemyCurrentStatusEffects.InflictStatus(statusType, stacks, playerOrEnemy, duration);
    }

    private bool PercentChance(int percentChance)
    {
        int randomNumber = Mathf.CeilToInt(Random.Range(0, 100));
        if (randomNumber <= percentChance)
        {
            return true;
        }
        return false;
    }

    private void GainPermaBuff(StatusEffect.PermaBuffType buffType, int amount)
    {
        FindObjectOfType<Enemy>().GainPermaBuff(buffType, amount);
    }

    public void DiscardCard()
    {
        // USE THE PREFAB INSTEAD OF INSTANCE
        if (!destroyOnPlay)
        {
            EnemyCard cardPrefab = FindObjectOfType<AllEnemyCards>().GetEnemyCardById(cardId);
            EnemyDiscard enemyDiscard = FindObjectOfType<EnemyDiscard>();
            enemyDiscard.AddCardToDiscard(cardPrefab);
        }
        Destroy(gameObject);
    }

    public bool IsVirus()
    {
        if (enemyCardKeywords.Contains(EnemyCardKeyword.Virus))
            return true;
        return false;
    }

    public List<EnemyCardKeyword> GetKeywords()
    {
        return enemyCardKeywords;
    }
}
