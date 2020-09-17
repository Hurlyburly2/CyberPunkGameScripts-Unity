using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] JobSelectMenu jobSelectMenu;
    [SerializeField] LoadoutMenu loadoutMenu;
    [SerializeField] InventoryMenu inventoryMenu;
    [SerializeField] ShopMenu shopMenu;

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
}
