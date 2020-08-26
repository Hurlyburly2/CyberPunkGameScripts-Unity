﻿using System.Collections;
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

    bool runnerMode = true;

    public void SetupCard(Card newCard)
    {
        card = newCard;
        string paddedCardId = "" + card.GetCardId().ToString("000");
        string imagePath = "CardParts/CardImages/CardImage" + paddedCardId;
        cardBattleImage.sprite = Resources.Load<Sprite>(imagePath);
        leftCircuit.sprite = card.GetLeftCircuitImage();
        topCircuit.sprite = card.GetTopCircuitImage();
        rightCircuit.sprite = card.GetRightCircuitImage();
        bottomCircuit.sprite = card.GetBottomCircuitImage();
        cardText.text = card.GetCardText();
    }

    public void ToggleToHackCard()
    {
        cardBattleContextHolder.SetActive(false);
        cardHackContextHolder.SetActive(true);

        string paddedCardId = "" + card.GetCardId().ToString("000");
        string imagePath = "CardPrefabs/HackPlayer/HackCard" + paddedCardId;
        HackCard hackCard = Resources.Load<HackCard>(imagePath);

        cardHackImage.sprite = hackCard.GetCardImage();

        SetupSpikes(hackCard);
    }

    public void ToggleToRunnerCard()
    {
        cardHackContextHolder.SetActive(false);
        cardBattleContextHolder.SetActive(true);
    }

    public void OpenBigCardSample()
    {
        cardSample.gameObject.SetActive(true);
        cardSample.SetupCardSample(card);
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
            topLeftSpikeImage.sprite = Resources.Load<Sprite>(emptyImagePath);
        }

        if (hackCard.GetBottomLeftSpike().GetSpikeColor() != "none")
        {
            string bottomLeftSpikePath = "CardParts/Spikes/" + hackCard.GetBottomLeftSpike().GetSpikeColor().ToUpper() + "-BOTTOMLEFT";
            bottomLeftSpikeImage.sprite = Resources.Load<Sprite>(bottomLeftSpikePath);
        } else
        {
            topLeftSpikeImage.sprite = Resources.Load<Sprite>(emptyImagePath);
        }

        if (hackCard.GetbottomRightSpike().GetSpikeColor() != "none")
        {
            string bottomRightSpikePath = "CardParts/Spikes/" + hackCard.GetbottomRightSpike().GetSpikeColor().ToUpper() + "-BOTTOMRIGHT";
            bottomRightSpikeImage.sprite = Resources.Load<Sprite>(bottomRightSpikePath);
        } else
        {
            topLeftSpikeImage.sprite = Resources.Load<Sprite>(emptyImagePath);
        }
    }
}
