﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] int cardId;
    [SerializeField] string[] keywords;
    [SerializeField] CardHelperText[] cardHelperTexts;
    ConfigData configData;
    BattleData battleData;
    PlayerHand playerHand;
    StatusEffectHolder playerCurrentStatusEffects;
    StatusEffectHolder enemyCurrentStatusEffects;
    Deck deck;
    Discard discard;

    float xPos;
    float yPos;
    float zPos;
    float rotation;
    float handAdjustSpeed = 20f;

    int rememberSortingOrder = 0;
    float rememberRotation = 0;
    float critModifier = 2;

    // where is the card, and how is it behaving?
        // draw = when card is initially drawn
        // hand = drawn card reaches hand
        // dragging = mouse down, can move it from hand
        // played = card dragged to play zone (above a certain y axis)
    string state;
    string playerOrEnemy;
        // tracks if the player or enemy is using the card, used for status effects

    // Start is called before the first frame update
    void Awake()
    {
        xPos = transform.position.x;
        yPos = transform.position.y;
        configData = FindObjectOfType<ConfigData>();
        battleData = FindObjectOfType<BattleData>();
        playerHand = FindObjectOfType<PlayerHand>();
        discard = FindObjectOfType<Discard>();
        deck = FindObjectOfType<Deck>();
        playerCurrentStatusEffects = configData.GetPlayerStatusEffects();
        enemyCurrentStatusEffects = configData.GetEnemyStatusEffects();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == "draw" || state == "hand")
        {
            MoveTowardTarget(xPos, yPos);
            AdjustRotation();
            DoesDrawnCardReachHand();
        } else if (state == "dragging")
        {
            float mouseX = Input.mousePosition.x / Screen.width * configData.GetHalfWidth() * 2 / configData.GetCardSizeMultiplier();
            float mouseY = Input.mousePosition.y / Screen.height * configData.GetHalfHeight() * 2;

            MoveTowardTarget(mouseX, mouseY);
            AdjustRotation();
            DoesDrawnCardReachHand();
        }
    }

    private void OnMouseDown()
    {
        if (!battleData.AreActionsDisabled())
        {
            SetState("dragging");
        }
    }

    private void OnMouseUp()
    {
        if (state == "dragging")
        {
            RemoveHelperText();
            rotation = rememberRotation;
            SetSortingOrder(rememberSortingOrder);
            float mouseY = Input.mousePosition.y / Screen.height * configData.GetHalfHeight() * 2;
            if (mouseY > configData.GetCardPlayedLine() && battleData.WhoseTurnIsIt() == "player")
            {
                SetState("played");
                playerHand.RemoveCard(GetComponent<Card>());
                PlayCard();
            } else if (battleData.WhoseTurnIsIt() == "playerDiscard")
            {
                DiscardCard();
                playerHand.RemoveCard(GetComponent<Card>());
                battleData.EndTurn();
                Destroy(gameObject);
            } else
            {
                transform.localScale = new Vector3(1, 1, 1);
                SetState("draw");
            }
        }
    }

    public void PlayCard()
    {
        PlayCardActions();
        DiscardCard();
        playerHand.RemoveFromHand(this);
        Destroy(gameObject);
    }

    private void DiscardCard()
    {
        discard.AddCardToDiscard(this);
    }

    private void DoesDrawnCardReachHand()
    {
        if (transform.position == new Vector3(xPos, yPos, transform.position.z))
        {
            SetState("hand");
        }
    }

    private void MoveTowardTarget(float targetX, float targetY)
    {
        float step = handAdjustSpeed * Time.deltaTime;

        // we make the card movement faster as the hand size increases to fight the slowdown
        step = ScaleStepByPlayerHandSize(step);
        Vector3 newPosition = new Vector3(targetX, targetY, zPos);

        transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
    }

    private float ScaleStepByPlayerHandSize(float step)
    {
        float cardsInHandCount = playerHand.GetCardsInHandCount();

        if (cardsInHandCount < 6)
        {
            return step;
        }

        float modifier = 1;
        cardsInHandCount -= 5;
        cardsInHandCount *= Mathf.Clamp(cardsInHandCount * 0.05f, 1, 1.15f);
        modifier *= cardsInHandCount;

        return step * modifier;
    }

    private void AdjustRotation()
    {
        Quaternion target = Quaternion.Euler(0f, 0f, rotation);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * handAdjustSpeed);
    }

    public void SetSortingOrder(int newSortingOrder)
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        rememberSortingOrder = spriteRenderers[0].sortingOrder;
        int currentSortingOrder = newSortingOrder;
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.sortingOrder = currentSortingOrder;
            currentSortingOrder++;
        }
    }

    public void SetState(string state)
    {
        this.state = state;
        if (this.state == "draw")
        {
            handAdjustSpeed = configData.GetHandDrawSpeed();
        } else if (state == "hand")
        {
            handAdjustSpeed = configData.GetHandAdjustSpeed();
        } else if (state == "dragging")
        {
            transform.localScale = new Vector3(2, 2, 2);
            rememberRotation = rotation;
            rotation = 0;
            SetSortingOrder(1000);
            handAdjustSpeed = 1000f;
            StartCoroutine(DisplayHelperText());
        }
    }

    private IEnumerator DisplayHelperText()
    {
        yield return new WaitForSeconds(1);
        if (state == "dragging")
        {
            List<string> keywordsToDefine = GetKeywordsToDefine();
            for(int i = 0; i < cardHelperTexts.Length; i++)
            {
                if (i < keywordsToDefine.Count)
                {
                    cardHelperTexts[i].Activate(keywordsToDefine[i]);
                }
            }
        } else
        {
            Debug.Log("Do not display helper text");
        }
    }

    private void RemoveHelperText()
    {
        foreach(CardHelperText cardHelperText in cardHelperTexts)
        {
            cardHelperText.Deactivate();
        }
    }

    private List<string> GetKeywordsToDefine()
    {
        List<string> keywordsToDefine = new List<string>();
        foreach(string keyword in keywords)
        {
            bool needsDefinition = false;
            switch(keyword)
            {
                case "Damage Resist":
                    needsDefinition = true;
                    break;
                case "Dodge":
                    needsDefinition = true;
                    break;
                case "Momentum":
                    needsDefinition = true;
                    break;
                case "Vulnerable":
                    needsDefinition = true;
                    break;
                case "Weakness":
                    needsDefinition = true;
                    break;
            }
            if (needsDefinition)
            {
                keywordsToDefine.Add(keyword);
            }
        }
        return keywordsToDefine;
    }

    public void SetCardPosition(float xPos)
    {
        this.xPos = xPos;
    }

    public void SetCardPosition(float xPos, float yPos)
    {
        this.xPos = xPos;
        this.yPos = yPos;
    }

    public void SetCardPosition(float xPos, float yPos, float rotation)
    {
        this.xPos = xPos;
        this.yPos = yPos;
        this.rotation = rotation;
    }

    public float GetXPos()
    {
        return xPos;
    }

    public void SetZPos(float zPos)
    {
        this.zPos = zPos;
    }

    public int GetCardId()
    {
        return cardId;
    }

    public string[] GetKeywords()
    {
        return keywords;
    }

    // PLAY CARD ACTIONS

    private void PlayCardActions()
    {
        switch (cardId)
        {
            case 0: // DUMMY CARD
                Debug.Log("This is a dummy card, it doesn't actually do anything");
                break;
            case 1: // AWARENESS
                GainStatus("Dodge", 1);
                break;
            case 2: // OBSERVE
                // Pick and draw one of the top three cards of your deck, discard the other two
                break;
            case 3: // DEEP BREATH
                // DRAW 5
                // GAIN VULNERABLE
                // END YOUR TURN AND SKIP YOUR DISCARD
                break;
            case 4: // WEAK SPOT
                InflictStatus("CritUp", 1);
                break;
            case 5: // SHAKE OFF
                // Heal 2
                // Remove a debuff effect
                break;
            case 6: // BRACE
                GainStatus("Damage Resist", 1, 2);
                break;
            case 7: // PUNCH
                DealDamage(2);
                if (PercentChance(10)) {
                    DrawXCards(1);
                }
                break;
            case 8: // QUICKDRAW
                DrawRandomCardFromDeck("Weapon");
                break;
            case 9: // KICK
                DealDamage(1);
                GainStatus("Momentum", 1);
                break;
            case 10: // SPRINT
                DrawXCards(2);
                break;
            case 11: // WHACK
                DealDamage(3, 10);
                break;
            case 12: // KNEECAP
                DealDamage(4);
                break;
            case 13: // BRUISE
                DealDamage(1);
                InflictStatus("Vulnerable", 2);
                break;
            default:
                Debug.Log("That card doesn't exist or doesn't have any actions on it built yet");
                break;
        }
    }

    private void GainStatus(string statusType, int stacks)
    {
        playerCurrentStatusEffects.InflictStatus(statusType, stacks, playerOrEnemy);
    }

    private void GainStatus(string statusType, int stacks, int duration)
    {
        playerCurrentStatusEffects.InflictStatus(statusType, stacks, playerOrEnemy, duration);
    }

    private void InflictStatus(string statusType, int stacks)
    {
        enemyCurrentStatusEffects.InflictStatus(statusType, stacks, playerOrEnemy);
    }

    private void DealDamage(int damageAmount, int critChance = 0)
    {
        // Modify Damage
        damageAmount += playerCurrentStatusEffects.GetMomentumStacks();
        damageAmount += enemyCurrentStatusEffects.GetVulnerableStacks();

        int modifiedDamage = CheckAndApplyCritical(damageAmount, critChance);
        battleData.GetEnemy().TakeDamage(modifiedDamage);
    }

    private int CheckAndApplyCritical(int damageAmount, int critChance)
    {
        // Check for crit
        bool criticalHit = false;
        if (PercentChance(critChance))
        {
            criticalHit = true;
        }
        else if (playerCurrentStatusEffects.GetCritUpStacks() > 0)
        {
            criticalHit = true;
            GainStatus("CritUp", -1);
        }

        // Apply crit
        int calculatedDamage = damageAmount;
        if (criticalHit)
        {
            calculatedDamage = Mathf.FloorToInt(critModifier * calculatedDamage);
        }
        return calculatedDamage;
    }

    private void DrawXCards(int amountToDraw)
    {
        playerHand.DrawXCards(amountToDraw);
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

    private void DrawRandomCardFromDeck(string keyword)
    {
        Card cardToDraw = deck.DrawRandomCardFromDeck(keyword);
        if (cardToDraw.GetCardId() == 0)
        {
            return;
        }
        playerHand.DrawCard(cardToDraw);
    }

    public string GetPlayerOrEnemy()
    {
        return playerOrEnemy;
    }

    public void SetPlayerOrEnemy(string newPlayerOrEnemy)
    {
        playerOrEnemy = newPlayerOrEnemy;
    }
}