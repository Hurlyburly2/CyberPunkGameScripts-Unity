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

    public void CloseMainMenu()
    {
        gameObject.SetActive(false);
    }

    public void OpenJobSelectMenu()
    {
        jobSelectMenu.gameObject.SetActive(true);
        jobSelectMenu.SetupMenu();
    }

    public void OpenLoadoutMenu()
    {
        loadoutMenu.gameObject.SetActive(true);
        loadoutMenu.SetupLoadoutMenu();
    }

    public void OpenInventoryMenu()
    {
        inventoryMenu.gameObject.SetActive(true);
        inventoryMenu.SetupInventoryMenu();
    }

    public void OpenShopMenu()
    {
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
