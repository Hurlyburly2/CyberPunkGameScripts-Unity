using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCard : MonoBehaviour
{
    [SerializeField] int cardId;
    EnemyDeck enemyDeck;
    EnemyDiscard enemyDiscard;

    Sprite cardImage;
    SpriteRenderer cardBackImage;

    string state = "hand";
        // hand, playing, played

    float xPos;
    float yPos;
    float zPos;
    float handAdjustSpeed = 5f;

    public void SetupCard(int count)
    {
        // do something with sprite layers (over everything including player hand)
        // save the card image and replace it with nothing (back of card)
        GetComponent<Animator>().Play("CardSpin");
        SpriteRenderer imageComponent = GetImageComponentByName("CardImage");
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
        GetImageComponentByName("CardImage").sprite = cardImage;
    }

    private void PlayCardEffect()
    {
        switch (cardId)
        {
            case 0:
                Debug.LogError("This is not a real card");
                break;
            case 1: // Ambush
                DealDamage(5);
                // Destroy this card
                break;
            case 2: // Stab
                DealDamage(2);
                break;
            default:
                Debug.Log("Card not implemented");
                break;
        }
    }

    private void DealDamage(int damageAmount)
    {
        CharacterData character = FindObjectOfType<BattleData>().GetCharacter();
        character.TakeDamage(damageAmount);
    }
}
