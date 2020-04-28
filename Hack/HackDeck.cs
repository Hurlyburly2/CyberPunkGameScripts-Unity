using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HackDeck : MonoBehaviour
{
    List<HackCard> cards = new List<HackCard>();

    public void SetTopCard()
    {
        Image[] imageHolders = GetComponentsInChildren<Image>();
        AllSpikeImages allSpikeImages = FindObjectOfType<AllSpikeImages>();
        HackCard topCard = cards[0];
        foreach(Image image in imageHolders)
        {
            switch(image.name)
            {
                case "CardBack":
                    // This one should never change (maybe status stuff later though???)
                    break;
                case "LeftCircuit":
                    string color = topCard.GetLeftCircuit();
                    Sprite currentImage = allSpikeImages.GetCircuitImageByColorAndDirection(color, "left");
                    image.sprite = currentImage;
                    break;
                case "TopCircuit":
                    color = topCard.GetTopCircuit();
                    currentImage = allSpikeImages.GetCircuitImageByColorAndDirection(color, "top");
                    image.sprite = currentImage;
                    break;
                case "RightCircuit":
                    color = topCard.GetRightCircuit();
                    currentImage = allSpikeImages.GetCircuitImageByColorAndDirection(color, "right");
                    image.sprite = currentImage;
                    break;
                case "DownCircuit":
                    color = topCard.GetBottomCircuit();
                    currentImage = allSpikeImages.GetCircuitImageByColorAndDirection(color, "bottom");
                    image.sprite = currentImage;
                    break;
                case "TopLeftSpike":
                    break;
                case "TopRightSpike":
                    break;
                case "BottomLeftSpike":
                    break;
                case "BottomRightSpike":
                    break;
                case "CardImage":
                    break;
            }
        }
    }

    public void SetDeckPrefabs(List<HackCard> newHackCards)
    {
        cards = newHackCards;
    }
}
