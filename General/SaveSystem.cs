using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public static class SaveSystem
{
    private static readonly string SAVE_FOLDER = Application.persistentDataPath + "/Saves/";

    public static void Init()
    {
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
    }

    public static void Save(string saveString, int saveSlot)
    {
        File.WriteAllText(SAVE_FOLDER + "/save" + saveSlot + ".txt", saveString);
    }

    public static string Load(int saveSlot)
    {
        if (File.Exists(SAVE_FOLDER + "/save" + saveSlot + ".txt"))
        {
            string saveString = File.ReadAllText(SAVE_FOLDER + "/save" + saveSlot + ".txt");

            return saveString;
        }
        else
        {
            return null;
        }
    }

    public static void SavePlayerData(PlayerData playerData)
    {
        SavePrefs.SavePrefsFromPlayer(playerData);

        Init();

        // Save Items...
        List<SaveItem> saveItems = new List<SaveItem>();
        foreach (Item item in playerData.GetPlayerItems())
        {
            SaveItem newItem = new SaveItem
            {
                itemName = item.GetItemName(),
                itemId = item.GetItemId(),
                itemType = (int)item.GetItemType(),
                itemLevel = item.GetItemLevel(),
                hackerOrRunner = (int)item.GetHackerOrRunner(),
            };
            saveItems.Add(newItem);
        }

        List<SaveItem> shopItems = new List<SaveItem>();
        foreach (Item item in playerData.GetItemsForSale())
        {
            SaveItem newItem = new SaveItem
            {
                itemName = item.GetItemName(),
                itemId = item.GetItemId(),
                itemType = (int)item.GetItemType(),
                itemLevel = item.GetItemLevel(),
                hackerOrRunner = (int)item.GetHackerOrRunner(),
            };
            shopItems.Add(newItem);
        }

        // Save Runner
        List<SaveRunner> runners = new List<SaveRunner>();
        foreach (CharacterData runner in playerData.GetRunnerList())
        {
            if (runner.GetIsLocked())
            {
                SaveRunner saveRunner = new SaveRunner
                {
                    id = runner.GetRunnerId(),
                    locked = runner.GetIsLocked(),
                    bio = runner.GetBio(),
                    runnerName = runner.GetRunnerName(),
                    maximumHealth = runner.GetMaximumHealth(),
                    currentHealth = runner.GetCurrentHealth(),
                    maximumEnergy = runner.GetMaximumEnergy(),
                    currentEnergy = runner.GetCurrentEnergy(),
                    handSize = runner.GetStartingHandSize()
                };
                runners.Add(saveRunner);
            } else
            {
                SaveRunner saveRunner = new SaveRunner
                {
                    id = runner.GetRunnerId(),
                    locked = runner.GetIsLocked(),
                    bio = runner.GetBio(),
                    runnerName = runner.GetRunnerName(),
                    maximumHealth = runner.GetMaximumHealth(),
                    currentHealth = runner.GetCurrentHealth(),
                    maximumEnergy = runner.GetMaximumEnergy(),
                    currentEnergy = runner.GetCurrentEnergy(),
                    handSize = runner.GetStartingHandSize(),
                    headItemId = runner.GetLoadout().GetEquippedModByItemType(Item.ItemTypes.Head, Loadout.LeftOrRight.None).GetItemId(),
                    torsoModId = runner.GetLoadout().GetEquippedModByItemType(Item.ItemTypes.Torso, Loadout.LeftOrRight.None).GetItemId(),
                    exoskeletonModId = runner.GetLoadout().GetEquippedModByItemType(Item.ItemTypes.Exoskeleton, Loadout.LeftOrRight.None).GetItemId(),
                    leftArmModId = runner.GetLoadout().GetEquippedModByItemType(Item.ItemTypes.Arm, Loadout.LeftOrRight.Left).GetItemId(),
                    rightArmModId = runner.GetLoadout().GetEquippedModByItemType(Item.ItemTypes.Arm, Loadout.LeftOrRight.Right).GetItemId(),
                    leftLegModId = runner.GetLoadout().GetEquippedModByItemType(Item.ItemTypes.Leg, Loadout.LeftOrRight.Left).GetItemId(),
                    rightLegModId = runner.GetLoadout().GetEquippedModByItemType(Item.ItemTypes.Leg, Loadout.LeftOrRight.Right).GetItemId(),
                    weaponModId = runner.GetLoadout().GetEquippedModByItemType(Item.ItemTypes.Weapon, Loadout.LeftOrRight.None).GetItemId()
                };
                runners.Add(saveRunner);
            }
        }

        List<SaveHacker> hackers = new List<SaveHacker>();
        foreach (HackerData hacker in playerData.GetHackerList())
        {
            if (hacker.GetIsLocked())
            {
                SaveHacker saveHacker = new SaveHacker
                {
                    id = hacker.GetHackerId(),
                    hackerName = hacker.GetName(),
                    bio = hacker.GetBio(),
                    locked = hacker.GetIsLocked()
                };
                hackers.Add(saveHacker);
            } else
            {
                List<HackerModChip> rigSoftwares = hacker.GetHackerLoadout().GetRigMod().GetAttachedChips();
                List<int> rigIds = new List<int>();
                foreach (HackerModChip rigSoftware in rigSoftwares)
                    rigIds.Add(rigSoftware.GetItemId());
                while (rigIds.Count < 3)
                    rigIds.Add(-1);

                List<HackerModChip> neuralWetwares = hacker.GetHackerLoadout().GetNeuralImplantMod().GetAttachedChips();
                List<int> neuralIds = new List<int>();
                foreach (HackerModChip wetware in neuralWetwares)
                    neuralIds.Add(wetware.GetItemId());
                while (neuralIds.Count < 3)
                    neuralIds.Add(-1);

                List<HackerModChip> uplinkChipsets = hacker.GetHackerLoadout().GetUplinkMod().GetAttachedChips();
                List<int> uplinkChipsetIds = new List<int>();
                foreach (HackerModChip chipset in uplinkChipsets)
                    uplinkChipsetIds.Add(chipset.GetItemId());
                while (uplinkChipsetIds.Count < 3)
                    uplinkChipsetIds.Add(-1);

                SaveHacker saveHacker = new SaveHacker
                {
                    id = hacker.GetHackerId(),
                    hackerName = hacker.GetName(),
                    bio = hacker.GetBio(),
                    locked = hacker.GetIsLocked(),
                    rigModId = hacker.GetHackerLoadout().GetRigMod().GetItemId(),
                    rigSoftwareId1 = rigIds[0],
                    rigSoftwareId2 = rigIds[1],
                    rigSoftwareId3 = rigIds[2],
                    neuralImplantId = hacker.GetHackerLoadout().GetNeuralImplantMod().GetItemId(),
                    neuralWetwareId1 = neuralIds[0],
                    neuralWetwareId2 = neuralIds[1],
                    neuralWetwareId3 = neuralIds[2],
                    uplinkId = hacker.GetHackerLoadout().GetUplinkMod().GetItemId(),
                    uplinkChipsetId1 = uplinkChipsetIds[0],
                    uplinkChipsetId2 = uplinkChipsetIds[1],
                    uplinkChipsetId3 = uplinkChipsetIds[2]
                };
                hackers.Add(saveHacker);
            }
        }

        List<JobOption> jobOptions = new List<JobOption>();
        foreach (Job job in playerData.GetCurrentJobsList())
        {
            List<int> jobEnemyTypes = new List<int>();
            foreach (Job.EnemyType enemyType in job.GetEnemyTypes())
                jobEnemyTypes.Add((int)enemyType);

            JobOption newJobOption = new JobOption
            {
                jobName = job.GetJobName(),
                jobDescription = job.GetJobDescription(),
                jobType = (int)job.GetJobType(),
                jobArea = (int)job.GetJobArea(),
                enemyTypes = jobEnemyTypes,
                jobDifficulty = job.GetJobDifficulty(),
                jobIntroText = job.GetJobIntroText(),
                jobMiddleTextOne = job.GetJobMiddleTextOne(),
                jobMiddleTextTwo = job.GetJobMiddleTextTwo(),
                jobEndText = job.GetJobEndText(),
                rewardItemType = (int)job.GetRewardItemType(),
                rewardMoney = job.GetRewardMoney(),
                mapSize = job.GetMapSize(),
                isStoryMission = job.GetIsStoryMission()
            };
            jobOptions.Add(newJobOption);
        }

        List<int> previousShopTypesInts = new List<int>();
        foreach (ShopMenu.ShopForSaleType shopType in playerData.GetPreviousShopTypes())
        {
            previousShopTypesInts.Add((int)shopType);
        }

        SaveObject saveObject = new SaveObject
        {
            playerCredits = playerData.GetCreditsAmount(),
            currentRunner = playerData.GetCurrentRunner().GetRunnerId(),
            currentHacker = playerData.GetCurrentHacker().GetHackerId(),
            items = JsonConvert.SerializeObject(saveItems, Formatting.Indented),
            runners = JsonConvert.SerializeObject(runners, Formatting.Indented),
            hackers = JsonConvert.SerializeObject(hackers, Formatting.Indented),
            jobOptions = JsonConvert.SerializeObject(jobOptions, Formatting.Indented),
            currentShopType = (int)playerData.GetShopType(),
            previousShopTypes = JsonConvert.SerializeObject(previousShopTypesInts, Formatting.Indented),
            currentItemId = playerData.GetItemId(false),
            itemsForSale = JsonConvert.SerializeObject(shopItems, Formatting.Indented),
            saveSlot = playerData.GetSaveSlot()
        };
        string jsonString = JsonConvert.SerializeObject(saveObject, Formatting.Indented);
        Debug.Log(jsonString);

        Save(jsonString, playerData.GetSaveSlot());
    }

    public static string LoadPlayerData(int saveSlot)
    {
        Init();

        string saveString = Load(saveSlot);

        if (saveString != null)
        {
            SaveObject loadedSaveObject = JsonUtility.FromJson<SaveObject>(saveString);
            Debug.Log(loadedSaveObject.playerCredits);
            Debug.Log(loadedSaveObject.items);
            List<SaveItem> deserializedSaveItems = JsonConvert.DeserializeObject<List<SaveItem>>(loadedSaveObject.items);
            //foreach (SaveItem item in deserializedSaveItems)
            //{
            //    Debug.Log(item.itemName);
            //    Debug.Log("item id: " + item.itemId);
            //}
            Debug.Log("Player Credits: " + loadedSaveObject.playerCredits);

            return loadedSaveObject.playerCredits.ToString();
        }
        return "";
    }

    class SaveRunner
    {
        public int id;
        public bool locked;
        public string bio;
        public string runnerName;
        public int maximumHealth;
        public int currentHealth;
        public int maximumEnergy;
        public int currentEnergy;
        public int handSize;
        public int headItemId;
        public int torsoModId;
        public int exoskeletonModId;
        public int leftArmModId;
        public int rightArmModId;
        public int leftLegModId;
        public int rightLegModId;
        public int weaponModId;
    }

    class SaveHacker
    {
        public int id;
        public string hackerName;
        public string bio;
        public bool locked;
        public int rigModId;
        public int rigSoftwareId1;
        public int rigSoftwareId2;
        public int rigSoftwareId3;
        public int neuralImplantId;
        public int neuralWetwareId1;
        public int neuralWetwareId2;
        public int neuralWetwareId3;
        public int uplinkId;
        public int uplinkChipsetId1;
        public int uplinkChipsetId2;
        public int uplinkChipsetId3;
    }

    class SaveItem
    {
        public string itemName;
        public int itemId;
        public int itemType;
        public int itemLevel;
        public int hackerOrRunner;
    }

    class JobOption
    {
        public string jobName;
        public string jobDescription;
        public int jobType;
        public int jobArea;
        public List<int> enemyTypes;
        public int jobDifficulty;

        public string jobIntroText;
        public string jobMiddleTextOne;
        public string jobMiddleTextTwo;
        public string jobEndText;

        public int rewardItemType;
        public int rewardMoney;
        public int mapSize;

        public bool isStoryMission;
    }

    class SaveObject
    {
        public int playerCredits;
        public int currentRunner;
        public int currentHacker;
        public int currentItemId;
        public string items;
        public int saveSlot;
        public string runners;
        public string hackers;
        public string jobOptions;
        public int currentShopType;
        public string previousShopTypes;
        public string itemsForSale;
    }
}