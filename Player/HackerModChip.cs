using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerModChip : Item
{
    List<int> levelOneCardIds = new List<int>();
    List<int> levelTwoCardIds = new List<int>();
    List<int> levelThreeCardIds = new List<int>();

    string passiveAbilityString1;
    string passiveAbilityString2;
    string passiveAbilityString3;

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
                levelOneItemAbilityDescription = "Your first 2 Two-Connection spikes each hack are worth +1.";
                levelTwoItemAbilityDescription = "Your first 2 Two-Connection spikes each hack are worth +2.";
                levelThreeItemAbilityDescription = "Your first 3 Two-Connection spikes each hack are worth +2.";

                passiveAbilityString1 = "Your first 2<color=#593757>/2/3</color> Two-connection spikes each hack are worth +1<color=#593757>/+1/+2</color>";
                passiveAbilityString2 = "Your first <color=#593757>2/</color>2<color=#593757>/3</color> Two-connection spikes each hack are worth <color=#593757>+1/</color>+2<color=#593757>/+2</color>";
                passiveAbilityString3 = "Your first <color=#593757>2/2/</color>3 Two-connection spikes each hack are worth <color=#593757>+1/+2/</color>+2"; ;

                levelOneCardIds.Add(14);    // Hidden Trigger
                levelOneCardIds.Add(14);    // Hidden Trigger
                levelOneCardIds.Add(15);    // Too Obvious

                levelTwoCardIds.Add(64);    // Hidden Trigger 2
                levelTwoCardIds.Add(64);    // Hidden Trigger 2
                levelTwoCardIds.Add(66);    // Too Obvious 2

                levelThreeCardIds.Add(65);    // Hidden Trigger 3
                levelThreeCardIds.Add(65);    // Hidden Trigger 3
                break;
            case "JuryRigged QwikThink":
                itemType = ItemTypes.Wetware;
                itemDescription = "Salvaged QwikThink1000 wetware. It's an older model, and you fixed it up as best you could.";
                levelOneItemAbilityDescription = "Your first 3-Connection spike is worth double the points.";
                levelTwoItemAbilityDescription = "Your first 3-Connection spike is worth double the points.";
                levelThreeItemAbilityDescription = "Your first 3-Connection spike is worth triple the points.";

                passiveAbilityString1 = "Your first 3-Connection spike is worth double<color=#593757>/double/triple</color> the points";
                passiveAbilityString2 = "Your first 3-Connection spike is worth <color=#593757>double/</color>double<color=#593757>/triple</color> the points";
                passiveAbilityString3 = "Your first 3-Connection spike is worth <color=#593757>double/double/</color>triple the points";

                levelOneCardIds.Add(16);    // QwikThink
                levelOneCardIds.Add(17);    // Ad-Hoc Upgrade

                levelTwoCardIds.Add(67);    // QwikThink 2
                levelTwoCardIds.Add(69);    // Ad-Hoc Upgrade 2

                levelThreeCardIds.Add(68);    // QwikThink 3
                levelThreeCardIds.Add(70);    // Ad-Hoc Upgrade 3
                break;
            case "Salvaged Router":
                itemType = ItemTypes.Chipset;
                itemDescription = "This would be fine for civilian use. You should replace it.";
                levelOneItemAbilityDescription = "The first time you place a card outside the safe zone, it does not raise the security level.";
                levelTwoItemAbilityDescription = "The first time you place a card outside the safe zone, it does not raise the security level.";
                levelThreeItemAbilityDescription = "The first time you place a card outside the safe zone, it does not raise the security level.";

                passiveAbilityString1 = "The first 1<color=#593757>/2/2</color> times you place a card outside the safe zone, it does not raise the security level.";
                passiveAbilityString2 = "The first <color=#593757>1/</color>2<color=#593757>/2</color> times you place a card outside the safe zone, it does not raise the security level.";
                passiveAbilityString3 = "The first <color=#593757>1/2/</color>2 times you place a card outside the safe zone, it does not raise the security level.";

                levelOneCardIds.Add(18);    // Failed Connection
                levelOneCardIds.Add(18);    // Failed Connection
                levelOneCardIds.Add(19);    // Cracked

                levelTwoCardIds.Add(18);    // Failed Connection
                levelTwoCardIds.Add(71);    // Cracked

                levelThreeCardIds.Add(72);    // Cracked
                break;
            default:
                itemType = ItemTypes.None;
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

    public string GetPassiveAbilityStringByLevel(int level)
    {
        switch (level)
        {
            case 1:
                return passiveAbilityString1;
            case 2:
                return passiveAbilityString2;
            case 3:
                return passiveAbilityString3;
        }
        return "";
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

    public string GetCurrentLevelAbilityString()
    {
        switch (itemLevel)
        {
            case 1:
                return levelOneItemAbilityDescription;
            case 2:
                return levelTwoItemAbilityDescription;
            case 3:
                return levelThreeItemAbilityDescription;
            default:
                return levelOneItemAbilityDescription;
        }
    }
}
