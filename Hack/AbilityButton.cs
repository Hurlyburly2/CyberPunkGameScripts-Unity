using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityButton : MonoBehaviour
{
    [SerializeField] string whichAbility;
        // possibilities: rig, neuralImplant, uplink
    int abilityId;
    int maxAbilityUses;
    int currentAbilityUses;

    public void SetupAbility(int newAbilityId, int newAbilityMaxUses)
    {
        abilityId = newAbilityId;
        maxAbilityUses = newAbilityMaxUses;
        currentAbilityUses = maxAbilityUses;
    }

    public string GetWhichAbility()
    {
        return whichAbility;
    }

    public void UseAbility()
    {
        switch(abilityId)
        {
            case 0:
                // Add a R connection and two R spikes to the active card
                AddConnectionsToActiveCard();
                AddSpikesToActiveCard();
                Debug.Log("Total uses: " + maxAbilityUses);
                break;
            case 1:
                //1: For your next action, pick from your top two cards.Discard the other
                SelectFromTopOfDeck();
                Debug.Log("Total uses: " + maxAbilityUses);
                break;
            case 2:
                // 2: you may play the top card of your discard as if it was your active card
                PlayCardsFromTopOfDiscard();
                Debug.Log("Total uses: " + maxAbilityUses);
                break;
        }
    }

    private void PlayCardsFromTopOfDiscard()
    {
        Debug.Log("play cards from top of discard");
    }

    private void SelectFromTopOfDeck()
    {
        Debug.Log("Select cards from top of deck");
    }

    private void AddConnectionsToActiveCard()
    {
        Debug.Log("Add Red connection to active card");
    }

    private void AddSpikesToActiveCard()
    {
        Debug.Log("Add Red spikes to active card");
    }
}
