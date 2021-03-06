﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHand : MonoBehaviour
{
    [SerializeField] GameObject playPosition;

    List<EnemyCard> cards = new List<EnemyCard>();
    float timeBetweenDraws = 0.2f;

    EnemyDeck enemyDeck;
    EnemyDiscard enemyDiscard;
    float cardInHandScale = 0.5f;

    float cardWidth;
    bool playingCards = false;

    int handBuff = 0;

    public void DrawInitialHand(int handSize)
    {
        handSize += handBuff;
        handSize += FindObjectOfType<Enemy>().GetPermaBuffAmount(StatusEffect.PermaBuffType.HandSize);
        // minimum hand size is 1, debuffs shouldn't prevent them from every playing anything
        handSize = Mathf.Clamp(handSize - FindObjectOfType<BattleData>().GetEnemyHandSizeDebuff(), 1, 999999);
        handBuff = 0;

        playingCards = false;
        enemyDeck = FindObjectOfType<EnemyDeck>();
        enemyDiscard = FindObjectOfType<EnemyDiscard>();
        Debug.Log("enemy handsize: " + handSize);
        StartCoroutine(DrawHandTimer(handSize));
    }

    private IEnumerator DrawHandTimer(int amountToDraw)
    {
        for (int i = 0; i < amountToDraw; i++)
        {
            yield return new WaitForSeconds(timeBetweenDraws);
            if (enemyDeck.GetCardsInDeckCount() > 0 || enemyDiscard.GetDiscardCount() > 0)
            {
                // Only draw a card if there are cards in the deck or discard
                DrawCardFromTopOfDeck(i);
            }
        }
        CenterHand();
        StartCoroutine(WaitForASec("DrawToPlay"));
    }

    private IEnumerator WaitForASec(string state)
    {
        switch(state)
        {
            case "DrawToPlay":
                yield return new WaitForSeconds(0.75f);
                FindObjectOfType<Enemy>().PlayCards();
                break;
            default:
                Debug.LogError("State doesn't exist");
                break;
        }
    }

    public void AlterHandSize(int amountToBuff)
    {
        handBuff += amountToBuff;
    }

    private void DrawCardFromTopOfDeck(int count)
    {
        // Shrink the card when its in enemy hand and redraw it later
        // Rejigger the cards so they're offset and centered
        EnemyCard cardToDraw = enemyDeck.DrawTopCard();
        EnemyCard instantiatedCard = Instantiate(cardToDraw, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        instantiatedCard.transform.SetParent(this.transform);
        instantiatedCard.transform.localScale = new Vector3(cardInHandScale, cardInHandScale, cardInHandScale);

        instantiatedCard.SetupCard(count);

        cardWidth = instantiatedCard.GetWidth(cardInHandScale);
        float offset = cardWidth * count / 2;
        instantiatedCard.transform.position = new Vector2(instantiatedCard.transform.position.x + offset, instantiatedCard.transform.position.y);
        instantiatedCard.SetPos(instantiatedCard.transform.position.x + offset, transform.position.y, count * -1);
        cards.Add(instantiatedCard);

        if (!playingCards)
        {
            CenterHand();
        }
    }

    private void CenterHand()
    {
        float totalWidth = ((cards.Count - 1) * cardWidth / 2) + cardWidth;
        float startPosition = transform.position.x - totalWidth / 2;

        int count = 1;
        foreach (EnemyCard card in cards)
        {
            card.SetPos(startPosition + (cardWidth * count / 2));
            count++;
        }
    }

    public void PlayAllCards()
    {
        cardWidth = cards[0].GetWidth(1);
        StartCoroutine(PlayCardsTimer(1));
    }

    private IEnumerator PlayCardsTimer(float timeBetweenPlays)
    {
        int count = 0;
        int row = 0;
        int column = 0;
        playingCards = true;
        for (int i = 0; i < cards.Count; i++)
        {
            SetPlayPosition(cards[i], count, row, column);
            cards[i].PlayCard(count);
            yield return new WaitForSeconds(timeBetweenPlays);
            count++;
            column++;
            if (count > 0 && count % 3 == 0)
            {
                row++;
            }
            if (column > 2)
            {
                column = 0;
            }
        }
        playingCards = false;
        FindObjectOfType<Enemy>().FinishTurn();
    }

    private void SetPlayPosition(EnemyCard card, int count, int row, int column)
    {
        card.SetState("playing");
        float xOffset = cardWidth / 2 * column;
        float yOffset = cardWidth / 8 * count;
        card.SetPos(playPosition.transform.position.x + xOffset, playPosition.transform.position.y - yOffset);
    }

    public void ClearPlayedCards()
    {
        StartCoroutine(ClearPlayedCardsTimer());
    }

    private IEnumerator ClearPlayedCardsTimer()
    {
        foreach (EnemyCard card in cards)
        {
            card.GetComponent<Animator>().Play("CardSpinAway");
            yield return new WaitForSeconds(0.3f);
        }
        cards.Clear();
        FindObjectOfType<BattleData>().EndTurn();
    }

    public void RemoveCard(EnemyCard card)
    {
        cards.Remove(card);
        Debug.Log("Hand Size: " + cards.Count);
    }

    public bool GetIsPlaying()
    {
        return playingCards;
    }
}
