using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerModChip : Item
{
    List<int> cardIds = new List<int>();

    public void SetupChip(string newChipName)
    {
        itemName = newChipName;
        SetChipProperties();
    }

    private void SetChipProperties()
    {
        hackerOrRunner = HackerRunner.Hacker;

        switch(itemName)
        {
            case "Cheap Ghost":
                itemType = ItemTypes.Software;
                cardIds.Add(14);    // Hidden Trigger
                cardIds.Add(14);    // Hidden Trigger
                cardIds.Add(15);    // Too Obvious
                break;
            case "JuryRigged QwikThink":
                itemType = ItemTypes.Wetware;
                cardIds.Add(16);    // QwikThink
                cardIds.Add(17);    // Ad-Hoc Upgrade
                break;
            case "Salvaged Router":
                itemType = ItemTypes.Chipset;
                cardIds.Add(18);    // Failed Connection
                cardIds.Add(18);    // Failed Connection
                cardIds.Add(19);    // Cracked
                break;
            default:
                break;
        }
    }

    public List<int> GetCardIds()
    {
        return cardIds;
    }

    public PassiveAbility SetupPassiveAbility()
    {
        PassiveAbility newAbility = CreateInstance<PassiveAbility>();
        switch(itemName)
        {
            case "Cheap Ghost":
                newAbility.SetupPassiveAbility("spikePointMultiplier", 2, "any", 1, 2);
                // Your first 2 two-connection spikes are worth double points
                break;
            case "JuryRigged QwikThink":
                newAbility.SetupPassiveAbility("spikePointMultiplier", 1, "any", 3, 2);
                // Your first 3 connection spike is worth double points
                break;
            case "Salvaged Router":
                newAbility.SetupPassiveAbility("dangerZoneBuffer", 1);
                // The first time you place a card outside the safe zone each hack, it does
                // not raise the security level
                break;
        }

        return newAbility;
    }
}
