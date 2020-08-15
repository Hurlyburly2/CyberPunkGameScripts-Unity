using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    List<CharacterData> ownedRunners;
    List<HackerData> ownedHackers;

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

        // Setup default runner
        currentRunner = ScriptableObject.CreateInstance<CharacterData>();
        currentRunner.CreateNewRunnerByClassId(0);
        ownedRunners.Add(currentRunner);

        // Setup default hacker
        currentHacker = ScriptableObject.CreateInstance<HackerData>();
        currentHacker.CreateNewHackerByClassId(0);
        ownedHackers.Add(currentHacker);
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
