using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DummyCard : MonoBehaviour
{
    int id;
    Sprite circuitImage;
    Sprite cardImage;

    public void SetupDummyCard(Card card)
    {
        id = card.GetCardId();
        SetupImages(card);
    }

    private void SetupImages(Card card)
    {
        Image[] images = GetComponentsInChildren<Image>();
        foreach (Image image in images)
        {
            Debug.Log("Image.name " + image.name);
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
            }
        }
    }
}
