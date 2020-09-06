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
        currentRunner.UnlockRunner();
        ownedRunners.Add(currentRunner);
        ownedItems.AddRange(currentRunner.GetListOfEquippedItems());

        // Add placeholders for other runners
        CharacterData lockedRunnerOne = ScriptableObject.CreateInstance<CharacterData>();
        lockedRunnerOne.CreateNewRunnerByClassId(1);
        ownedRunners.Add(lockedRunnerOne);
        CharacterData lockedRunnerTwo = ScriptableObject.CreateInstance<CharacterData>();
        lockedRunnerTwo.CreateNewRunnerByClassId(2);
        ownedRunners.Add(lockedRunnerTwo);

        // Setup default hacker
        currentHacker = ScriptableObject.CreateInstance<HackerData>();
        currentHacker.CreateNewHackerByClassId(0);
        currentHacker.UnlockHacker();
        ownedHackers.Add(currentHacker);
        ownedItems.AddRange(currentHacker.GetListOfEquippedItems());

        // Add placeholders for other hackers
        HackerData lockedHackerOne = ScriptableObject.CreateInstance<HackerData>();
        lockedHackerOne.CreateNewHackerByClassId(1);
        ownedHackers.Add(lockedHackerOne);
        HackerData lockedHackerTwo = ScriptableObject.CreateInstance<HackerData>();
        lockedHackerTwo.CreateNewHackerByClassId(2);
        ownedHackers.Add(lockedHackerTwo);
    }

    public List<Item> GetPlayerItems()
    {
        return ownedItems;
    }

    public CharacterData GetCurrentRunner()
    {
        return currentRunner;
    }

    public List<CharacterData> GetRunnerList()
    {
        return ownedRunners;
    }

    public HackerData GetCurrentHacker()
    {
        return currentHacker;
    }

    public List<HackerData> GetHackerList()
    {
        return ownedHackers;
    }

    public void SetCurrentRunner(CharacterData newRunner)
    {
        currentRunner = newRunner;
    }

    public void SetCurrentHacker(HackerData newHacker)
    {
        currentHacker = newHacker;
    }
}
