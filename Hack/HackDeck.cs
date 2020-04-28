using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HackDeck : MonoBehaviour
{
    List<HackCard> cards = new List<HackCard>();

    public void OnPointerDown (PointerEventData eventData)
    {
        Debug.Log("clicked");
    }

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
                    Spike currentspike = topCard.GetTopLeftSpike();
                    string position = currentspike.GetSpikePosition();
                    string state = currentspike.GetSpikeState();
                    color = currentspike.GetSpikeColor();
                    currentImage = allSpikeImages.GetSpikebyColorCornerAndState(color, position, state);
                    image.sprite = currentImage;
                    break;
                case "TopRightSpike":
                    currentspike = topCard.GetTopRightSpike();
                    position = currentspike.GetSpikePosition();
                    state = currentspike.GetSpikeState();
                    color = currentspike.GetSpikeColor();
                    currentImage = allSpikeImages.GetSpikebyColorCornerAndState(color, position, state);
                    image.sprite = currentImage;
                    break;
                case "BottomLeftSpike":
                    currentspike = topCard.GetBottomLeftSpike();
                    position = currentspike.GetSpikePosition();
                    state = currentspike.GetSpikeState();
                    color = currentspike.GetSpikeColor();
                    currentImage = allSpikeImages.GetSpikebyColorCornerAndState(color, position, state);
                    image.sprite = currentImage;
                    break;
                case "BottomRightSpike":
                    currentspike = topCard.GetbottomRightSpike();
                    position = currentspike.GetSpikePosition();
                    state = currentspike.GetSpikeState();
                    color = currentspike.GetSpikeColor();
                    currentImage = allSpikeImages.GetSpikebyColorCornerAndState(color, position, state);
                    image.sprite = currentImage;
                    break;
                case "CardImage":
                    image.sprite = topCard.GetCardImage();
                    break;
            }
        }
    }

    public void SetDeckPrefabs(List<HackCard> newHackCards)
    {
        cards = newHackCards;
    }
}
