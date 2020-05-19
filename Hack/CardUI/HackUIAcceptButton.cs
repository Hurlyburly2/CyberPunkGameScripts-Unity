using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackUIAcceptButton : MonoBehaviour
{
    [SerializeField] HackCardUIHolder hackCardUIHolder;

    private void OnMouseUp()
    {
        HackBattleData hackBattleData = FindObjectOfType<HackBattleData>();
        hackBattleData.SetStateToNormal();
        hackBattleData.OnCardPlacement();

        FindObjectOfType<HackDeck>().SetTopCard();
        hackCardUIHolder.GetParentCard().FindParentSquare().UpdateSpikeConnections();
        hackCardUIHolder.TurnOffCardUIHolder();
    }
}
