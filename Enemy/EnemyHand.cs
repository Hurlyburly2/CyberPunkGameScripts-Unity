using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHand : MonoBehaviour
{
    List<EnemyCard> cards = new List<EnemyCard>();
    float timeBetweenDraws = 0.2f;

    EnemyDeck enemyDeck;
    EnemyDiscard enemyDiscard;
    float cardInHandScale = 0.5f;

    float cardWidth;

    public void DrawInitialHand(int handSize)
    {
        enemyDeck = FindObjectOfType<EnemyDeck>();
        enemyDiscard = FindObjectOfType<EnemyDiscard>();
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

        CenterHand();
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
}
