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

        SaveObject saveObject = new SaveObject
        {
            playerCredits = playerData.GetCreditsAmount(),
            items = JsonConvert.SerializeObject(playerData.GetPlayerItems().ToArray(), Formatting.Indented),
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
            List<Item> deserializedItems = JsonConvert.DeserializeObject<List<Item>>(loadedSaveObject.items);
            foreach (Item item in deserializedItems)
            {
                Debug.Log(item.GetItemName());
                Debug.Log(item.GetItemLevel());
            }

            return loadedSaveObject.playerCredits.ToString();
        }
        return "";
    }

    class SaveObject
    {
        public int playerCredits;
        public string items;
    }
}