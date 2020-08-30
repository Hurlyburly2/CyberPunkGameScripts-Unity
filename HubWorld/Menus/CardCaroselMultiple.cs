using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardCaroselMultiple : MonoBehaviour
{
    [SerializeField] CardCaroselCard lvl1CardTemplate;
    [SerializeField] CardCaroselCard lvl2CardTemplate;
    [SerializeField] CardCaroselCard lvl3CardTemplate;
    [SerializeField] CardCaroselCard lvl4CardTemplate;
    [SerializeField] CardCaroselCard lvl5CardTemplate;

    [SerializeField] Image rightButton;
    [SerializeField] Image leftButton;
    [SerializeField] Image battleTextImg;
    [SerializeField] Image hackTextImg;

    [SerializeField] Sprite buttonOn;
    [SerializeField] Sprite buttonOff;

    List<Card> lvl1containedCards = new List<Card>();
    List<CardCaroselCard> lvl1containedCardObjects = new List<CardCaroselCard>();
    List<Card> lvl2containedCards = new List<Card>();
    List<CardCaroselCard> lvl2containedCardObjects = new List<CardCaroselCard>();
    List<Card> lvl3containedCards = new List<Card>();
    List<CardCaroselCard> lvl3containedCardObjects = new List<CardCaroselCard>();
    List<Card> lvl4containedCards = new List<Card>();
    List<CardCaroselCard> lvl4containedCardObjects = new List<CardCaroselCard>();
    List<Card> lvl5containedCards = new List<Card>();
    List<CardCaroselCard> lvl5containedCardObjects = new List<CardCaroselCard>();

    bool showRunnerCards = true; // toggle to false to show hacker cards

    public void InitializeToggle()
    {
        HorizontalLayoutGroup[] horizontalLayoutGroups = GetComponentsInChildren<HorizontalLayoutGroup>();
        foreach (HorizontalLayoutGroup group in horizontalLayoutGroups)
        {
            group.spacing = 50;
        }

        rightButton.sprite = buttonOn;
        leftButton.sprite = buttonOff;
        battleTextImg.gameObject.SetActive(true);
        hackTextImg.gameObject.SetActive(false);
    }

    public void ClearCardLists()
    {
        showRunnerCards = true;
        if (lvl1containedCardObjects.Count > 0)
        {
            for (int i = 0; i < lvl1containedCardObjects.Count; i++)
            {
                Destroy(lvl1containedCardObjects[i].gameObject);
            }
        }

        if (lvl2containedCardObjects.Count > 0)
        {
            for (int i = 0; i < lvl2containedCardObjects.Count; i++)
            {
                Destroy(lvl2containedCardObjects[i].gameObject);
            }
        }

        if (lvl3containedCardObjects.Count > 0)
        {
            for (int i = 0; i < lvl3containedCardObjects.Count; i++)
            {
                Destroy(lvl3containedCardObjects[i].gameObject);
            }
        }

        if (lvl4containedCardObjects.Count > 0)
        {
            for (int i = 0; i < lvl4containedCardObjects.Count; i++)
            {
                Destroy(lvl4containedCardObjects[i].gameObject);
            }
        }

        if (lvl5containedCardObjects.Count > 0)
        {
            for (int i = 0; i < lvl5containedCardObjects.Count; i++)
            {
                Destroy(lvl5containedCardObjects[i].gameObject);
            }
        }

        lvl1containedCards = new List<Card>();
        lvl1containedCardObjects = new List<CardCaroselCard>();
    }

    public void AddCardToList(Card newCard)
    {
        lvl1containedCards.Add(newCard);
    }

    public void GenerateListItems()
    {
        foreach (Card card in lvl1containedCards)
        {
            CardCaroselCard cardItem = Instantiate(lvl1CardTemplate);
            cardItem.gameObject.SetActive(true);

            cardItem.transform.SetParent(lvl1CardTemplate.transform.parent, false);
            cardItem.SetupCard(card);
            lvl1containedCardObjects.Add(cardItem);
            Debug.Log(lvl1containedCardObjects.Count);
        }
    }

    public void ToggleHackerOrRunner()
    {
        if (showRunnerCards)
        {
            ToggleToHackerCards();
        }
        else
        {
            ToggleToRunnerCards();
        }
        showRunnerCards = !showRunnerCards;
    }

    private void ToggleToHackerCards()
    {
        rightButton.sprite = buttonOff;
        leftButton.sprite = buttonOn;
        battleTextImg.gameObject.SetActive(false);
        hackTextImg.gameObject.SetActive(true);
        foreach (CardCaroselCard cardCaroselCard in lvl1containedCardObjects)
        {
            cardCaroselCard.ToggleToHackCard();
        }
    }

    private void ToggleToRunnerCards()
    {
        rightButton.sprite = buttonOn;
        leftButton.sprite = buttonOff;
        battleTextImg.gameObject.SetActive(true);
        hackTextImg.gameObject.SetActive(false);
        foreach (CardCaroselCard cardCaroselCard in lvl1containedCardObjects)
        {
            cardCaroselCard.ToggleToRunnerCard();
        }
    }
}
