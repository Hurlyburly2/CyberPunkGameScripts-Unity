using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackBattleData : MonoBehaviour
{
    // config
    CharacterData runner;
    HackerData hacker;
    HackDeck hackDeck;
    HackDiscard hackDiscard;

    public void SetCharacterData(CharacterData newRunner, HackerData newHacker)
    {
        runner = newRunner;
        hacker = newHacker;
    }

    public void SetupHack()
    {
        hackDeck = FindObjectOfType<HackDeck>();
        hackDiscard = FindObjectOfType<HackDiscard>();

        List<int> cardIds = runner.GetLoadout().GetAllCardIds();
        cardIds.AddRange(hacker.GetHackerLoadout().GetCardIds());
        LogAllCardIds(cardIds);
    }

    public HackerData GetHacker()
    {
        return hacker;
    }

    private void LogAllCardIds(List<int> cardIds)
    {
        string idString = "";
        foreach (int cardId in cardIds)
        {
            idString += cardId + " ";
        }
        Debug.Log("All Card Ids: " + idString);
    }
}
