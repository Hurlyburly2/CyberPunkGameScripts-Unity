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
        string runnerName = "Runner";
        currentRunner.SetupCharacter(runnerName, 30, 30, 10, 0, 3);
        ownedRunners.Add(currentRunner);

        // Setup default hacker
        currentHacker = ScriptableObject.CreateInstance<HackerData>();
        string hackerName = "Hacker";
        currentHacker.SetupHacker(hackerName);
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
