using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DummyCard : MonoBehaviour
{
    Image selectedImage;
    [SerializeField] Sprite selectedImageSprite;
    [SerializeField] Sprite emptyImageSprite;

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
    }

    public void ToggleSelect()
    {
        if (selected)
        {
            selected = false;
            selectedImage.sprite = emptyImageSprite;
        } else
        {
            selected = true;
            selectedImage.sprite = selectedImageSprite;
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
}
