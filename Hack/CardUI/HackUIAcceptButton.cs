using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackUIAcceptButton : MonoBehaviour
{
    [SerializeField] HackCardUIHolder hackCardUIHolder;

    private void OnMouseUp()
    {
        FindObjectOfType<HackBattleData>().SetStateToNormal();
        FindObjectOfType<HackDeck>().SetTopCard();
        hackCardUIHolder.TurnOffCardUIHolder();
    }
}
