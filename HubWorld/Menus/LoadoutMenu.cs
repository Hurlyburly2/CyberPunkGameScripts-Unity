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

    HubWorldSFX hubWorldSFX;

    private void Awake()
    {
        hubWorldSFX = FindObjectOfType<HubWorldSFX>();
    }

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

        runnerPortraitImage.sprite = currentRunner.GetRunnerPortraitFull();
        runnerName.text = currentRunner.GetRunnerName();

        hackerPortraitImage.sprite = currentHacker.GetHackerPortraitFull();
        hackerName.text = currentHacker.GetName();
    }

    private void OpenCharacterSelectMenu(Item.HackerRunner hackerRunner)
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.ButtonPress);
        characterSelectMenu.gameObject.SetActive(true);
        characterSelectMenu.SetupCharacterSelectMenu(hackerRunner);
    }

    public void OpenCharacterSelectMenuHacker()
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.ButtonPress);
        OpenCharacterSelectMenu(Item.HackerRunner.Hacker);
    }

    public void OpenCharacterSelectMenuRunner()
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.ButtonPress);
        OpenCharacterSelectMenu(Item.HackerRunner.Runner);
    }

    public void CloseLoadoutMenu()
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.ButtonPress);
        gameObject.SetActive(false);
        if (context == ItemDetailsMenu.ItemDetailMenuContextType.JobSelect)
        {
            context = ItemDetailsMenu.ItemDetailMenuContextType.Inventory;
            jobSelectMenu.SetupMenu();
        }
    }

    public void OpenLoadoutEquipmentMenuRunner()
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.ButtonPress);
        loadoutEquipmentMenu.gameObject.SetActive(true);
        loadoutEquipmentMenu.SetupLoadoutEquipmentMenu(currentRunner);
    }

    public void OpenLoadoutEquipmentMenuHacker()
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.ButtonPress);
        loadoutEquipmentMenu.gameObject.SetActive(true);
        loadoutEquipmentMenu.SetupLoadoutEquipmentMenu(currentHacker);
    }
}
