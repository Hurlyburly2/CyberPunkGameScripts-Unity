using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerMod : ScriptableObject
{
    List<int> cardIds = new List<int>();
    string modName;

    public void SetupMod(string newModName)
    {
        GetMod(newModName);
    }

    private void GetMod(string newModName)
    {
        modName = newModName;
        cardIds = GetCards(modName);
    }

    public string GetModName()
    {
        return modName;
    }

    public List<int> GetCardIds()
    {
        return cardIds;
    }

    private List<int> GetCards(string modName)
    {
        List<int> newCardList = new List<int>();
        switch (modName)
        {
            case "Human Eyes":
                newCardList.Add(1); // Awareness
                newCardList.Add(2); // Observer
                break;
            case "Unmodded Torso":
                newCardList.Add(3); // Deep Breath
                newCardList.Add(4); // Weak Spot
                break;
            case "Human Skin":
                newCardList.Add(5); // Shake Off
                newCardList.Add(6); // Brace
                break;
            case "Unmodded Arm":
                newCardList.Add(7);
                newCardList.Add(7);
                newCardList.Add(8);
                break;
            case "Unmodded Leg":
                newCardList.Add(9);
                newCardList.Add(9);
                newCardList.Add(10);
                break;
            case "Spanner":
                newCardList.Add(11);
                newCardList.Add(11);
                newCardList.Add(12);
                newCardList.Add(13);
                break;
            default:
                break;
        }

        return newCardList;
    }
}
