using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SavePrefs
{
    const string MOST_RECENTLY_USED_SAVE_SLOT = "MOST_RECENTLY_USED_SAVE_SLOT";

    const string SAVE_SLOT_1_CHAPTER = "SAVE_SLOT_1_CHAPTER";
    const string SAVE_SLOT_1_CREDITS = "SAVE_SLOT_1_CREDITS";
    const string SAVE_SLOT_1_CURRENTRUNNER_ID = "SAVE_SLOT_1_CURRENTRUNNER_ID";
    const string SAVE_SLOT_1_CURRENTHACKER_ID = "SAVE_SLOT_1_CURRENTHACKER_ID";

    const string SAVE_SLOT_2_CHAPTER = "SAVE_SLOT_2_CHAPTER";
    const string SAVE_SLOT_2_CREDITS = "SAVE_SLOT_2_CREDITS";
    const string SAVE_SLOT_2_CURRENTRUNNER_ID = "SAVE_SLOT_2_CURRENTRUNNER_ID";
    const string SAVE_SLOT_2_CURRENTHACKER_ID = "SAVE_SLOT_2_CURRENTHACKER_ID";

    const string SAVE_SLOT_3_CHAPTER = "SAVE_SLOT_3_CHAPTER";
    const string SAVE_SLOT_3_CREDITS = "SAVE_SLOT_3_CREDITS";
    const string SAVE_SLOT_3_CURRENTRUNNER_ID = "SAVE_SLOT_3_CURRENTRUNNER_ID";
    const string SAVE_SLOT_3_CURRENTHACKER_ID = "SAVE_SLOT_3_CURRENTHACKER_ID";

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
        string chapterName = "";
        switch (playerData.GetPlayerLevel())
        {
            case 0:
                chapterName = "Prologue";
                break;
            case 1:
                chapterName = "Chapter 1";
                break;
            default:
                chapterName = "Chapter " + playerData.GetPlayerLevel().ToString();
                break;
        }
        string creditsAmount = playerData.GetCreditsAmount().ToString();
        int currentRunnerId = playerData.GetCurrentRunner().GetRunnerId();
        int currentHackerId = playerData.GetCurrentHacker().GetHackerId();

        switch(playerData.GetSaveSlot())
        {
            case 1:
                PlayerPrefs.SetString(SAVE_SLOT_1_CHAPTER, chapterName);
                PlayerPrefs.SetString(SAVE_SLOT_1_CREDITS, creditsAmount);
                PlayerPrefs.SetInt(SAVE_SLOT_1_CURRENTRUNNER_ID, currentRunnerId);
                PlayerPrefs.SetInt(SAVE_SLOT_1_CURRENTHACKER_ID, currentHackerId);
                break;
            case 2:
                PlayerPrefs.SetString(SAVE_SLOT_2_CHAPTER, chapterName);
                PlayerPrefs.SetString(SAVE_SLOT_2_CREDITS, creditsAmount);
                PlayerPrefs.SetInt(SAVE_SLOT_2_CURRENTRUNNER_ID, currentRunnerId);
                PlayerPrefs.SetInt(SAVE_SLOT_2_CURRENTHACKER_ID, currentHackerId);
                break;
            case 3:
                PlayerPrefs.SetString(SAVE_SLOT_3_CHAPTER, chapterName);
                PlayerPrefs.SetString(SAVE_SLOT_3_CREDITS, creditsAmount);
                PlayerPrefs.SetInt(SAVE_SLOT_3_CURRENTRUNNER_ID, currentRunnerId);
                PlayerPrefs.SetInt(SAVE_SLOT_3_CURRENTHACKER_ID, currentHackerId);
                break;
        }
    }

    public static void DeleteSavePrefsForSaveSlot(int saveSlot)
    {
        if (GetMostRecentlyUsedSaveSlot() == saveSlot)
            PlayerPrefs.DeleteKey(MOST_RECENTLY_USED_SAVE_SLOT);

        switch (saveSlot)
        {
            case 1:
                PlayerPrefs.SetString(SAVE_SLOT_1_CHAPTER, "");
                PlayerPrefs.SetString(SAVE_SLOT_1_CREDITS, "0");
                PlayerPrefs.SetInt(SAVE_SLOT_1_CURRENTRUNNER_ID, 0);
                PlayerPrefs.SetInt(SAVE_SLOT_1_CURRENTHACKER_ID, 0);
                break;
            case 2:
                Debug.Log("test test test");
                PlayerPrefs.SetString(SAVE_SLOT_2_CHAPTER, "");
                PlayerPrefs.SetString(SAVE_SLOT_2_CREDITS, "0");
                PlayerPrefs.SetInt(SAVE_SLOT_2_CURRENTRUNNER_ID, 0);
                PlayerPrefs.SetInt(SAVE_SLOT_2_CURRENTHACKER_ID, 0);
                break;
            case 3:
                PlayerPrefs.SetString(SAVE_SLOT_3_CHAPTER, "");
                PlayerPrefs.SetString(SAVE_SLOT_3_CREDITS, "0");
                PlayerPrefs.SetInt(SAVE_SLOT_3_CURRENTRUNNER_ID, 0);
                PlayerPrefs.SetInt(SAVE_SLOT_3_CURRENTHACKER_ID, 0);
                break;
        }
    }

    public static string GetCurrentChapterString(int saveSlot)
    {
        switch (saveSlot) {
            case 1:
                return PlayerPrefs.GetString(SAVE_SLOT_1_CHAPTER);
            case 2:
                return PlayerPrefs.GetString(SAVE_SLOT_2_CHAPTER);
            case 3:
                return PlayerPrefs.GetString(SAVE_SLOT_3_CHAPTER);
        }
        return "";
    }

    public static string GetCreditsAmount(int saveSlot)
    {
        switch (saveSlot)
        {
            case 1:
                return PlayerPrefs.GetString(SAVE_SLOT_1_CREDITS);
            case 2:
                return PlayerPrefs.GetString(SAVE_SLOT_2_CREDITS);
            case 3:
                return PlayerPrefs.GetString(SAVE_SLOT_3_CREDITS);
        }
        return "";
    }

    public static int GetCurrentRunnerId(int saveSlot)
    {
        switch (saveSlot)
        {
            case 1:
                return PlayerPrefs.GetInt(SAVE_SLOT_1_CURRENTRUNNER_ID);
            case 2:
                return PlayerPrefs.GetInt(SAVE_SLOT_2_CURRENTRUNNER_ID);
            case 3:
                return PlayerPrefs.GetInt(SAVE_SLOT_3_CURRENTRUNNER_ID);
        }
        return 999;
    }

    public static int GetCurrentHackerId(int saveSlot)
    {
        switch (saveSlot)
        {
            case 1:
                return PlayerPrefs.GetInt(SAVE_SLOT_1_CURRENTHACKER_ID);
            case 2:
                return PlayerPrefs.GetInt(SAVE_SLOT_2_CURRENTHACKER_ID);
            case 3:
                return PlayerPrefs.GetInt(SAVE_SLOT_3_CURRENTHACKER_ID);
        }
        return 999;
    }
}