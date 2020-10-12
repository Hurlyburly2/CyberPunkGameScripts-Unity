using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllHackCards : MonoBehaviour
{
    public HackCard GetCardById(int cardId)
    {
        string paddedCardId = "" + cardId.ToString("000");
        string imagePath = "CardPrefabs/HackPlayer/HackCard" + paddedCardId;
        HackCard newHackCard = Resources.Load<HackCard>(imagePath);

        return newHackCard;
    }
}
