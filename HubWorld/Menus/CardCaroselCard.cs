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

    public void OpenBigCardSample()
    {
        cardSample.gameObject.SetActive(true);
        cardSample.SetupCardSample(card);
    }
}
