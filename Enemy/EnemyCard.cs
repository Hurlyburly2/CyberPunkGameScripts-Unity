using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCard : MonoBehaviour
{
    [SerializeField] int cardId;
    EnemyDeck enemyDeck;
    EnemyDiscard enemyDiscard;

    public void SetupCard()
    {
        // do something with sprite layers (over everything including player hand)
        // save the card image and replace it with nothing (back of card)
        enemyDeck = FindObjectOfType<EnemyDeck>();
        enemyDiscard = FindObjectOfType<EnemyDiscard>();
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
}
