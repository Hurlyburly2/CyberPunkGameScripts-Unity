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
        enemyDeck = FindObjectOfType<EnemyDeck>();
        enemyDiscard = FindObjectOfType<EnemyDiscard>();
    }

    public void PlayCard()
    {
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
