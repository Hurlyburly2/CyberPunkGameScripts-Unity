using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllHackCards : MonoBehaviour
{
    [SerializeField] HackCard[] allHackCards;

    public HackCard GetCardById(int cardId)
    {
        return allHackCards[cardId];
    }
}
