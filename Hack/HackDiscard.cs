using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HackDiscard : MonoBehaviour
{
    List<HackCard> cards = new List<HackCard>();

    private void Awake()
    {
        SetTopCard();
    }

    public void AddCardToDiscard(HackCard card)
    {
        cards.Add(card);
        SetTopCard();
    }

    public void SetTopCard()
    {
        Image[] imageHolders = GetComponentsInChildren<Image>();
        AllSpikeImages allSpikeImages = FindObjectOfType<AllSpikeImages>();

        if (cards.Count == 0)
        {
            foreach(Image image in imageHolders)
            {
                image.sprite = allSpikeImages.GetEmptyImage();
            }
        } else
        {
            HackCard topCard = cards[cards.Count - 1];
            foreach (Image image in imageHolders)
            {
                switch (image.name)
                {
                    case "HackDiscardCardBack":
                        image.sprite = allSpikeImages.GetCardBack();
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
                        Spike currentspike = topCard.GetTopLeftSpike();
                        string state = currentspike.GetSpikeState();
                        color = currentspike.GetSpikeColor();
                        currentImage = allSpikeImages.GetSpikebyColorCornerAndState(color, "topleft", state);
                        image.sprite = currentImage;
                        break;
                    case "TopRightSpike":
                        currentspike = topCard.GetTopRightSpike();
                        state = currentspike.GetSpikeState();
                        color = currentspike.GetSpikeColor();
                        currentImage = allSpikeImages.GetSpikebyColorCornerAndState(color, "topright", state);
                        image.sprite = currentImage;
                        break;
                    case "BottomLeftSpike":
                        currentspike = topCard.GetBottomLeftSpike();
                        state = currentspike.GetSpikeState();
                        color = currentspike.GetSpikeColor();
                        currentImage = allSpikeImages.GetSpikebyColorCornerAndState(color, "bottomleft", state);
                        image.sprite = currentImage;
                        break;
                    case "BottomRightSpike":
                        currentspike = topCard.GetbottomRightSpike();
                        state = currentspike.GetSpikeState();
                        color = currentspike.GetSpikeColor();
                        currentImage = allSpikeImages.GetSpikebyColorCornerAndState(color, "bottomright", state);
                        image.sprite = currentImage;
                        break;
                    case "CardImage":
                        image.sprite = topCard.GetCardImage();
                        break;
                }
            }
        }
    }
}
