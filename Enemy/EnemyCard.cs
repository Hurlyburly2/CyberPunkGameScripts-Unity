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
        foreach(SpriteRenderer spriteRenderer in imageComponents)
        {
            spriteRenderer.sortingOrder = 2000 + (cardCount * 10) + layerCount;
            layerCount++;
        }
    }

    public float GetWidth(float cardInHandScale)
    {
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        return boxCollider2D.size.x * cardInHandScale;
    }

    public void PlayCard()
    {
        // 'flip' the card over to show the card image
        switch (cardId)
        {
            default:
                Debug.Log("Card not implemented");
                break;
        }
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
    }

    private void DoesCardReachTarget()
    {
    }

    private void Update()
    {
        MoveTowardTarget();
    }

    public float GetXTarget()
    {
        return xPos;
    }
}
