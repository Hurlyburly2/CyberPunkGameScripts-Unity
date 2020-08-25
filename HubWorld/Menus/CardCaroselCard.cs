using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardCaroselCard : MonoBehaviour
{
    // Battle Card Stuff
    [SerializeField] GameObject cardBattleContextHolder;
    [SerializeField] Image cardBattleImage;

    // Circuits
    [SerializeField] Image leftCircuit;
    [SerializeField] Image topCircuit;
    [SerializeField] Image rightCircuit;
    [SerializeField] Image bottomCircuit;

    bool runnerMode = true;

    public void SetupCard(Card card)
    {
        string paddedCardId = "" + card.GetCardId().ToString("000");
        string imagePath = "CardParts/CardImages/CardImage" + paddedCardId;
        cardBattleImage.sprite = Resources.Load<Sprite>(imagePath);
        leftCircuit.sprite = card.GetLeftCircuitImage();
        topCircuit.sprite = card.GetTopCircuitImage();
        rightCircuit.sprite = card.GetRightCircuitImage();
        bottomCircuit.sprite = card.GetBottomCircuitImage();
    }
}
