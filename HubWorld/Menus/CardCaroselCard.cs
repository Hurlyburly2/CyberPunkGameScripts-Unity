using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardCaroselCard : MonoBehaviour
{
    [SerializeField] MenuCardSample cardSample;

    // Battle Card Stuff
    [SerializeField] GameObject cardBattleContextHolder;
    [SerializeField] Image cardBattleImage;
    [SerializeField] TextMeshProUGUI cardText;
    [SerializeField] GameObject energyCost;

    // Hack Card Stuff
    [SerializeField] GameObject cardHackContextHolder;
    [SerializeField] Image cardHackImage;
    [SerializeField] Image topLeftSpikeImage;
    [SerializeField] Image topRightSpikeImage;
    [SerializeField] Image bottomLeftSpikeImage;
    [SerializeField] Image bottomRightSpikeImage;

    // Circuits
    [SerializeField] Image leftCircuit;
    [SerializeField] Image topCircuit;
    [SerializeField] Image rightCircuit;
    [SerializeField] Image bottomCircuit;

    Card card;
    HackCard hackCard;

    bool runnerMode = true;

    public void SetupCard(Card newCard)
    {
        runnerMode = true;
        card = newCard;
        string paddedCardId = "" + card.GetCardImageId().ToString("000");
        string imagePath = "CardParts/CardImages/CardImage" + paddedCardId;
        cardBattleImage.sprite = Resources.Load<Sprite>(imagePath);
        leftCircuit.sprite = card.GetLeftCircuitImage();
        topCircuit.sprite = card.GetTopCircuitImage();
        rightCircuit.sprite = card.GetRightCircuitImage();
        bottomCircuit.sprite = card.GetBottomCircuitImage();
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

    public void ToggleToHackCard()
    {
        runnerMode = false;
        cardBattleContextHolder.SetActive(false);
        cardHackContextHolder.SetActive(true);

        string paddedCardId = "" + card.GetCardId().ToString("000");
        string imagePath = "CardPrefabs/HackPlayer/HackCard" + paddedCardId;
        HackCard newHackCard = Resources.Load<HackCard>(imagePath);

        hackCard = newHackCard;
        cardHackImage.sprite = hackCard.GetCardImage();

        SetupSpikes(hackCard);
    }

    public void ToggleToRunnerCard()
    {
        runnerMode = true;
        cardHackContextHolder.SetActive(false);
        cardBattleContextHolder.SetActive(true);
    }

    public void OpenBigCardSample()
    {
        cardSample.gameObject.SetActive(true);
        if (runnerMode)
        {
            cardSample.SetupCardSample(card);
        } else
        {
            cardSample.SetupCardSample(hackCard, card, topLeftSpikeImage.sprite, topRightSpikeImage.sprite, bottomLeftSpikeImage.sprite, bottomRightSpikeImage.sprite);
        }
    }

    private void SetupSpikes(HackCard hackCard)
    {
        string emptyImagePath = "CardParts/Circuits/EMPTYCIRCUIT";

        if (hackCard.GetTopLeftSpike().GetSpikeColor() != "none")
        {
            string topLeftSpikePath = "CardParts/Spikes/" + hackCard.GetTopLeftSpike().GetSpikeColor().ToUpper() + "-UPLEFT";
            topLeftSpikeImage.sprite = Resources.Load<Sprite>(topLeftSpikePath);
        } else
        {
            topLeftSpikeImage.sprite = Resources.Load<Sprite>(emptyImagePath);
        }

        if (hackCard.GetTopRightSpike().GetSpikeColor() != "none")
        {
            string topRightSpikePath = "CardParts/Spikes/" + hackCard.GetTopRightSpike().GetSpikeColor().ToUpper() + "-UPRIGHT";
            topRightSpikeImage.sprite = Resources.Load<Sprite>(topRightSpikePath);
        } else
        {
            topRightSpikeImage.sprite = Resources.Load<Sprite>(emptyImagePath);
        }

        if (hackCard.GetBottomLeftSpike().GetSpikeColor() != "none")
        {
            string bottomLeftSpikePath = "CardParts/Spikes/" + hackCard.GetBottomLeftSpike().GetSpikeColor().ToUpper() + "-BOTTOMLEFT";
            bottomLeftSpikeImage.sprite = Resources.Load<Sprite>(bottomLeftSpikePath);
        } else
        {
            bottomLeftSpikeImage.sprite = Resources.Load<Sprite>(emptyImagePath);
        }

        if (hackCard.GetbottomRightSpike().GetSpikeColor() != "none")
        {
            string bottomRightSpikePath = "CardParts/Spikes/" + hackCard.GetbottomRightSpike().GetSpikeColor().ToUpper() + "-BOTTOMRIGHT";
            bottomRightSpikeImage.sprite = Resources.Load<Sprite>(bottomRightSpikePath);
        } else
        {
            bottomRightSpikeImage.sprite = Resources.Load<Sprite>(emptyImagePath);
        }
    }
}
