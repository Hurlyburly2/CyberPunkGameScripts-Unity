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

        // Generate Save Items...
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

        SaveObject saveObject = new SaveObject
        {
            playerCredits = playerData.GetCreditsAmount(),
            currentRunner = playerData.GetCurrentRunner().GetRunnerId(),
            currentHacker = playerData.GetCurrentHacker().GetHackerId(),
            items = JsonConvert.SerializeObject(saveItems, Formatting.Indented),
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

    class SaveItem
    {
        public string itemName;
        public int itemId;
        public int itemType;
        public int itemLevel;
        public int hackerOrRunner;
    }

    class SaveObject
    {
        public int playerCredits;
        public int currentRunner;
        public int currentHacker;
        public string items;
        public int saveSlot;
        // OWNED RUNNERS
        // OWNED HACKERS
        // CURRENT JOB OPTIONS
        // CURRENT SHOP TYPE
        // PREVIOUS SHOP TYPES
        // ITEMS FOR SALE
    }
}