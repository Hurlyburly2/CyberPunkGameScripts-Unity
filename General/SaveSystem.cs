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

    public static void Save(string saveString)
    {
        File.WriteAllText(SAVE_FOLDER + "/save.txt", saveString);
    }

    public static string Load()
    {
        if (File.Exists(SAVE_FOLDER + "/save.txt"))
        {
            string saveString = File.ReadAllText(SAVE_FOLDER + "/save.txt");

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
                itemType = (int)item.GetItemType(),
                itemLevel = item.GetItemLevel(),
                hackerOrRunner = (int)item.GetHackerOrRunner()
            };
            saveItems.Add(newItem);
        }

        SaveObject saveObject = new SaveObject
        {
            playerCredits = playerData.GetCreditsAmount(),
            items = JsonConvert.SerializeObject(saveItems, Formatting.Indented)
        };
        string jsonString = JsonConvert.SerializeObject(saveObject, Formatting.Indented);
        Debug.Log(jsonString);

        Save(jsonString);
    }

    public static string LoadPlayerData()
    {
        Init();

        string saveString = Load();

        if (saveString != null)
        {
            SaveObject loadedSaveObject = JsonUtility.FromJson<SaveObject>(saveString);
            Debug.Log(loadedSaveObject.playerCredits);
            Debug.Log(loadedSaveObject.items);
            List<SaveItem> deserializedSaveItems = JsonConvert.DeserializeObject<List<SaveItem>>(loadedSaveObject.items);
            foreach (SaveItem item in deserializedSaveItems)
            {
                Debug.Log(item.itemName);
                Debug.Log(item.itemLevel);
                Debug.Log(item.hackerOrRunner);
                Debug.Log(item.itemType);
            }

            return loadedSaveObject.playerCredits.ToString();
        }
        return "";
    }

    class SaveItem
    {
        public string itemName;
        public int itemType;
        public int itemLevel;
        public int hackerOrRunner;
    }

    class SaveObject
    {
        public int playerCredits;
        public string items;
    }
}