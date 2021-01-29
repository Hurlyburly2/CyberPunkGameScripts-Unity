using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryMenu : MonoBehaviour
{
    [SerializeField] Sprite[] filterButtonsOn;
    [SerializeField] Sprite[] filterButtonsOff;

    [SerializeField] Image runnerFilterImage;
    [SerializeField] Image hackerFilterImage;
    [SerializeField] Image modFilterImage;
    [SerializeField] Image installFilterImage;

    // Filters
    bool runnerFilterOn = true;
    bool hackerFilterOn = true;
    bool modFilterOn = true;
    bool installFilterOn = true;
    List<Item.ItemTypes> currentFilters = new List<Item.ItemTypes>();

    // Fields
    public enum InventoryFields { None, Name, Type, Lvl };
    InventoryFields[] fields = { InventoryFields.Name, InventoryFields.Type, InventoryFields.Lvl };

    [SerializeField] InventoryList inventoryList;

    HubWorldSFX hubWorldSFX;

    private void Awake()
    {
        hubWorldSFX = FindObjectOfType<HubWorldSFX>();
    }

    public void SetupInventoryMenu()
    {
        ResetFilters();
        SetFilterButtonImages();
        SetupInventoryList();
    }

    private void ResetFilters()
    {
        Item.ItemTypes[] resetFilters = {
            Item.ItemTypes.Arm,
            Item.ItemTypes.Chipset,
            Item.ItemTypes.Exoskeleton,
            Item.ItemTypes.Head,
            Item.ItemTypes.Leg,
            Item.ItemTypes.NeuralImplant,
            Item.ItemTypes.Rig,
            Item.ItemTypes.Software,
            Item.ItemTypes.Torso,
            Item.ItemTypes.Uplink,
            Item.ItemTypes.Weapon,
            Item.ItemTypes.Wetware
        };
        currentFilters = new List<Item.ItemTypes>();
        currentFilters.AddRange(resetFilters);
    }

    private void SetupInventoryList()
    {
        inventoryList.DestroyListItems();

        CheckFilters();

        List<Item> items = FindObjectOfType<PlayerData>().GetPlayerItems();
        List<Item> filteredItems = new List<Item>();
        foreach (Item item in items)
        {
            if (currentFilters.Contains(item.GetItemType()))
            {
                filteredItems.Add(item);
            }
        }
        inventoryList.SetupInventoryList(fields, filteredItems, ItemDetailsMenu.ItemDetailMenuContextType.Inventory);
    }

    private void CheckFilters()
    {
        Item.ItemTypes[] types = {
            Item.ItemTypes.Arm,
            Item.ItemTypes.Chipset,
            Item.ItemTypes.Exoskeleton,
            Item.ItemTypes.Head,
            Item.ItemTypes.Leg,
            Item.ItemTypes.NeuralImplant,
            Item.ItemTypes.Rig,
            Item.ItemTypes.Software,
            Item.ItemTypes.Torso,
            Item.ItemTypes.Uplink,
            Item.ItemTypes.Weapon,
            Item.ItemTypes.Wetware
        };
        List<Item.ItemTypes> filters = new List<Item.ItemTypes>();
        filters.AddRange(types);

        if (!runnerFilterOn)
        {
            Item.ItemTypes[] runnerTypes = {
                Item.ItemTypes.Arm,
                Item.ItemTypes.Exoskeleton,
                Item.ItemTypes.Head,
                Item.ItemTypes.Leg,
                Item.ItemTypes.Torso,
                Item.ItemTypes.Weapon
            };
            filters = RemoveFromFilterIfExists(filters, runnerTypes);
        }
        if (!hackerFilterOn)
        {
            Item.ItemTypes[] hackerTypes = {
                Item.ItemTypes.Chipset,
                Item.ItemTypes.NeuralImplant,
                Item.ItemTypes.Rig,
                Item.ItemTypes.Software,
                Item.ItemTypes.Uplink,
                Item.ItemTypes.Wetware
            };
            filters = RemoveFromFilterIfExists(filters, hackerTypes);
        }
        if (!modFilterOn)
        {
            Item.ItemTypes[] modTypes = {
                Item.ItemTypes.Arm,
                Item.ItemTypes.Exoskeleton,
                Item.ItemTypes.Head,
                Item.ItemTypes.Leg,
                Item.ItemTypes.NeuralImplant,
                Item.ItemTypes.Rig,
                Item.ItemTypes.Torso,
                Item.ItemTypes.Uplink,
                Item.ItemTypes.Weapon
            };
            filters = RemoveFromFilterIfExists(filters, modTypes);
        }
        if (!installFilterOn)
        {
            Item.ItemTypes[] installTypes = {
                Item.ItemTypes.Chipset,
                Item.ItemTypes.Software,
                Item.ItemTypes.Wetware
            };
            filters = RemoveFromFilterIfExists(filters, installTypes);
        }
        currentFilters = filters;
    }

    private List<Item.ItemTypes> RemoveFromFilterIfExists(List<Item.ItemTypes> types, Item.ItemTypes[] typesToRemove)
    {
        foreach (Item.ItemTypes type in typesToRemove)
        {
            if (types.Contains(type))
                types.Remove(type);
        }
        return types;
    }

    public void CloseInventoryMenu()
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.ButtonPress);
        inventoryList.DestroyListItems();
        gameObject.SetActive(false);
    }

    private void AddToFilters(List<Item.ItemTypes> typesToAdd)
    {
        foreach (Item.ItemTypes itemType in typesToAdd)
        {
            if (currentFilters.Contains(itemType))
                break;
            currentFilters.Add(itemType);
        }
    }

    private void RemoveFromFilters(List<Item.ItemTypes> typesToRemove)
    {
        foreach (Item.ItemTypes itemType in typesToRemove)
        {
            if (currentFilters.Contains(itemType))
                currentFilters.Remove(itemType);
        }
    }

    public void PressRunnerFilterBtn()
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.ButtonPress);
        if (runnerFilterOn)
        {
            runnerFilterOn = false;
        } else
        {
            runnerFilterOn = true;
        }
        SetFilterButtonImages();
        SetupInventoryList();
    }

    public void PressHackerFilterBtn()
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.ButtonPress);
        if (hackerFilterOn)
        {
            hackerFilterOn = false;
        } else
        {
            hackerFilterOn = true;
        }
        SetFilterButtonImages();
        SetupInventoryList();
    }

    public void PressModFilterBtn()
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.ButtonPress);
        if (modFilterOn)
        {
            modFilterOn = false;
        } else
        {
            modFilterOn = true;
        }
        SetFilterButtonImages();
        SetupInventoryList();
    }

    public void PressInstallFilterBtn()
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.ButtonPress);
        if (installFilterOn)
        {
            installFilterOn = false;
        } else
        {
            installFilterOn = true;
        }
        SetFilterButtonImages();
        SetupInventoryList();
    }

    private void SetFilterButtonImages()
    {
        if (runnerFilterOn)
        {
            runnerFilterImage.sprite = filterButtonsOn[0];
        } else
        {
            runnerFilterImage.sprite = filterButtonsOff[0];
        }

        if (hackerFilterOn)
        {
            hackerFilterImage.sprite = filterButtonsOn[1];
        } else
        {
            hackerFilterImage.sprite = filterButtonsOff[1];
        }

        if (modFilterOn)
        {
            modFilterImage.sprite = filterButtonsOn[2];
        } else
        {
            modFilterImage.sprite = filterButtonsOff[2];
        }

        if (installFilterOn)
        {
            installFilterImage.sprite = filterButtonsOn[3];
        } else
        {
            installFilterImage.sprite = filterButtonsOff[3];
        }
    }
}
