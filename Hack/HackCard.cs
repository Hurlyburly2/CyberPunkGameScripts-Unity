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

    int orientation = 0;
        // 0 = 0, 1 = 90, 2 = 180, 3 = 270
        // needed for figuring out connections

    private void Start()
    {
        SetupCard();
    }

    public void SetupCard()
    {
        
    }
}
