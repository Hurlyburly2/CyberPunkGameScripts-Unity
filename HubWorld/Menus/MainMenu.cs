using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] JobSelectMenu jobSelectMenu;
    [SerializeField] LoadoutMenu loadoutMenu;
    [SerializeField] InventoryMenu inventoryMenu;
    [SerializeField] ShopMenu shopMenu;
    [SerializeField] TextMeshProUGUI loadTestText;

    HubWorldSFX hubWorldSFX;

    private void Awake()
    {
        hubWorldSFX = FindObjectOfType<HubWorldSFX>();
    }

    public void CloseMainMenu()
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.ButtonPress);
        gameObject.SetActive(false);
    }

    public void OpenJobSelectMenu()
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.ButtonPress);
        jobSelectMenu.gameObject.SetActive(true);
        jobSelectMenu.SetupMenu();
    }

    public void OpenLoadoutMenu()
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.ButtonPress);
        loadoutMenu.gameObject.SetActive(true);
        loadoutMenu.SetupLoadoutMenu();
    }

    public void OpenInventoryMenu()
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.ButtonPress);
        inventoryMenu.gameObject.SetActive(true);
        inventoryMenu.SetupInventoryMenu();
    }

    public void OpenShopMenu()
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.ButtonPress);
        shopMenu.gameObject.SetActive(true);
        shopMenu.SetupShopMenu();
    }

    public void QuitGame()
    {
        // Test if this actually works on ipad
        Application.Quit();
    }

    public void SaveGame()
    {
        PlayerData playerData = FindObjectOfType<PlayerData>();
        playerData.SavePlayer();
    }

    public void LoadGame()
    {
        PlayerData playerData = FindObjectOfType<PlayerData>();
        playerData.LoadPlayer(1);
    }
}
