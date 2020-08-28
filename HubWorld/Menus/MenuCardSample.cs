using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuCardSample : MonoBehaviour
{
    Card card;
    HackCard hackCard;

    // Runner Card Stuff
    [SerializeField] GameObject runnerCard;
    [SerializeField] Image cardImage;
    [SerializeField] Image leftCircuit;
    [SerializeField] Image topCircuit;
    [SerializeField] Image rightCircuit;
    [SerializeField] Image bottomCircuit;
    [SerializeField] TextMeshProUGUI cardText;

    // Hacker Card Stuff
    [SerializeField] GameObject hackerCard;
    [SerializeField] Image hackerCardImage;
    [SerializeField] Image hackerCardLeftCircuit;
    [SerializeField] Image hackerCardTopCircuit;
    [SerializeField] Image hackerCardRightCircuit;
    [SerializeField] Image hackerCardBottomCircuit;
    [SerializeField] Image topLeftSpike;
    [SerializeField] Image topRightSpike;
    [SerializeField] Image bottomLeftSpike;
    [SerializeField] Image bottomRightSpike;

    public void SetupCardSample(Card newCard)
    {
        runnerCard.SetActive(true);
        hackerCard.SetActive(false);
        card = newCard;

        string paddedCardId = "" + card.GetCardId().ToString("000");
        string imagePath = "CardParts/CardImages/CardImage" + paddedCardId;
        cardImage.sprite = Resources.Load<Sprite>(imagePath);
        leftCircuit.sprite = card.GetLeftCircuitImage();
        topCircuit.sprite = card.GetTopCircuitImage();
        rightCircuit.sprite = card.GetRightCircuitImage();
        bottomCircuit.sprite = card.GetBottomCircuitImage();
        cardText.text = card.GetCardText();
    }

    public void SetupCardSample(
        HackCard newHackCard,
        Card newCard,
        Sprite topLeftSpikeImage,
        Sprite topRightSpikeImage,
        Sprite bottomLeftSpikeImage,
        Sprite bottomRightSpikeImage
    )
    {
        runnerCard.SetActive(false);
        hackerCard.SetActive(true);

        hackCard = newHackCard;
        card = newCard;
        hackerCardImage.sprite = hackCard.GetCardImage();
        hackerCardLeftCircuit.sprite = card.GetLeftCircuitImage();
        hackerCardTopCircuit.sprite = card.GetTopCircuitImage();
        hackerCardRightCircuit.sprite = card.GetRightCircuitImage();
        hackerCardBottomCircuit.sprite = card.GetBottomCircuitImage();
        topLeftSpike.sprite = topLeftSpikeImage;
        topRightSpike.sprite = topRightSpikeImage;
        bottomLeftSpike.sprite = bottomLeftSpikeImage;
        bottomRightSpike.sprite = bottomRightSpikeImage;
    }

    public void CloseCardSample()
    {
        gameObject.SetActive(false);
    }
}
