using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCarosel : MonoBehaviour
{
    [SerializeField] CardCaroselCard cardTemplate;

    List<Card> containedCards = new List<Card>();
    List<CardCaroselCard> containedCardObjects = new List<CardCaroselCard>();

    bool showRunnerCards = true; // toggle to false to show hacker cards

    public void ClearCardList()
    {
        showRunnerCards = true;
        if (containedCardObjects.Count > 0)
        {
            for (int i = 0; i < containedCardObjects.Count; i++)
            {
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
            cardItem.SetupCard(card);
            containedCardObjects.Add(cardItem);
            Debug.Log(containedCardObjects.Count);
        }
    }

    public void ToggleHackerOrRunner()
    {
        Debug.Log("Toggle it");
        if (showRunnerCards)
        {
            ToggleToHackerCards();
        } else
        {
            ToggleToRunnerCards();
        }
        showRunnerCards = !showRunnerCards;
    }

    private void ToggleToHackerCards()
    {
        foreach(CardCaroselCard cardCaroselCard in containedCardObjects)
        {
            cardCaroselCard.ToggleToHackCard();
        }
    }

    private void ToggleToRunnerCards()
    {

    }
}
