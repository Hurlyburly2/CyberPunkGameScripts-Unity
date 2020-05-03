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
    AllHackCards allHackCards;

    private void Awake()
    {
        int count = FindObjectsOfType<BattleData>().Length;

        if (count > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void SetCharacterData(CharacterData newRunner, HackerData newHacker)
    {
        runner = newRunner;
        hacker = newHacker;
    }

    public void SetupHack()
    {
        hackDeck = FindObjectOfType<HackDeck>();
        hackDiscard = FindObjectOfType<HackDiscard>();
        allHackCards = FindObjectOfType<AllHackCards>();

        List<int> cardIds = runner.GetLoadout().GetAllCardIds();
        cardIds.AddRange(hacker.GetHackerLoadout().GetCardIds());

        // Create a deck from here, but for now we use nonsense cards
        LogAllCardIds(cardIds);

        List<HackCard> cards = GetCardsByIds(cardIds);
        hackDeck.SetDeckPrefabs(cards);
        hackDeck.ShuffleDeck();
        hackDeck.SetTopCard();
    }

    private List<HackCard> GetCardsByIds(List<int> cardIds)
    {
        List<HackCard> foundCards = new List<HackCard>();
        foreach(int cardId in cardIds)
        {
            foundCards.Add(allHackCards.GetCardById(cardId));
        }
        return foundCards;
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
