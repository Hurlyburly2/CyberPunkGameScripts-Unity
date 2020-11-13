using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    int playerLevel;
    // This is a hidden value that the user will never see, it will be used to determine:
    //      available jobs, job rewards, items available in shop
    //      you increase playerLevel by playing story missions   

    List<CharacterData> ownedRunners;
    List<HackerData> ownedHackers;

    List<Item> ownedItems;

    CharacterData currentRunner;
    HackerData currentHacker;

    List<Job> currentJobOptions;

    int currentCredits;

    private void Start()
    {
        SetupNewGame();
    }

    private void SetupNewGame()
    {
        playerLevel = 0;
        currentCredits = 100000;

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

        RunnerMod adaptableCranioPatch = ScriptableObject.CreateInstance<RunnerMod>();
        adaptableCranioPatch.SetupMod("Adaptable CranioPatch");
        //adaptableCranioPatch.SetItemLevel(2);
        ownedItems.Add(adaptableCranioPatch);

        GenerateJobOptions();
    }

    public void GenerateJobOptions()
    {
        // this should be run once upon every return to the hub world
        currentJobOptions = new List<Job>();
        for (int i = 0; i < 3; i++)
        {
            Job newJob = ScriptableObject.CreateInstance<Job>();
            newJob.GenerateJob(playerLevel);
            currentJobOptions.Add(newJob);
            Debug.Log(newJob.GetInstanceID());
        }
        Debug.Log("jobs generated: " + currentJobOptions.Count);
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

    public void AddToOwnedItems(Item item)
    {
        ownedItems.Add(item);
    }

    public List<Job> GetCurrentJobsList()
    {
        return currentJobOptions;
    }

    public void CreditsGain(int amount)
    {
        currentCredits += amount;
    }

    public void CreditsSpend(int amount)
    {
        if (currentCredits - amount > 0)
        {
            currentCredits -= amount;
        } else
        {
            currentCredits = 0;
        }
    }

    public void CreditsLose(int amount)
    {
        currentCredits -= amount;
        if (currentCredits < 0)
            currentCredits = 0;
    }

    public int GetCreditsAmount()
    {
        return currentCredits;
    }
}
