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

    ShopMenu.ShopForSaleType currentShopType;
    List<ShopMenu.ShopForSaleType> previousShopTypes = new List<ShopMenu.ShopForSaleType>();
    List<Item> itemsForSale = new List<Item>();

    private void Start()
    {
        Debug.Log(currentShopType.ToString());
        SetupNewGame();
    }

    private void SetupNewGame()
    {
        // TODO: CHANGE THIS BACK TO ZERO
        playerLevel = 1;
        currentCredits = 1000;

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

        //RunnerMod adaptableCranioPatch = ScriptableObject.CreateInstance<RunnerMod>();
        //adaptableCranioPatch.SetupMod("Adaptable CranioPatch");
        ////adaptableCranioPatch.SetItemLevel(2);
        //ownedItems.Add(adaptableCranioPatch);

        //RunnerMod adrenalInjector = ScriptableObject.CreateInstance<RunnerMod>();
        //adrenalInjector.SetupMod("Adrenal Injector");
        //ownedItems.Add(adrenalInjector);

        //RunnerMod sensoryRegulator = ScriptableObject.CreateInstance<RunnerMod>();
        //sensoryRegulator.SetupMod("Sensory Regulator");
        //ownedItems.Add(sensoryRegulator);

        //RunnerMod automatedDigits = ScriptableObject.CreateInstance<RunnerMod>();
        //automatedDigits.SetupMod("Automated Digits");
        //ownedItems.Add(automatedDigits);

        //RunnerMod automatedDigitsTwo = ScriptableObject.CreateInstance<RunnerMod>();
        //automatedDigitsTwo.SetupMod("Automated Digits");
        //ownedItems.Add(automatedDigitsTwo);

        //RunnerMod polymorphicSupport = ScriptableObject.CreateInstance<RunnerMod>();
        //polymorphicSupport.SetupMod("Polymorphic Support");
        //ownedItems.Add(polymorphicSupport);

        //RunnerMod polymorphicSupportTwo = ScriptableObject.CreateInstance<RunnerMod>();
        //polymorphicSupportTwo.SetupMod("Polymorphic Support");
        //ownedItems.Add(polymorphicSupportTwo);

        //RunnerMod tornadoHandgun = ScriptableObject.CreateInstance<RunnerMod>();
        //tornadoHandgun.SetupMod("Tornado Handgun T-492");
        //ownedItems.Add(tornadoHandgun);

        //RunnerMod voltHandCannon = ScriptableObject.CreateInstance<RunnerMod>();
        //voltHandCannon.SetupMod("Volt HandCannon V-1");
        //ownedItems.Add(voltHandCannon);

        Loadout runnerLoadout = currentRunner.GetLoadout();
        //runnerLoadout.EquipItem(adaptableCranioPatch);
        //runnerLoadout.EquipItem(adrenalInjector);
        //runnerLoadout.EquipItem(sensoryRegulator);
        //runnerLoadout.EquipItem(automatedDigits, Loadout.LeftOrRight.Left);
        //runnerLoadout.EquipItem(automatedDigitsTwo, Loadout.LeftOrRight.Right);
        //runnerLoadout.EquipItem(polymorphicSupport, Loadout.LeftOrRight.Left);
        //runnerLoadout.EquipItem(polymorphicSupportTwo, Loadout.LeftOrRight.Right);
        //runnerLoadout.EquipItem(tornadoHandgun);
        //runnerLoadout.EquipItem(voltHandCannon);

        GenerateJobOptions();
        GenerateNewShop();
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

    public void GenerateNewShop()
    {
        GenerateNewShopType();
        Debug.Log("Current Shop Type: " + currentShopType.ToString());
        GenerateShopInventory();
    }

    private void GenerateShopInventory()
    {
        ItemGenerator itemGenerator = ScriptableObject.CreateInstance<ItemGenerator>();
        List<string> applicableItems = itemGenerator.GetItemListByLevelAndType(playerLevel, currentShopType);

        // Only put in shop items that the player does not own
        foreach (Item item in ownedItems)
        {
            if (applicableItems.Contains(item.GetItemName()))
                applicableItems.Remove(item.GetItemName());
        }

        // Here we limit the amount of items in the shop to 5 if over 5 applicable items are available
        if (applicableItems.Count > 5)
        {
            List<string> newList = new List<string>();
            while (newList.Count < 5)
            {
                int index = Random.Range(0, applicableItems.Count);
                if (!newList.Contains(applicableItems[index]))
                    newList.Add(applicableItems[index]);
            }
            applicableItems = newList;
        }

        AttemptToCreateShopItems(applicableItems);
    }

    private void AttemptToCreateShopItems(List<string> itemNames)
    {
        // TODO: DO OBJECTS NEED TO BE DESTROYED???
        List<Item> shopItems = new List<Item>();
        foreach (string itemName in itemNames)
        {
            RunnerMod createAsRunnerMod = ScriptableObject.CreateInstance<RunnerMod>();
            bool success = createAsRunnerMod.SetupMod(itemName);
            if (!success)
            {
                // Failed to create as a runner mod, try a hacker mod...
                HackerMod createAsHackerMod = ScriptableObject.CreateInstance<HackerMod>();
                success = createAsRunnerMod.SetupMod(itemName);
                if (!success)
                {
                    // Failed to create as a hacker mod, try a hacker install...
                    HackerModChip createAsHackerInstall = ScriptableObject.CreateInstance<HackerModChip>();
                    success = createAsHackerInstall.SetupChip(itemName);
                    if (success)
                    {
                        shopItems.Add(createAsHackerInstall);
                    }
                } else
                {
                    shopItems.Add(createAsHackerMod);
                }
            } else
            {
                shopItems.Add(createAsRunnerMod);
            }
        }

        itemsForSale = shopItems;
    }

    private void GenerateNewShopType()
    {
        // Here, we make sure that a full rotation of all four shop types are
        // gone through without repeats. Then, a new cycle of four is started.
        // This ensures that the player is eventually able to see all four shop types fairly often
        if (previousShopTypes.Count == 4)
            previousShopTypes = new List<ShopMenu.ShopForSaleType>();

        List<ShopMenu.ShopForSaleType> availableTypes = new List<ShopMenu.ShopForSaleType>();
        availableTypes.Add(ShopMenu.ShopForSaleType.Mech);
        availableTypes.Add(ShopMenu.ShopForSaleType.Tech);
        availableTypes.Add(ShopMenu.ShopForSaleType.Cyber);
        availableTypes.Add(ShopMenu.ShopForSaleType.Bio);

        foreach (ShopMenu.ShopForSaleType previousType in previousShopTypes)
        {
            if (availableTypes.Contains(previousType))
                availableTypes.Remove(previousType);
        }

        ShopMenu.ShopForSaleType newShopType = availableTypes[Random.Range(0, availableTypes.Count)];

        currentShopType = newShopType;

        previousShopTypes.Add(currentShopType);
    }

    public List<Item> GetItemsForSale()
    {
        return itemsForSale;
    }

    public ShopMenu.ShopForSaleType GetShopType()
    {
        return currentShopType;
    }

    public void GainItem(Item itemToGain)
    {
        ownedItems.Add(itemToGain);
    }

    public void RemoveItemFromForSale(Item itemToRemove)
    {
        itemsForSale.Remove(itemToRemove);
    }
}
