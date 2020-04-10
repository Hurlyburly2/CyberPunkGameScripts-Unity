using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDiscard : MonoBehaviour
{
    List<EnemyCard> cards = new List<EnemyCard>();
    EnemyDeck enemyDeck;

    public void ShuffleDiscardIntoDeck()
    {
        enemyDeck = FindObjectOfType<EnemyDeck>();
        enemyDeck.AddCardToDeck(cards);
        enemyDeck.ShuffleDeck();
        cards.Clear();
    }

    public int GetDiscardCount()
    {
        return cards.Count;
    }
}
