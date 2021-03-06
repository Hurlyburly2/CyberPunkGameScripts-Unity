﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSlotMenu : MonoBehaviour
{
    [SerializeField] FirstMenu firstMenu;
    [SerializeField] List<SaveSlotUI> saveSlotUIs;
    [SerializeField] NotificationMenu notificationMenu;

    HubWorldSFX hubWorldSFX;
    bool newGame;

    private void Awake()
    {
        hubWorldSFX = FindObjectOfType<HubWorldSFX>();
    }

    public void SetupSaveSlotMenu(bool newGameFromFirstMenu)
    {
        newGame = newGameFromFirstMenu;

        int slotNumber = 0;
        foreach (SaveSlotUI saveSlot in saveSlotUIs)
        {
            slotNumber++;
            saveSlot.SetupSaveSlot(slotNumber, newGameFromFirstMenu, this);
        }
    }

    public void DeleteSaveBySlot(int slotNumber)
    {
        SaveSystem.DeleteSaveBySlot(slotNumber);
        SetupSaveSlotMenu(newGame);
    }

    public void CloseSaveSelectMenu()
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.ButtonPress);
        firstMenu.gameObject.SetActive(true);
        firstMenu.SetupFirstMenu();
        gameObject.SetActive(false);
    }

    public NotificationMenu GetNotificationMenu()
    {
        return notificationMenu;
    }
}
