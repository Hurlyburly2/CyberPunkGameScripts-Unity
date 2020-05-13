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

    string state = "normal";
    // currently: normal, cardui
    int redPoints = 0;
    int bluePoints = 0;
    int purplePoints = 0;
    PointIconHolder redPointIconHolder;
    PointIconHolder bluePointIconHolder;
    PointIconHolder purplePointIconHolder;

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

        SetupPointHolders();
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
    }

    public void SetStateToCardUI()
    {
        state = "cardui";
    }

    public void SetStateToNormal()
    {
        state = "normal";
    }

    public string GetState()
    {
        return state;
    }

    public bool IsPlayerAllowedToDragCard()
    {
        if (state == "cardui")
        {
            return false;
        } else
        {
            return true;
        }
    }

    private void SetupPointHolders()
    {
        PointIconHolder[] pointIconHolders = FindObjectsOfType<PointIconHolder>();
        foreach (PointIconHolder pointIconHolder in pointIconHolders)
        {
            switch(pointIconHolder.GetWhichColor())
            {
                case "red":
                    redPointIconHolder = pointIconHolder;
                    break;
                case "blue":
                    bluePointIconHolder = pointIconHolder;
                    break;
                case "purple":
                    purplePointIconHolder = pointIconHolder;
                    break;
            }
        }
        UpdatePointDisplay();
    }

    public void UpdatePointValue(string color, int amount)
    {
        switch(color)
        {
            case "red":
                redPoints += amount;
                break;
            case "blue":
                bluePoints += amount;
                break;
            case "purple":
                purplePoints += amount;
                break;
        }
        UpdatePointDisplay();
    }

    public void UpdatePointDisplay()
    {
        redPointIconHolder.UpdatePointDisplay(redPoints);
        bluePointIconHolder.UpdatePointDisplay(bluePoints);
        purplePointIconHolder.UpdatePointDisplay(purplePoints);
    }
}
