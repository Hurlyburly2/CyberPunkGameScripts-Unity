using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardPicker : MonoBehaviour
{
    // config
    [SerializeField] DummyCard dummycard;
    [SerializeField] CardPickerCardHolder cardHolder;

    Image cardBackImage;
    float cardWidth;

    // state
    List<DummyCard> cardOptions = new List<DummyCard>();

    public void Initialize(List<Card> cards, int amountToPick)
    {
        gameObject.SetActive(true);
        SetCardSize();

        int counter = 0;
        foreach (Card card in cards)
        {
            CreateCardOption(card, counter);
            counter++;
        }
    }

    private void CreateCardOption(Card card, int counter)
    {
        float imageOffset = counter * cardWidth;
        if (counter > 0)
        {
            imageOffset += cardWidth / 4;
        }
        DummyCard newDummyCard = Instantiate(dummycard, new Vector2(transform.position.x + imageOffset, transform.position.y), Quaternion.identity);
        newDummyCard.transform.SetParent(cardHolder.transform);
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
}
