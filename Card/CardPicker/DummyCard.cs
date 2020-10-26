using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DummyCard : MonoBehaviour
{
    Image selectedImage;
    [SerializeField] Sprite selectedImageSprite;
    [SerializeField] Sprite emptyImageSprite;
    [SerializeField] TextMeshProUGUI cardText;
    [SerializeField] GameObject energyCost;

    // config
    int id;
    Sprite circuitImage;
    Sprite cardImage;

    // state
    bool selected = false;

    public void SetupDummyCard(Card card)
    {
        selected = false;
        id = card.GetCardId();
        SetupImages(card);
        cardText.text = card.GetCardText();
        if (card.GetEnergyCost() > 0)
        {
            energyCost.SetActive(true);
            energyCost.GetComponentInChildren<TextMeshProUGUI>().text = card.GetEnergyCost().ToString();
        } else
        {
            energyCost.SetActive(false);
        }
    }

    public void ToggleSelect()
    {
        CardPicker cardPicker = FindObjectOfType<CardPicker>();
        if (selected)
        {
            selected = false;
            selectedImage.sprite = emptyImageSprite;
            cardPicker.UnSelectOne();
        } else
        {
            if (cardPicker.CanSelectMore())
            {
                selected = true;
                selectedImage.sprite = selectedImageSprite;
                cardPicker.SelectOne();
            }
        }
    }

    private void SetupImages(Card card)
    {
        Image[] images = GetComponentsInChildren<Image>();
        foreach (Image image in images)
        {
            switch (image.name)
            {
                case "CircuitImage":
                    circuitImage = card.GetImageByGameobjectName("CircuitImage");
                    image.sprite = circuitImage;
                    break;
                case "FrontImage":
                    cardImage = card.GetImageByGameobjectName("Image");
                    image.sprite = cardImage;
                    break;
                case "Selected":
                    selectedImage = image;
                    break;
            }
        }
    }

    public bool IsSelected()
    {
        return selected;
    }

    public int GetCardId()
    {
        return id;
    }

    public void DestroyDummyCard()
    {
        Destroy(gameObject);
    }
}
