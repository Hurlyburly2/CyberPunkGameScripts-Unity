using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCarosel : MonoBehaviour
{
    [SerializeField] CardCaroselCard cardTemplate;

    List<Card> containedCards = new List<Card>();
    List<CardCaroselCard> containedCardObjects = new List<CardCaroselCard>();

    public void ClearCardList()
    {
        if (containedCardObjects.Count > 0)
        {
            Debug.Log("for loop: ");
            for (int i = 0; i < containedCardObjects.Count; i++)
            {
                Debug.Log("for loop: " + i);
                Destroy(containedCardObjects[i].gameObject);
            }
        }

        containedCards = new List<Card>();
        containedCardObjects = new List<CardCaroselCard>();
    }

    public void AddCardToList(Card newCard)
    {
        containedCards.Add(newCard);
    }

    public void GenerateListItems()
    {
        foreach (Card card in containedCards)
        {
            CardCaroselCard cardItem = Instantiate(cardTemplate);
            cardItem.gameObject.SetActive(true);

            cardItem.transform.SetParent(cardTemplate.transform.parent, false);
            containedCardObjects.Add(cardItem);
            Debug.Log(containedCardObjects.Count);
        }
    }
}
