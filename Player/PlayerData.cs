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

    // item id is used to compare uniqueness of items, for example - two leg slot items
    int currentItemId;

    private void Start()
    {
        SetupNewGame();
    }

    private void SetupNewGame()
    {
        currentItemId = 0;

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

        RunnerMod extraHeadMod = ScriptableObject.CreateInstance<RunnerMod>();
        extraHeadMod.SetupMod("Human Eyes");
        ownedItems.Add(extraHeadMod);

        RunnerMod extraArmMod = ScriptableObject.CreateInstance<RunnerMod>();
        extraArmMod.SetupMod("Unmodded Arm");
        ownedItems.Add(extraArmMod);

        HackerMod anotherRig = ScriptableObject.CreateInstance<HackerMod>();
        anotherRig.SetupMod("Basic Rig");
        anotherRig.SetItemLevel(3);
        ownedItems.Add(anotherRig);

        HackerMod aThirdRig = ScriptableObject.CreateInstance<HackerMod>();
        aThirdRig.SetupMod("Basic Rig");
        aThirdRig.SetItemLevel(5);
        ownedItems.Add(aThirdRig);

        HackerModChip anotherSoftware = ScriptableObject.CreateInstance<HackerModChip>();
        anotherSoftware.SetupChip("Cheap Ghost");
        ownedItems.Add(anotherSoftware);

        HackerModChip aThirdSoftware = ScriptableObject.CreateInstance<HackerModChip>();
        aThirdSoftware.SetupChip("Cheap Ghost");
        ownedItems.Add(aThirdSoftware);
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

    public int GetAndIncrementItemId()
    {
        currentItemId++;
        return currentItemId - 1;
    }

    public void AddToOwnedItems(Item item)
    {
        ownedItems.Add(item);
    }
}
