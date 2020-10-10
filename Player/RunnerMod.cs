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
        if (newModName == "Human Eyes")
            itemLevel = 3;
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
                levelOneCards.Add(2); // Observer

                levelTwoCards.Add(1); // Awareness 1
                levelTwoCards.Add(2); // Observer

                levelThreeCards.Add(20); // Awareness 2
                levelThreeCards.Add(2); // Observer

                levelFourCards.Add(20); // Awareness 2
                levelFourCards.Add(2); // Observer

                levelFiveCards.Add(1); // Awareness
                levelFiveCards.Add(2); // Observer
                break;
            case "Unmodded Torso":
                itemType = ItemTypes.Torso;
                itemDescription = "Unmodified human torso.";
                levelOneCards.Add(3); // Deep Breath
                levelOneCards.Add(4); // Weak Spot

                levelTwoCards.Add(3); // Deep Breath
                levelTwoCards.Add(4); // Weak Spot

                levelThreeCards.Add(3); // Deep Breath
                levelThreeCards.Add(4); // Weak Spot

                levelFourCards.Add(3); // Deep Breath

                levelFiveCards.Add(3); // Deep Breath
                break;
            case "Human Skin":
                itemType = ItemTypes.Exoskeleton;
                itemDescription = "Unmodified human skin.";
                levelOneCards.Add(5); // Shake Off
                levelOneCards.Add(6); // Brace

                levelTwoCards.Add(5); // Shake Off
                levelTwoCards.Add(6); // Brace

                levelThreeCards.Add(5); // Shake Off
                levelThreeCards.Add(6); // Brace

                levelFourCards.Add(5); // Shake Off
                levelFourCards.Add(6); // Brace

                levelFiveCards.Add(5); // Shake Off
                levelFiveCards.Add(6); // Brace
                break;
            case "Unmodded Arm":
                itemType = ItemTypes.Arm;
                itemDescription = "Unmodified human arm.";
                levelOneCards.Add(7); // Punch
                levelOneCards.Add(7); // Punch
                levelOneCards.Add(8); // Quickdraw

                levelTwoCards.Add(7); // Punch
                levelTwoCards.Add(7); // Punch
                levelTwoCards.Add(8); // Quickdraw

                levelThreeCards.Add(7); // Punch
                levelThreeCards.Add(7); // Punch
                levelThreeCards.Add(8); // Quickdraw

                levelFourCards.Add(7); // Punch
                levelFourCards.Add(7); // Punch
                levelFourCards.Add(8); // Quickdraw

                levelFiveCards.Add(7); // Punch
                levelFiveCards.Add(7); // Punch
                levelFiveCards.Add(8); // Quickdraw
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
