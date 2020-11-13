using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerMod : Item
{
    List<int> levelOneCards = new List<int>();
    List<int> levelTwoCards = new List<int>();
    List<int> levelThreeCards = new List<int>();
    List<int> levelFourCards = new List<int>();
    List<int> levelFiveCards = new List<int>();

    public void SetupMod(string newModName)
    {
        GetMod(newModName);
        itemLevel = 1;
        itemMaxLevel = 5;
        hackerOrRunner = HackerRunner.Runner;
    }

    private void GetMod(string newModName)
    {
        itemName = newModName;
        SetVariablesAndGetCards(itemName);
    }

    public List<int> GetCardIds()
    {
        switch (itemLevel)
        {
            case 1:
                return levelOneCards;
            case 2:
                return levelTwoCards;
            case 3:
                return levelThreeCards;
            case 4:
                return levelFourCards;
            case 5:
                return levelFiveCards;
            default:
                return levelOneCards;
        }
    }

    private List<int> SetVariablesAndGetCards(string modName)
    {
        switch (modName)
        {
            case "Human Eyes":
                itemType = ItemTypes.Head;
                itemDescription = "Unmodified human eyes.";
                levelOneCards.Add(1); // Awareness 1
                levelOneCards.Add(2); // Observe

                levelTwoCards.Add(1); // Awareness 1
                levelTwoCards.Add(22); // Observe 2

                levelThreeCards.Add(20); // Awareness 2
                levelThreeCards.Add(23); // Observe 3

                levelFourCards.Add(20); // Awareness 2
                levelFourCards.Add(24); // Observe

                levelFiveCards.Add(21); // Awareness 3
                levelFiveCards.Add(25); // Observe
                break;
            case "Unmodded Torso":
                itemType = ItemTypes.Torso;
                itemDescription = "Unmodified human torso.";
                levelOneCards.Add(3); // Deep Breath
                levelOneCards.Add(4); // Weak Spot

                levelTwoCards.Add(26); // Deep Breath 2
                levelTwoCards.Add(29); // Weak Spot

                levelThreeCards.Add(27); // Deep Breath 3
                levelThreeCards.Add(30); // Weak Spot

                levelFourCards.Add(27); // Deep Breath 3
                levelFourCards.Add(6); // Brace 1

                levelFiveCards.Add(28); // Deep Breath 4
                levelFiveCards.Add(6); // Brace 1
                break;
            case "Human Skin":
                itemType = ItemTypes.Exoskeleton;
                itemDescription = "Unmodified human skin.";
                levelOneCards.Add(5); // Shake Off
                levelOneCards.Add(6); // Brace

                levelTwoCards.Add(31); // Shake Off 2
                levelTwoCards.Add(6); // Brace

                levelThreeCards.Add(32); // Shake Off
                levelThreeCards.Add(35); // Brace 2

                levelFourCards.Add(33); // Shake Off
                levelFourCards.Add(36); // Brace 3

                levelFiveCards.Add(34); // Shake Off
                levelFiveCards.Add(37); // Brace 4
                break;
            case "Unmodded Arm":
                itemType = ItemTypes.Arm;
                itemDescription = "Unmodified human arm.";
                levelOneCards.Add(7); // Punch
                levelOneCards.Add(7); // Punch
                levelOneCards.Add(8); // Quickdraw

                levelTwoCards.Add(38); // Punch 2
                levelTwoCards.Add(38); // Punch 2
                levelTwoCards.Add(8); // Quickdraw

                levelThreeCards.Add(39); // Punch 3
                levelThreeCards.Add(39); // Punch 3
                levelThreeCards.Add(42); // Quickdraw 2

                levelFourCards.Add(40); // Punch 4
                levelFourCards.Add(40); // Punch 4
                levelFourCards.Add(43); // Quickdraw 3

                levelFiveCards.Add(41); // Punch 5
                levelFiveCards.Add(41); // Punch 5
                levelFiveCards.Add(44); // Quickdraw 4
                break;
            case "Unmodded Leg":
                itemType = ItemTypes.Leg;
                itemDescription = "Unmodified human leg.";
                levelOneCards.Add(9); // Kick
                levelOneCards.Add(9); // Kick
                levelOneCards.Add(10);    // Sprint

                levelTwoCards.Add(45); // Kick 2
                levelTwoCards.Add(45); // Kick 2
                levelTwoCards.Add(49);    // Sprint 2

                levelThreeCards.Add(46); // Kick 3
                levelThreeCards.Add(46); // Kick 3
                levelThreeCards.Add(50);    // Sprint 3

                levelFourCards.Add(47); // Kick 4
                levelFourCards.Add(47); // Kick 4
                levelFourCards.Add(50);    // Sprint 3

                levelFiveCards.Add(48); // Kick 5
                levelFiveCards.Add(48); // Kick 5
                levelFiveCards.Add(51);    // Sprint 4
                break;
            case "Spanner":
                itemType = ItemTypes.Weapon;
                itemDescription = "Heavy wrench. It'll do in a pinch, until you find something better.";
                levelOneCards.Add(11);    // Whack
                levelOneCards.Add(11);    // Whack
                levelOneCards.Add(12);    // Kneecap
                levelOneCards.Add(13);    // Bruise

                levelTwoCards.Add(52);    // Whack 2
                levelTwoCards.Add(52);    // Whack 2
                levelTwoCards.Add(56);    // Kneecap 2
                levelTwoCards.Add(60);    // Bruise 2

                levelThreeCards.Add(53);    // Whack 3
                levelThreeCards.Add(53);    // Whack 3
                levelThreeCards.Add(57);    // Kneecap 3
                levelThreeCards.Add(61);    // Bruise 3

                levelFourCards.Add(54);    // Whack 4
                levelFourCards.Add(54);    // Whack 4
                levelFourCards.Add(58);    // Kneecap
                levelFourCards.Add(62);    // Bruise 4

                levelFiveCards.Add(55);    // Whack 5
                levelFiveCards.Add(55);    // Whack 5
                levelFiveCards.Add(59);    // Kneecap
                levelFiveCards.Add(63);    // Bruise 5
                break;
            case "Adaptable CranioPatch":
                itemType = ItemTypes.Head;
                itemDescription = "FILL THIS IN FILL THIS IN FILL THIS IN FILL THIS IN FILL THIS IN FILL THIS IN";
                levelOneCards.Add(73);  // Quick Targetting 1

                // TODO: NOT ALL LEVEL ONE
                levelTwoCards.Add(73);  // Quick Targetting 1

                levelThreeCards.Add(73);  // Quick Targetting 1

                levelFourCards.Add(73);  // Quick Targetting 1

                levelFiveCards.Add(73);  // Quick Targetting 1
                break;
            default:
                break;
        }

        return levelOneCards;
    }

    public List<int> GetLevelOneCardIds()
    {
        return levelOneCards;
    }

    public List<int> GetLevelTwoCardIds()
    {
        return levelTwoCards;
    }

    public List<int> GetLevelThreeCardIds()
    {
        return levelThreeCards;
    }

    public List<int> GetLevelFourCardIds()
    {
        return levelFourCards;
    }

    public List<int> GetLevelFiveCardIds()
    {
        return levelFiveCards;
    }
}
