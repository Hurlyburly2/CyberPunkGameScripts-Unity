using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstMenu : MonoBehaviour
{
    [SerializeField] Button continueButton;
    [SerializeField] SaveSlotMenu saveSlotMenu;

    HubWorldSFX hubWorldSFX;

    public void SetupFirstMenu()
    {
        hubWorldSFX = FindObjectOfType<HubWorldSFX>();
        if (SavePrefs.GetMostRecentlyUsedSaveSlot() != 0)
            continueButton.interactable = true;
        else
            continueButton.interactable = false;
    }

    public void ContinueButtonPress()
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.ButtonPress);
        PlayerData playerData = FindObjectOfType<PlayerData>();
        playerData.LoadPlayer(SavePrefs.GetMostRecentlyUsedSaveSlot());
        FindObjectOfType<HubMenuButton>().OpenMenu();
        gameObject.SetActive(false);
    }

    public void NewGameBtnPress()
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.ButtonPress);
        saveSlotMenu.gameObject.SetActive(true);
        saveSlotMenu.SetupSaveSlotMenu(true);
        gameObject.SetActive(false);
    }

    public void LoadGameBtnPress()
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.ButtonPress);
        saveSlotMenu.gameObject.SetActive(true);
        saveSlotMenu.SetupSaveSlotMenu(false);
        gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void CloseFirstMenu()
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.ButtonPress);
        gameObject.SetActive(false);
    }
}
