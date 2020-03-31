﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] int cardId;
    ConfigData configData;
    BattleData battleData;
    PlayerHand playerHand;
    Discard discard;

    float xPos;
    float yPos;
    float zPos;
    float rotation;
    float handAdjustSpeed = 20f;

    int rememberSortingOrder = 0;
    float rememberRotation = 0;

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

    private void DealDamage(int damageAmount)
    {
        // TODO: CALCULATE DAMAGE MODIFIERS
        battleData.GetEnemy().TakeDamage(damageAmount);
    }

    private void PlayCardActions()
    {
        switch (cardId)
        {
            case 0: // DUMMY CARD
                Debug.Log("This is a dummy card, it doesn't actually do anything");
                break;
            case 1: // AWARENESS
                Debug.Log("Awareness is not yet implemented");
                // GAIN DODGE
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
                // Remove a status effect
                break;
            case 6: // BRACE
                // Gain 1 damage resist for 2 turns
                break;
            case 7: // PUNCH
                DealDamage(2);
                // 10% CHANCE TO DRAW TWO CARDS
                break;
            case 8: // QUICKDRAW
                // Draw a random weapon card from your deck
                break;
            case 9: // KICK
                // Deal 1 damage
                // Gain 1 momentum
                break;
            case 10: // SPRINT
                // Draw 2 cards
                break;
            case 11: // WHACK
                // Deal 3 damage
                // 10% chance: critical hit
                break;
            case 12: // KNEECAP
                // Deal 4 damage
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
}
