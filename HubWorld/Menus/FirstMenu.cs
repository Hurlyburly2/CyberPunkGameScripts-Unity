using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstMenu : MonoBehaviour
{
    [SerializeField] Button continueButton;
    [SerializeField] SaveSlotMenu saveSlotMenu;

    HubWorldSFX hubWorldSFX;

    private void Awake()
    {
        hubWorldSFX = FindObjectOfType<HubWorldSFX>();
    }

    public void SetupFirstMenu()
    {
        if (SavePrefs.GetMostRecentlyUsedSaveSlot() != 0)
            continueButton.interactable = true;
        else
            continueButton.interactable = false;
    }

    public void ContinueButtonPress()
    {
        PlayButtonSound();
        PlayerData playerData = FindObjectOfType<PlayerData>();
        playerData.LoadPlayer(SavePrefs.GetMostRecentlyUsedSaveSlot());
        FindObjectOfType<HubMenuButton>().OpenMenu();
        gameObject.SetActive(false);
    }

    public void NewGameBtnPress()
    {
        PlayButtonSound();
        saveSlotMenu.gameObject.SetActive(true);
        saveSlotMenu.SetupSaveSlotMenu(true);
        gameObject.SetActive(false);
    }

    public void LoadGameBtnPress()
    {
        PlayButtonSound();
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
        PlayButtonSound();
        gameObject.SetActive(false);
    }

    private void PlayButtonSound()
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.ButtonPress);
    }
}
