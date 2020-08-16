using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    List<CharacterData> ownedRunners;
    List<HackerData> ownedHackers;

    List<Item> ownedItems;

    CharacterData currentRunner;
    HackerData currentHacker;

    private void Start()
    {
        SetupNewGame();
    }

    private void SetupNewGame()
    {
        ownedRunners = new List<CharacterData>();
        ownedHackers = new List<HackerData>();
        ownedItems = new List<Item>();

        // Setup default runner
        currentRunner = ScriptableObject.CreateInstance<CharacterData>();
        currentRunner.CreateNewRunnerByClassId(0);
        ownedRunners.Add(currentRunner);
        ownedItems.AddRange(currentRunner.GetListOfEquippedItems());

        // Setup default hacker
        currentHacker = ScriptableObject.CreateInstance<HackerData>();
        currentHacker.CreateNewHackerByClassId(0);
        ownedHackers.Add(currentHacker);
        ownedItems.AddRange(currentHacker.GetListOfEquippedItems());

        Debug.Log("Found " + ownedItems.Count + " items on hacker and runner");
    }

    public CharacterData GetCurrentRunner()
    {
        return currentRunner;
    }

    public HackerData GetCurrentHacker()
    {
        return currentHacker;
    }
}
