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

    public void SetupCardSample(Card newCard)
    {
        runnerCard.SetActive(true);
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

    public void SetupCardSample(HackCard newHackCard)
    {
        runnerCard.SetActive(false);
        hackCard = newHackCard;
    }

    public void CloseCardSample()
    {
        gameObject.SetActive(false);
    }
}
