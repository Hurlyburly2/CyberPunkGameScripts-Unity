using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SavePrefs
{
    const string MOST_RECENTLY_USED_SAVE_SLOT = "MOST_RECENTLY_USED_SAVE_SLOT";

    const string SAVE_SLOT_1_CHAPTER = "SAVE_SLOT_1_CHAPTER";
    const string SAVE_SLOT_2_CHAPTER = "SAVE_SLOT_2_CHAPTER";
    const string SAVE_SLOT_3_CHAPTER = "SAVE_SLOT_3_CHAPTER";

    public static string GetSaveSlotArea(int saveSlot)
    {
        switch (saveSlot)
        {
            case 1:
                if (PlayerPrefs.HasKey(SAVE_SLOT_1_CHAPTER))
                    return PlayerPrefs.GetString(SAVE_SLOT_1_CHAPTER);
                return "";
            case 2:
                if (PlayerPrefs.HasKey(SAVE_SLOT_2_CHAPTER))
                    return PlayerPrefs.GetString(SAVE_SLOT_2_CHAPTER);
                return "";
            case 3:
                if (PlayerPrefs.HasKey(SAVE_SLOT_3_CHAPTER))
                    return PlayerPrefs.GetString(SAVE_SLOT_3_CHAPTER);
                return "";
        }
        return "";
    }

    public static int GetMostRecentlyUsedSaveSlot()
    {
        if (PlayerPrefs.HasKey(MOST_RECENTLY_USED_SAVE_SLOT))
            return PlayerPrefs.GetInt(MOST_RECENTLY_USED_SAVE_SLOT);
        return 0;
    }

    public static void SavePrefsFromPlayer(PlayerData playerData)
    {
        PlayerPrefs.SetInt(MOST_RECENTLY_USED_SAVE_SLOT, playerData.GetSaveSlot());
    }
}