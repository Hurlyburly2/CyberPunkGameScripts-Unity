using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardPicker : MonoBehaviour
{
    // config
    [SerializeField] DummyCard dummycard;
    [SerializeField] CardPickerCardHolder cardHolder;
    [SerializeField] TextMeshProUGUI selectTextField;
    [SerializeField] Button finishSelectionButton;

    Image cardBackImage;
    float cardWidth;
    float contentWidth;

    // state
    List<DummyCard> cardOptions = new List<DummyCard>();
    List<Card> actualCardObjects;

    string type;
    // SelectToHand, DiscardCardsFromHand
    int amountToPick;
    int amountPicked;

    public void Initialize(List<Card> cards, int newAmountToPick, string newTypeOfPicker)
    {
        actualCardObjects = cards;
        contentWidth = 0;
        gameObject.SetActive(true);
        SetCardSize();

        type = newTypeOfPicker;
        amountToPick = newAmountToPick;
        amountPicked = 0;
        UpdateText();

        int counter = 0;
        foreach (Card card in cards)
        {
            CreateCardOption(card, counter);
            counter++;
        }

        HorizontalLayoutGroup horizontalLayoutGroup = cardHolder.GetComponent<HorizontalLayoutGroup>();
        horizontalLayoutGroup.spacing = cardWidth * 1.5f;
        horizontalLayoutGroup.padding.left += Mathf.CeilToInt(cardWidth);
        horizontalLayoutGroup.padding.right += Mathf.CeilToInt(cardWidth);
    }

    private void CreateCardOption(Card card, int counter)
    {
        float imageOffset = cardWidth;
        contentWidth += cardWidth;
        if (counter > 0)
        {
            imageOffset += cardWidth / 4;
            contentWidth += cardWidth / 4;
        }
        imageOffset *= counter;
        DummyCard newDummyCard = Instantiate(dummycard, new Vector2(transform.position.x + imageOffset, transform.position.y), Quaternion.identity);
        newDummyCard.transform.SetParent(cardHolder.transform);
        newDummyCard.SetupDummyCard(card);

        cardOptions.Add(newDummyCard);
    }

    private void SetCardSize()
    {
        Image[] images = dummycard.GetComponentsInChildren<Image>();
        foreach (Image image in images)
        {
            if (image.name == "BackImage")
            {
                RectTransform rt = (RectTransform)image.transform;
                cardWidth = rt.rect.width;
            }
        }
    }

    public void SelectOne()
    {
        amountPicked++;
        UpdateText();
        SelectedMax();
    }

    public void UnSelectOne()
    {
        amountPicked--;
        UpdateText();
        SelectedMax();
    }

    private void UpdateText()
    {
        string textFieldText = "";
        switch(type)
        {
            case "SelectToHandFromDeckAndDiscardOthers":
                textFieldText = "Select " + (amountToPick - amountPicked) + " cards to add to your hand:";
                break;
            case "DiscardCardsFromHand":
                textFieldText = "Discard " + (amountToPick - amountPicked) + " cards from your hand:";
                break;
        }
        selectTextField.text = textFieldText;
    }

    public bool CanSelectMore()
    {
        if (amountToPick - amountPicked > 0)
        {
            return true;
        }
        return false;
    }

    private void SelectedMax()
    {
        if (amountPicked >= amountToPick)
        {
            finishSelectionButton.interactable = true;
        } else
        {
            finishSelectionButton.interactable = false;
        }
    }

    public void FinishSelection()
    {
        switch(type)
        {
            case "SelectToHandFromDeckAndDiscardOthers":
                SelectToHandFromDeckAndDiscardOthers();
                break;
            case "DiscardCardsFromHand":
                DiscardCardsFromHand();
                break;
        }
    }

    private void DiscardCardsFromHand()
    {
        List<int> selectedCardIds = GetSelectedCards();
        List<Card> selectedCardObjects = GetCardsListById(selectedCardIds);

        foreach(Card card in selectedCardObjects)
        {
            card.DiscardFromHand();
        }
        FindObjectOfType<BattleData>().EndTurn();
        ShutOff();
    }

    private void SelectToHandFromDeckAndDiscardOthers()
    {
        // Add selection of cards from deck to hand, add the rest to discard
        List<int> selectedCardIds = GetSelectedCards();
        List<int> notSelectedCardIds = GetNotSelectedCards();

        List<Card> selectedCardObjects = GetCardsListById(selectedCardIds);
        List<Card> notSelectedCardObjects = GetCardsListById(notSelectedCardIds);

        DrawCardsFromDeck(selectedCardObjects);
        DiscardCardsFromDeck(notSelectedCardObjects);
        ShutOff();
    }

    private void DrawCardsFromDeck(List<Card> cardsToDraw)
    {
        PlayerHand playerHand = FindObjectOfType<PlayerHand>();
        foreach (Card card in cardsToDraw)
        {
            playerHand.DrawSpecificCard(card);
        }
    }

    private void DiscardCardsFromDeck(List<Card> cardsToDiscard)
    {
        Deck deck = FindObjectOfType<Deck>();
        foreach(Card card in cardsToDiscard)
        {
            deck.DiscardSpecificCard(card);
        }
    }

    private List<Card> GetCardsListById(List<int> cardIds)
    {
        List<Card> foundCards = new List<Card>();
        foreach(Card card in actualCardObjects)
        {
            if (cardIds.Contains(card.GetCardId()))
            {
                foundCards.Add(card);
                cardIds.Remove(card.GetCardId());
            }
        }
        return foundCards;
    }

    private List<int> GetSelectedCards()
    {
        List<int> selectedCardIds = new List<int>();
        foreach (DummyCard dummyCard in cardOptions)
        {
            if (dummyCard.IsSelected())
            {
                selectedCardIds.Add(dummyCard.GetCardId());
            }
        }
        return selectedCardIds;
    }

    private List<int> GetNotSelectedCards()
    {
        List<int> notSelectedCardIds = new List<int>();
        foreach (DummyCard dummyCard in cardOptions)
        {
            if (!dummyCard.IsSelected())
            {
                notSelectedCardIds.Add(dummyCard.GetCardId());
            }
        }
        return notSelectedCardIds;
    }

    private void ShutOff()
    {
        gameObject.SetActive(false);
    }
}
