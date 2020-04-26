using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackCard : MonoBehaviour
{
    [SerializeField] int cardId;

    [SerializeField] Spike topLeftSpike;
    [SerializeField] Spike topRightSpike;
    [SerializeField] Spike bottomLeftSpike;
    [SerializeField] Spike bottomRightSpike;

    private void Start()
    {
        SetupCard();
    }

    public void SetupCard()
    {

    }
}
