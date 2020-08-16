using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerMod : Item
{
    List<int> cardIds = new List<int>();

    public void SetupMod(string newModName)
    {
        GetMod(newModName);
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
        hackerOrRunner = HackerRunner.Runner;

        List<int> newCardList = new List<int>();
        switch (modName)
        {
            case "Human Eyes":
                itemType = ItemTypes.Head;
                newCardList.Add(1); // Awareness
                newCardList.Add(2); // Observer
                break;
            case "Unmodded Torso":
                itemType = ItemTypes.Torso;
                newCardList.Add(3); // Deep Breath
                newCardList.Add(4); // Weak Spot
                break;
            case "Human Skin":
                itemType = ItemTypes.Exoskeleton;
                newCardList.Add(5); // Shake Off
                newCardList.Add(6); // Brace
                break;
            case "Unmodded Arm":
                itemType = ItemTypes.Arm;
                newCardList.Add(7); // Punch
                newCardList.Add(7); // Punch
                newCardList.Add(8); // Quickdraw
                break;
            case "Unmodded Leg":
                itemType = ItemTypes.Leg;
                newCardList.Add(9); // Kick
                newCardList.Add(9); // Kick
                newCardList.Add(10);    // Sprint
                break;
            case "Spanner":
                itemType = ItemTypes.Weapon;
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
