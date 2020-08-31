using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerModChip : Item
{
    List<int> levelOneCardIds = new List<int>();
    List<int> levelTwoCardIds = new List<int>();
    List<int> levelThreeCardIds = new List<int>();

    public void SetupChip(string newChipName)
    {
        itemName = newChipName;
        SetChipProperties();
        itemLevel = 1;
        itemMaxLevel = 3;
        hackerOrRunner = HackerRunner.Hacker;
    }

    private void SetChipProperties()
    {
        // WHEN CREATING A NEW CARD, MUST ALSO ADD PASSIVE ABILITY FUNCTIONALITY BELOW
        switch(itemName)
        {
            case "Cheap Ghost":
                itemType = ItemTypes.Software;
                itemDescription = "Install this on a machine and, if you're lucky, it won't be detected before it starts funnelling you data";
                levelOneItemAbilityDescription = "Your first two 2-Connection spikes each hack are worth +1.";
                levelTwoItemAbilityDescription = "Your first two 2-Connection spikes each hack are worth +1.";
                levelThreeItemAbilityDescription = "Your first two 2-Connection spikes each hack are worth +1.";

                levelOneCardIds.Add(14);    // Hidden Trigger
                levelOneCardIds.Add(14);    // Hidden Trigger
                levelOneCardIds.Add(15);    // Too Obvious

                levelTwoCardIds.Add(14);    // Hidden Trigger
                levelTwoCardIds.Add(14);    // Hidden Trigger
                levelTwoCardIds.Add(15);    // Too Obvious

                levelThreeCardIds.Add(14);    // Hidden Trigger
                levelThreeCardIds.Add(14);    // Hidden Trigger
                levelThreeCardIds.Add(15);    // Too Obvious
                break;
            case "JuryRigged QwikThink":
                itemType = ItemTypes.Wetware;
                itemDescription = "Salvaged QwikThink1000 wetware. It's an older model, and you fixed it up as best you could.";
                levelOneItemAbilityDescription = "Your first 3-Connection spike is worth double the points.";
                levelTwoItemAbilityDescription = "Your first 3-Connection spike is worth double the points.";
                levelThreeItemAbilityDescription = "Your first 3-Connection spike is worth double the points.";

                levelOneCardIds.Add(16);    // QwikThink
                levelOneCardIds.Add(17);    // Ad-Hoc Upgrade

                levelTwoCardIds.Add(16);    // QwikThink
                levelTwoCardIds.Add(17);    // Ad-Hoc Upgrade

                levelThreeCardIds.Add(16);    // QwikThink
                levelThreeCardIds.Add(17);    // Ad-Hoc Upgrade
                break;
            case "Salvaged Router":
                itemType = ItemTypes.Chipset;
                itemDescription = "This would be fine for civilian use. You should replace it.";
                levelOneItemAbilityDescription = "Each Hack: The first time you place a card outside the safe zone, it does not raise the security level.";
                levelTwoItemAbilityDescription = "Each Hack: The first time you place a card outside the safe zone, it does not raise the security level.";
                levelThreeItemAbilityDescription = "Each Hack: The first time you place a card outside the safe zone, it does not raise the security level.";

                levelOneCardIds.Add(18);    // Failed Connection
                levelOneCardIds.Add(18);    // Failed Connection
                levelOneCardIds.Add(19);    // Cracked

                levelTwoCardIds.Add(18);    // Failed Connection
                levelTwoCardIds.Add(18);    // Failed Connection
                levelTwoCardIds.Add(19);    // Cracked

                levelThreeCardIds.Add(18);    // Failed Connection
                levelThreeCardIds.Add(18);    // Failed Connection
                levelThreeCardIds.Add(19);    // Cracked
                break;
            default:
                break;
        }
    }

    public PassiveAbility SetupPassiveAbility()
    {
        PassiveAbility newAbility = CreateInstance<PassiveAbility>();
        switch(itemName)
        {
            case "Cheap Ghost":
                switch (itemLevel)
                {
                    case 1:
                        // Your first 2 two-connection spikes are worth double points
                        newAbility.SetupPassiveAbility(PassiveAbility.PassiveAbilityType.spikePointMultiplier, 2, "any", 1, 2);
                        break;
                    case 2:
                        newAbility.SetupPassiveAbility(PassiveAbility.PassiveAbilityType.spikePointMultiplier, 2, "any", 1, 2);
                        break;
                    case 3:
                        newAbility.SetupPassiveAbility(PassiveAbility.PassiveAbilityType.spikePointMultiplier, 2, "any", 1, 2);
                        break;
                }
                break;
            case "JuryRigged QwikThink":
                switch (itemLevel)
                {
                    case 1:
                        // Your first 3 connection spike is worth double points
                        newAbility.SetupPassiveAbility(PassiveAbility.PassiveAbilityType.spikePointMultiplier, 1, "any", 3, 2);
                        break;
                    case 2:
                        newAbility.SetupPassiveAbility(PassiveAbility.PassiveAbilityType.spikePointMultiplier, 1, "any", 3, 2);
                        break;
                    case 3:
                        newAbility.SetupPassiveAbility(PassiveAbility.PassiveAbilityType.spikePointMultiplier, 1, "any", 3, 2);
                        break;
                }
                break;
            case "Salvaged Router":
                switch (itemLevel)
                {
                    case 1:
                        // The first time you place a card outside the safe zone each hack, it does not raise the security level
                        newAbility.SetupPassiveAbility(PassiveAbility.PassiveAbilityType.dangerZoneBuffer, 1);
                        break;
                    case 2:
                        newAbility.SetupPassiveAbility(PassiveAbility.PassiveAbilityType.dangerZoneBuffer, 1);
                        break;
                    case 3:
                        newAbility.SetupPassiveAbility(PassiveAbility.PassiveAbilityType.dangerZoneBuffer, 1);
                        break;
                }
                break;
        }
        return newAbility;
    }

    public List<int> GetCardIds()
    {
        switch (itemLevel)
        {
            case 1:
                return levelOneCardIds;
            case 2:
                return levelTwoCardIds;
            case 3:
                return levelThreeCardIds;
            default:
                return levelOneCardIds;
        }
    }

    public List<int> GetLevelOneCardIds()
    {
        return levelOneCardIds;
    }

    public List<int> GetLevelTwoCardIds()
    {
        return levelTwoCardIds;
    }

    public List<int> GetLevelThreeCardIds()
    {
        return levelThreeCardIds;
    }
}
