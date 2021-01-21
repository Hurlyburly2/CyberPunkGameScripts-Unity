using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

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

    bool isPlayerLoaded = false;
    int saveSlot;
    int generatedItemId = 0;
    // Represents the difference between an empty player object and an object that a player has been loaded into

    public void SetupNewGame(int newSaveSlot)
    {
        saveSlot = newSaveSlot;
        isPlayerLoaded = true;

        // TODO: CHANGE THIS BACK TO ZERO
        playerLevel = 1;
        currentCredits = 1000000;

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

        // COMMENT OUT FROM HERE UNTIL NEXT MARKER TO GENERATE UNFILTERED ITEM LIST
        // Only put in shop items that the player does not own
        foreach (Item item in ownedItems)
        {
            if (applicableItems.Contains(item.GetItemName()) && item.GetItemType() != Item.ItemTypes.Arm && item.GetItemType() != Item.ItemTypes.Leg)
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
        // END COMMENT ZONE FOR UNFILTERED ITEM LIST

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
                success = createAsHackerMod.SetupMod(itemName);
                if (!success)
                {
                    // Failed to create as a hacker mod, try a hacker install...
                    HackerModChip createAsHackerInstall = ScriptableObject.CreateInstance<HackerModChip>();
                    success = createAsHackerInstall.SetupChip(itemName);
                    if (success)
                    {
                        // TODO: CREATE EXTRA COPIES TO ADD TO LIST
                        shopItems.Add(createAsHackerInstall);
                    }
                } else
                {
                    shopItems.Add(createAsHackerMod);
                }
            } else
            {
                bool addToShop = true;
                if (createAsRunnerMod.GetItemType() == Item.ItemTypes.Arm || createAsRunnerMod.GetItemType() == Item.ItemTypes.Leg)
                {
                    // If arm or leg, we need to count them
                    List<Item> sameItems = ownedItems.FindAll(curentItem => createAsRunnerMod.GetItemName() == curentItem.GetItemName());
                    switch (sameItems.Count)
                    {
                        case 0:
                            RunnerMod secondCopy = ScriptableObject.CreateInstance<RunnerMod>();
                            secondCopy.SetupMod(itemName);
                            shopItems.Add(secondCopy);
                            break;
                        case 1:
                            break;
                        case 2:
                            addToShop = false;
                            break;
                    }
                }
                if (addToShop)
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

    public bool GetIsPlayerLoaded()
    {
        return isPlayerLoaded;
    }

    public int GetSaveSlot()
    {
        return saveSlot;
    }

    public int GetPlayerLevel()
    {
        return playerLevel;
    }

    public List<ShopMenu.ShopForSaleType> GetPreviousShopTypes()
    {
        return previousShopTypes;
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayerData(this);
    }

    public void LoadPlayer(int saveSlotToLoad)
    {
        SaveObject saveObject = SaveSystem.LoadPlayerData(saveSlotToLoad);

        // Recreate Owned Items
        ownedItems = new List<Item>();
        List<SaveItem> deserializedSaveItems = JsonConvert.DeserializeObject<List<SaveItem>>(saveObject.items);
        foreach (SaveItem saveItem in deserializedSaveItems)
        {
            if (saveItem.hackerOrRunner == (int)Item.HackerRunner.Hacker)
            {
                // Hacker Items
                if (saveItem.itemType == (int)Item.ItemTypes.Rig || saveItem.itemType == (int)Item.ItemTypes.NeuralImplant || saveItem.itemType == (int)Item.ItemTypes.Uplink)
                {
                    HackerMod newHackerMod = ScriptableObject.CreateInstance<HackerMod>();
                    newHackerMod.RecreateSavedHackerMod(saveItem);
                    ownedItems.Add(newHackerMod);
                } else
                {
                    HackerModChip newHackerModChip = ScriptableObject.CreateInstance<HackerModChip>();
                    newHackerModChip.RecreateSavedHackerModChip(saveItem);
                    ownedItems.Add(newHackerModChip);
                }

            } else if (saveItem.hackerOrRunner == (int)Item.HackerRunner.Runner)
            {
                // Create Runner Items
                RunnerMod newRunnerMod = ScriptableObject.CreateInstance<RunnerMod>();
                newRunnerMod.RecreateSavedRunnerMod(saveItem);
                ownedItems.Add(newRunnerMod);
            }
        }

        // Create Runners
        List<SaveRunner> saveRunners = JsonConvert.DeserializeObject<List<SaveRunner>>(saveObject.runners);
        ownedRunners = new List<CharacterData>();
        foreach (SaveRunner saveRunner in saveRunners)
        {
            CharacterData newRunner = ScriptableObject.CreateInstance<CharacterData>();
            newRunner.RecreateRunnerFromSaveData(saveRunner);

            Loadout runnerLoadout = newRunner.GetLoadout();
            runnerLoadout.EquipItem((RunnerMod)GetLoadedItemById(saveRunner.headItemId));
            runnerLoadout.EquipItem((RunnerMod)GetLoadedItemById(saveRunner.torsoModId));
            runnerLoadout.EquipItem((RunnerMod)GetLoadedItemById(saveRunner.exoskeletonModId));
            runnerLoadout.EquipItem((RunnerMod)GetLoadedItemById(saveRunner.leftArmModId), Loadout.LeftOrRight.Left);
            runnerLoadout.EquipItem((RunnerMod)GetLoadedItemById(saveRunner.rightArmModId), Loadout.LeftOrRight.Right);
            runnerLoadout.EquipItem((RunnerMod)GetLoadedItemById(saveRunner.leftLegModId), Loadout.LeftOrRight.Left);
            runnerLoadout.EquipItem((RunnerMod)GetLoadedItemById(saveRunner.rightLegModId), Loadout.LeftOrRight.Right);
            runnerLoadout.EquipItem((RunnerMod)GetLoadedItemById(saveRunner.weaponModId));

            ownedRunners.Add(newRunner);
            if (newRunner.GetRunnerId() == saveObject.currentRunner)
                currentRunner = newRunner;
        }

        List<SaveHacker> saveHackers = JsonConvert.DeserializeObject<List<SaveHacker>>(saveObject.hackers);
        ownedHackers = new List<HackerData>();
        foreach (SaveHacker saveHacker in saveHackers)
        {
            HackerData newHacker = ScriptableObject.CreateInstance<HackerData>();
            newHacker.RecreateHackerFromSave(saveHacker);

            if (!newHacker.GetIsLocked())
            {
                HackerMod newRigMod = (HackerMod)GetLoadedItemById(saveHacker.rigModId);
                if (saveHacker.rigSoftwareId1 != -1)
                    newRigMod.InstallChip((HackerModChip)GetLoadedItemById(saveHacker.rigSoftwareId1), 0);
                if (saveHacker.rigSoftwareId2 != -1)
                    newRigMod.InstallChip((HackerModChip)GetLoadedItemById(saveHacker.rigSoftwareId2), 1);
                if (saveHacker.rigSoftwareId3 != -1)
                    newRigMod.InstallChip((HackerModChip)GetLoadedItemById(saveHacker.rigSoftwareId3), 2);
                newHacker.GetHackerLoadout().EquipRecreatedItem(newRigMod);

                HackerMod newNeuralImplantMod = (HackerMod)GetLoadedItemById(saveHacker.neuralImplantId);
                if (saveHacker.neuralWetwareId1 != -1)
                    newNeuralImplantMod.InstallChip((HackerModChip)GetLoadedItemById(saveHacker.neuralWetwareId1), 0);
                if (saveHacker.neuralWetwareId2 != -1)
                    newNeuralImplantMod.InstallChip((HackerModChip)GetLoadedItemById(saveHacker.neuralWetwareId2), 1);
                if (saveHacker.neuralWetwareId3 != -1)
                    newNeuralImplantMod.InstallChip((HackerModChip)GetLoadedItemById(saveHacker.neuralWetwareId3), 2);
                newHacker.GetHackerLoadout().EquipRecreatedItem(newNeuralImplantMod);

                HackerMod newUplinkMod = (HackerMod)GetLoadedItemById(saveHacker.uplinkId);
                if (saveHacker.uplinkChipsetId1 != -1)
                    newUplinkMod.InstallChip((HackerModChip)GetLoadedItemById(saveHacker.uplinkChipsetId1), 0);
                if (saveHacker.uplinkChipsetId2 != -1)
                    newUplinkMod.InstallChip((HackerModChip)GetLoadedItemById(saveHacker.uplinkChipsetId2), 1);
                if (saveHacker.uplinkChipsetId3 != -1)
                    newUplinkMod.InstallChip((HackerModChip)GetLoadedItemById(saveHacker.uplinkChipsetId3), 2);
                newHacker.GetHackerLoadout().EquipRecreatedItem(newUplinkMod);
            }

            ownedHackers.Add(newHacker);
            if (newHacker.GetHackerId() == saveObject.currentHacker)
                currentHacker = newHacker;
        }

        // General Player Data
        currentCredits = saveObject.playerCredits;
        generatedItemId = saveObject.currentItemId;
        saveSlot = saveObject.saveSlot;

        //public string jobOptions;
        //public int currentShopType;
        //public string previousShopTypes;
        //public string itemsForSale;

        isPlayerLoaded = true;
    }

    public int GetItemId(bool increment=true)
    {
        int idToReturn = generatedItemId;

        if (increment)
            generatedItemId++;

        return idToReturn;
    }

    public Item GetLoadedItemById(int id)
    {
        foreach (Item item in ownedItems)
        {
            if (item.GetItemId() == id)
                return item;
        }

        // This should never happen
        return ScriptableObject.CreateInstance<Item>();
    }
}
