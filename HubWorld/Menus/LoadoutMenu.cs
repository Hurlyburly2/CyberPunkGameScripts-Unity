using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadoutMenu : MonoBehaviour
{
    [SerializeField] LoadoutEquipmentMenu loadoutEquipmentMenu;
    [SerializeField] JobSelectMenu jobSelectMenu;

    [SerializeField] Image runnerPortraitImage;
    [SerializeField] TextMeshProUGUI runnerName;
    [SerializeField] Image hackerPortraitImage;
    [SerializeField] TextMeshProUGUI hackerName;

    [SerializeField] CharacterSelectMenu characterSelectMenu;

    PlayerData playerData;
    CharacterData currentRunner;
    HackerData currentHacker;

    ItemDetailsMenu.ItemDetailMenuContextType context;

    public void SetupLoadoutMenu()
    {
        RefreshPlayerData();
    }

    public void SetupLoadoutMenu(ItemDetailsMenu.ItemDetailMenuContextType newContext)
    {
        context = newContext;
        SetupLoadoutMenu();
    }

    private void RefreshPlayerData()
    {
        PlayerData playerData = FindObjectOfType<PlayerData>();
        currentRunner = playerData.GetCurrentRunner();
        currentHacker = playerData.GetCurrentHacker();

        string runnerImagePath = "Characters/Runner" + currentRunner.getId() + "-Full";
        runnerPortraitImage.sprite = Resources.Load<Sprite>(runnerImagePath);
        runnerName.text = currentRunner.GetRunnerName();

        string hackerImagePath = "Characters/Hacker" + currentHacker.GetId() + "-Full";
        hackerPortraitImage.sprite = Resources.Load<Sprite>(hackerImagePath);
        hackerName.text = currentHacker.GetName();
    }

    private void OpenCharacterSelectMenu(Item.HackerRunner hackerRunner)
    {
        characterSelectMenu.gameObject.SetActive(true);
        characterSelectMenu.SetupCharacterSelectMenu(hackerRunner);
    }

    public void OpenCharacterSelectMenuHacker()
    {
        OpenCharacterSelectMenu(Item.HackerRunner.Hacker);
    }

    public void OpenCharacterSelectMenuRunner()
    {
        OpenCharacterSelectMenu(Item.HackerRunner.Runner);
    }

    public void CloseLoadoutMenu()
    {
        gameObject.SetActive(false);
        if (context == ItemDetailsMenu.ItemDetailMenuContextType.JobSelect)
        {
            context = ItemDetailsMenu.ItemDetailMenuContextType.Inventory;
            jobSelectMenu.SetupMenu();
        }
    }

    public void OpenLoadoutEquipmentMenuRunner()
    {
        loadoutEquipmentMenu.gameObject.SetActive(true);
        loadoutEquipmentMenu.SetupLoadoutEquipmentMenu(currentRunner);
    }

    public void OpenLoadoutEquipmentMenuHacker()
    {
        loadoutEquipmentMenu.gameObject.SetActive(true);
        loadoutEquipmentMenu.SetupLoadoutEquipmentMenu(currentHacker);
    }
}
