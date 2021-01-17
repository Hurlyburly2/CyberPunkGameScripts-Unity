﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstMenu : MonoBehaviour
{
    [SerializeField] Button continueButton;
    [SerializeField] SaveSlotMenu saveSlotMenu;

    public void SetupFirstMenu()
    {
        if (SavePrefs.GetMostRecentlyUsedSaveSlot() != 0)
            continueButton.interactable = true;
        else
            continueButton.interactable = false;
    }

    public void ContinueButtonPress()
    {

    }

    public void NewGameBtnPress()
    {
        // TODO: REMOVE THIS
        FindObjectOfType<PlayerData>().SetupNewGame(1);
        saveSlotMenu.gameObject.SetActive(true);
        saveSlotMenu.SetupSaveSlotMenu(true);
        gameObject.SetActive(false);
    }

    public void LoadGameBtnPress()
    {
        saveSlotMenu.gameObject.SetActive(true);
        saveSlotMenu.SetupSaveSlotMenu();
        gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void CloseFirstMenu()
    {
        gameObject.SetActive(false);
    }
}