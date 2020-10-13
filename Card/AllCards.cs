using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllCards : MonoBehaviour
{
    public Card GetSampleCard()
    {
        return Resources.Load<Card>("CardPrefabs/Player/Card" + 1);
    }

    public Card GetCardById(int cardId)
    {
        Card card = Resources.Load<Card>("CardPrefabs/Player/Card" + cardId);
        return card;
    }
}
