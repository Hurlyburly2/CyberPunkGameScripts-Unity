using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHand : MonoBehaviour
{
    List<EnemyCard> cards = new List<EnemyCard>();
    [SerializeField] float timeBetweenDraws = 0.5f;

    EnemyDeck enemyDeck;
    EnemyDiscard enemyDiscard;

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
            DrawCardFromTopOfDeck(i);
        }
    }

    private void DrawCardFromTopOfDeck(int count)
    {
        EnemyCard cardToDraw = enemyDeck.DrawTopCard();
        cards.Add(cardToDraw);
        Debug.Log("Drawn card id: " + cardToDraw.GetCardId());
        Debug.Log("Cards in hand: " + cards.Count);
    }
}
