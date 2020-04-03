using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] int cardId;
    [SerializeField] string[] keywords;
    ConfigData configData;
    BattleData battleData;
    PlayerHand playerHand;
    StatusEffectHolder playerCurrentStatusEffects;
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
            rotation = rememberRotation;
            SetSortingOrder(rememberSortingOrder);
            float mouseY = Input.mousePosition.y / Screen.height * configData.GetHalfHeight() * 2;
            if (mouseY > configData.GetCardPlayedLine())
            {
                SetState("played");
                playerHand.RemoveCard(GetComponent<Card>());
                PlayCard();
            }
            else
            {
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
            rememberRotation = rotation;
            rotation = 0;
            SetSortingOrder(1000);
            handAdjustSpeed = 1000f;
        }
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
                // The next damage you take is doubled
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
                DealDamage(1, 0);
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
                // Deal 1 damage
                // Inflict vulnerable x2
                break;
            default:
                Debug.Log("That card doesn't exist or doesn't have any actions on it built yet");
                break;
        }
    }

    private void GainStatus(string statusType, int stacks)
    {
        playerCurrentStatusEffects.InflictStatus(statusType, stacks);
    }

    private void GainStatus(string statusType, int stacks, int duration)
    {
        playerCurrentStatusEffects.InflictStatus(statusType, stacks, duration);
    }

    private void DealDamage(int damageAmount, int critChance = 0)
    {
        damageAmount += playerCurrentStatusEffects.GetMomentumStacks();
        bool criticalHit = false;
        if (PercentChance(critChance))
        {
            criticalHit = true;
        }
        int modifiedDamage = ApplyDamageModifiers(damageAmount, criticalHit);
        battleData.GetEnemy().TakeDamage(modifiedDamage);
    }

    private int ApplyDamageModifiers(int damageAmount, bool criticalHit)
    {
        // TODO: CALCULATE DAMAGE MODIFIERS
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
}
