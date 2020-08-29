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
        itemLevel = 1;
        itemMaxLevel = 5;
        hackerOrRunner = HackerRunner.Hacker;
    }

    private void SetChipProperties()
    {
        switch(itemName)
        {
            case "Cheap Ghost":
                itemType = ItemTypes.Software;
                itemDescription = "Install this on a machine and, if you're lucky, it won't be detected before it starts funnelling you data";
                levelOneItemAbilityDescription = "Your first two 2-Connection spikes each hack are worth +1.";
                cardIds.Add(14);    // Hidden Trigger
                cardIds.Add(14);    // Hidden Trigger
                cardIds.Add(15);    // Too Obvious
                break;
            case "JuryRigged QwikThink":
                itemType = ItemTypes.Wetware;
                itemDescription = "Salvaged QwikThink1000 wetware. It's an older model, and you fixed it up as best you could.";
                levelOneItemAbilityDescription = "Your first 3-Connection spike is worth double the points.";
                cardIds.Add(16);    // QwikThink
                cardIds.Add(17);    // Ad-Hoc Upgrade
                break;
            case "Salvaged Router":
                itemType = ItemTypes.Chipset;
                itemDescription = "This would be fine for civilian use. You should replace it.";
                levelOneItemAbilityDescription = "Each Hack: The first time you place a card outside the safe zone, it does not raise the security level.";
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
