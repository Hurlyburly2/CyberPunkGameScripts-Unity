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
        itemLevel = 5;
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

                levelTwoCards.Add(9); // Kick
                levelTwoCards.Add(9); // Kick
                levelTwoCards.Add(10);    // Sprint

                levelThreeCards.Add(9); // Kick
                levelThreeCards.Add(9); // Kick
                levelThreeCards.Add(10);    // Sprint

                levelFourCards.Add(9); // Kick
                levelFourCards.Add(9); // Kick
                levelFourCards.Add(10);    // Sprint

                levelFiveCards.Add(9); // Kick
                levelFiveCards.Add(9); // Kick
                levelFiveCards.Add(10);    // Sprint
                break;
            case "Spanner":
                itemType = ItemTypes.Weapon;
                itemDescription = "Heavy wrench. It'll do in a pinch, until you find something better.";
                levelOneCards.Add(11);    // Whack
                levelOneCards.Add(11);    // Whack
                levelOneCards.Add(12);    // Kneecap
                levelOneCards.Add(13);    // Bruise

                levelTwoCards.Add(11);    // Whack
                levelTwoCards.Add(11);    // Whack
                levelTwoCards.Add(12);    // Kneecap
                levelTwoCards.Add(13);    // Bruise

                levelThreeCards.Add(11);    // Whack
                levelThreeCards.Add(11);    // Whack
                levelThreeCards.Add(12);    // Kneecap
                levelThreeCards.Add(13);    // Bruise

                levelFourCards.Add(11);    // Whack
                levelFourCards.Add(11);    // Whack
                levelFourCards.Add(12);    // Kneecap
                levelFourCards.Add(13);    // Bruise

                levelFiveCards.Add(11);    // Whack
                levelFiveCards.Add(11);    // Whack
                levelFiveCards.Add(12);    // Kneecap
                levelFiveCards.Add(13);    // Bruise
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
