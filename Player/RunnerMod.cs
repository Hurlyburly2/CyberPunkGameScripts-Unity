using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerMod : Item
{
    List<int> cardIds = new List<int>();

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
        cardIds = SetVariablesAndGetCards(itemName);
    }

    public List<int> GetCardIds()
    {
        return cardIds;
    }

    private List<int> SetVariablesAndGetCards(string modName)
    {
        List<int> newCardList = new List<int>();
        switch (modName)
        {
            case "Human Eyes":
                itemType = ItemTypes.Head;
                itemDescription = "Unmodified human eyes.";
                newCardList.Add(1); // Awareness
                newCardList.Add(2); // Observer
                break;
            case "Unmodded Torso":
                itemType = ItemTypes.Torso;
                itemDescription = "Unmodified human torso.";
                newCardList.Add(3); // Deep Breath
                newCardList.Add(4); // Weak Spot
                break;
            case "Human Skin":
                itemType = ItemTypes.Exoskeleton;
                itemDescription = "Unmodified human skin.";
                newCardList.Add(5); // Shake Off
                newCardList.Add(6); // Brace
                break;
            case "Unmodded Arm":
                itemType = ItemTypes.Arm;
                itemDescription = "Unmodified human arm.";
                newCardList.Add(7); // Punch
                newCardList.Add(7); // Punch
                newCardList.Add(8); // Quickdraw
                break;
            case "Unmodded Leg":
                itemType = ItemTypes.Leg;
                itemDescription = "Unmodified human leg.";
                newCardList.Add(9); // Kick
                newCardList.Add(9); // Kick
                newCardList.Add(10);    // Sprint
                break;
            case "Spanner":
                itemType = ItemTypes.Weapon;
                itemDescription = "Heavy wrench. It'll do in a pinch, until you find something better.";
                newCardList.Add(11);    // Whack
                newCardList.Add(11);    // Whack
                newCardList.Add(12);    // Kneecap
                newCardList.Add(13);    // Bruise
                break;
            default:
                break;
        }

        return newCardList;
    }
}
