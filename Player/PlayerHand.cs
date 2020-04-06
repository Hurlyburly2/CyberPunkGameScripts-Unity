using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    // state
    List<Card> cardsInHand = new List<Card>();
    int currentHandSize = 0;
    int initialHandSize = 0;
    float cardGap = 0;
    [SerializeField] float maxCardGap;
    bool centered = false;

    // config
    AllCards allCards;
    ConfigData configData;
    float maxWidth;
    [SerializeField] float cardWidthBuffer;
    BattleData battleData;
    Deck deck;
    Discard discard;
    CharacterData character;

    // Start is called before the first frame update
    void Start()
    {
        battleData = FindObjectOfType<BattleData>();
        deck = FindObjectOfType<Deck>();
        discard = FindObjectOfType<Discard>();
        character = FindObjectOfType<CharacterData>();

        initialHandSize = character.GetStartingHandSize();

        ConfigHand();
    }

    public void RemoveFromHand(Card cardToRemove)
    {
        cardsInHand.Remove(cardToRemove);
    }

    public void DrawStartingHand(int startingHandSize, float setupTimeInSeconds)
    {
        float timePerCardDraw = setupTimeInSeconds / startingHandSize;

        StartCoroutine(DrawStartingHandCoroutine(startingHandSize, timePerCardDraw));
    }

    private IEnumerator DrawStartingHandCoroutine(int startingHandSize, float timePerCardDraw)
    {
        for (int i = 0; i < startingHandSize; i++)
        {
            DrawCard();
            yield return new WaitForSeconds(timePerCardDraw);
        }
    }

    public void DrawXCards(int amountOfCards)
    {
        StartCoroutine(DrawXCardsCoroutine(amountOfCards));
    }

    public void DrawToMaxHandSize()
    {
        if (cardsInHand.Count < initialHandSize)
        {
            DrawXCards(initialHandSize - cardsInHand.Count);
        }
    }

    private IEnumerator DrawXCardsCoroutine(int amountOfCards)
    {
        for (int i = 0; i < amountOfCards; i++)
        {
            DrawCard();
            yield return new WaitForSeconds(.05f);
        }
    }

    public void DrawCard()
    {
        float cardSizeMultiplier = configData.GetCardSizeMultiplier();

        if (deck.GetCardCount() > 0)
        {
            Card cardToDraw = deck.DrawCardFromTop();
            Card newCard = Instantiate(cardToDraw, new Vector2(configData.GetHalfWidth() * 2.2f, 0 - (configData.GetCardWidth() / 2)), Quaternion.identity);
            newCard.SetPlayerOrEnemy("player");
            newCard.SetState("draw");
            newCard.transform.localScale = new Vector3(cardSizeMultiplier, cardSizeMultiplier, cardSizeMultiplier);
            cardsInHand.Add(newCard);

            CalculateHandPositions();
        }
    }

    public void DrawCard(Card cardToDraw)
    {
        float cardSizeMultiplier = configData.GetCardSizeMultiplier();
        Card newCard = Instantiate(cardToDraw, new Vector2(configData.GetHalfWidth() * 2.2f, 0 - (configData.GetCardWidth() / 2)), Quaternion.identity);
        newCard.SetPlayerOrEnemy("player");
        newCard.SetState("draw");
        newCard.transform.localScale = new Vector3(cardSizeMultiplier, cardSizeMultiplier, cardSizeMultiplier);
        cardsInHand.Add(newCard);

        CalculateHandPositions();
    }

    private void CalculateHandPositions()
    {
        if (cardsInHand.Count > 0)
        {
            SetCardsInHandXValues();
            CenterHand();
        }
    }

    private void SetCardsInHandXValues()
    {
        // potential division by zero
        float distanceBetweenCards = maxWidth / cardsInHand.Count;

        // if the distance between cards is too great, set it to the max gap
        if (distanceBetweenCards > maxCardGap)
        {
            distanceBetweenCards = maxCardGap;
        }

        // give each card their hand position
        float startPosition = configData.GetHandStartPos();
        int cardsPlaced = 0;

        // figure out the initial rotation and the rotation increment
        float rotationStep = 0;
        if (cardsInHand.Count > 1)
        {
            rotationStep = configData.MaxCardInHandAngle() * 2 / (cardsInHand.Count - 1);
        }
        float currentRotation = CalculateInitialRotation(rotationStep);

        float currentVerticalOffset = 0;
        float verticalOffsetIncrement = 0;
        int middleCardIndex = (int)Mathf.Ceil(cardsInHand.Count / 2);
        bool isEven = (cardsInHand.Count / 2f) % 1 == 0;
        if (cardsInHand.Count > 2)
        {
            float maxVerticalOffset = configData.GetHandMaxVerticalOffset();

            verticalOffsetIncrement = configData.GetHandMaxVerticalOffset() * 2 / (cardsInHand.Count);
            currentVerticalOffset -= maxVerticalOffset - verticalOffsetIncrement;
        }

        foreach (Card card in cardsInHand)
        {
            currentRotation -= rotationStep;

            currentVerticalOffset += verticalOffsetIncrement;
            if (isEven && cardsPlaced == middleCardIndex)
            {
                verticalOffsetIncrement *= -1;
            } else if (isEven && cardsPlaced + 1 == middleCardIndex) {
                currentVerticalOffset -= verticalOffsetIncrement;
            } else if (!isEven && cardsPlaced == middleCardIndex)
            {
                verticalOffsetIncrement *= -1;
            }

            card.SetCardPosition(startPosition + distanceBetweenCards * cardsPlaced, configData.GetHandMiddlePos() + currentVerticalOffset, currentRotation);
            card.SetZPos(cardsPlaced * -1);
            // Multiply this by 10 so each card can have 10 image sort layers
            card.SetSortingOrder(cardsPlaced * 10);
            cardsPlaced++;
        }
    }

    private void CenterHand()
    {
        // then rejigger the cards x alignment to center them in the playspace
        float firstCardLeft = cardsInHand[0].GetXPos();
        float lastCardRight = cardsInHand[cardsInHand.Count - 1].GetXPos();
        float handWidth = lastCardRight - firstCardLeft;
        float centerBuffer = (maxWidth - handWidth) / 2; ;

        foreach (Card card in cardsInHand)
        {
            card.SetCardPosition(card.GetXPos() + centerBuffer);
        }
    }

    private void ConfigHand()
    {
        allCards = FindObjectOfType<AllCards>();
        configData = FindObjectOfType<ConfigData>();
        maxCardGap = configData.GetCardWidth() + cardWidthBuffer;
        cardGap = 1f;
        maxWidth = configData.GetHandEndPos() - configData.GetHandStartPos();
    }

    private float CalculateInitialRotation(float rotationStep)
    {
        float currentRotation = configData.MaxCardInHandAngle();
        
        if (cardsInHand.Count == 1)
        {
            currentRotation = 0;
        }
        else
        {
            currentRotation += rotationStep;
        }
        return currentRotation;
    }

    public bool AreThereWeaknesses()
    {
        foreach(Card card in cardsInHand)
        {
            foreach(string keyword in card.GetKeywords())
            {
                if (keyword == "Weakness")
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void RemoveCard(Card card)
    {
        cardsInHand.Remove(card);
        CalculateHandPositions();
    }

    public int GetCardsInHandCount()
    {
        return cardsInHand.Count;
    }
}
