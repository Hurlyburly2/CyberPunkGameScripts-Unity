using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerLoadout : ScriptableObject
{
    // HOLD THREE PIECES OF EQUIPMENT, EACH PIECE HAS ABILITIES AND SLOTS, EACH SLOT HOLDS MODS WHICH HOLD CARDS
    HackerMod rig;
    HackerMod neuralImplant;
    HackerMod uplink;
    int hackerId;

    List<HackerMod> allMods = new List<HackerMod>();

    public HackerMod GetRigMod()
    {
        return rig;
    }

    public HackerMod GetNeuralImplantMod()
    {
        return neuralImplant;
    }

    public HackerMod GetUplinkMod()
    {
        return uplink;
    }

    public void SetupInitialLoadout(int newHackerId)
    {
        hackerId = newHackerId;

        switch(hackerId)
        {
            case 0:
                rig = CreateInstance<HackerMod>();
                rig.SetupMod("Basic Rig");
                HackerModChip newSoftware = CreateInstance<HackerModChip>();
                newSoftware.SetupChip("Cheap Ghost");
                rig.InstallChip(newSoftware, 0);

                neuralImplant = CreateInstance<HackerMod>();
                neuralImplant.SetupMod("Basic Cranial Dock");
                HackerModChip newWetware = CreateInstance<HackerModChip>();
                newWetware.SetupChip("JuryRigged QwikThink");
                neuralImplant.InstallChip(newWetware, 0);

                uplink = CreateInstance<HackerMod>();
                uplink.SetupMod("Basic Uplink");
                HackerModChip newChipset = CreateInstance<HackerModChip>();
                newChipset.SetupChip("Salvaged Router");
                uplink.InstallChip(newChipset, 0);
                break;
        }
    }

    public List<int> GetCardIds()
    {
        List<int> cardIds = new List<int>();
        foreach(HackerMod hackerMod in GetAllMods())
        {
            cardIds.AddRange(hackerMod.GetCardIds());
        }
        return cardIds;
    }

    private List<HackerMod> GetAllMods()
    {
        allMods = new List<HackerMod>();
        allMods.Add(rig);
        allMods.Add(neuralImplant);
        allMods.Add(uplink);
        return allMods;
    }

    public List<HackerModChip> GetAllModChips()
    {
        List<HackerModChip> modChips = new List<HackerModChip>();
        List<HackerMod> mods = GetAllMods();

        foreach (HackerMod mod in mods)
        {
            modChips.AddRange(mod.GetAttachedChips());
        }

        return modChips;
    }

    public List<Item> GetAllEquippedModsAndChipsAsItems()
    {
        List<Item> allEquipped = new List<Item>();
        allEquipped.AddRange(GetAllMods());
        allEquipped.AddRange(GetAllModChips());
        return allEquipped;
    }

    public bool IsItemEquipped(Item item)
    {
        List<Item> equippedItems = new List<Item>();
        equippedItems.AddRange(GetAllMods());
        equippedItems.AddRange(GetAllModChips());
        foreach (Item equippedItem in equippedItems)
        {
            if (equippedItem.GetInstanceID() == item.GetInstanceID())
            {
                return true;
            }
        }
        return false;
    }

    public HackerMod GetEquippedModByItemType(Item.ItemTypes itemType)
    {
        switch (itemType)
        {
            case Item.ItemTypes.NeuralImplant:
                return neuralImplant;
            case Item.ItemTypes.Rig:
                return rig;
            case Item.ItemTypes.Uplink:
                return uplink;
        }
        // This should never happen:
        return neuralImplant;
    }

    public HackerModChip GetEquippedInstallByItemTypeAndSlotNumber(Item.ItemTypes itemType, int slotNumber)
    {
        switch (itemType)
        {
            case Item.ItemTypes.Chipset:
                return uplink.GetChipBySlot(slotNumber);
            case Item.ItemTypes.Software:
                return rig.GetChipBySlot(slotNumber);
            case Item.ItemTypes.Wetware:
                return neuralImplant.GetChipBySlot(slotNumber);
        }
        return uplink.GetChipBySlot(0); // this will break, as intended. We shouldn't reach this.
    }

    public void EquipItem(HackerMod hackerMod)
    {
        Debug.Log("Hackermod stuff");
        List<HackerModChip> equippedChips = new List<HackerModChip>();
        switch (hackerMod.GetItemType())
        {
            case Item.ItemTypes.NeuralImplant:
                equippedChips = neuralImplant.GetAttachedChips();
                if (equippedChips.Count < hackerMod.GetCurrentLevelSlotCount())
                    equippedChips.AddRange(FillEmptySlotsWithInventoryItems(Item.ItemTypes.Wetware, hackerMod.GetCurrentLevelSlotCount() - equippedChips.Count));
                neuralImplant = hackerMod;
                break;
            case Item.ItemTypes.Rig:
                equippedChips = rig.GetAttachedChips();
                if (equippedChips.Count < hackerMod.GetCurrentLevelSlotCount())
                    equippedChips.AddRange(FillEmptySlotsWithInventoryItems(Item.ItemTypes.Software, hackerMod.GetCurrentLevelSlotCount() - equippedChips.Count));
                rig = hackerMod;
                break;
            case Item.ItemTypes.Uplink:
                equippedChips = uplink.GetAttachedChips();
                if (equippedChips.Count < hackerMod.GetCurrentLevelSlotCount())
                    equippedChips.AddRange(FillEmptySlotsWithInventoryItems(Item.ItemTypes.Chipset, hackerMod.GetCurrentLevelSlotCount() - equippedChips.Count));
                uplink = hackerMod;
                break;
        }

        for (int i = 0; i < hackerMod.GetCurrentLevelSlotCount(); i++)
        {
            hackerMod.InstallChip(equippedChips[i], i);
        }
    }

    private List<HackerModChip> FillEmptySlotsWithInventoryItems(Item.ItemTypes chipType, int amountNeeded)
    {
        List<Item> allItems = FindObjectOfType<PlayerData>().GetPlayerItems();
        List<HackerModChip> unequippedModChips = new List<HackerModChip>();
        foreach (Item item in allItems)
        {
            if (item.IsHackerChipset() && item.GetItemType() == chipType && !IsItemEquipped(item))
            {
                unequippedModChips.Add(item as HackerModChip);
            }
        }

        if (unequippedModChips.Count < amountNeeded)
            unequippedModChips.AddRange(AddItemsToInventoryToFillEmptySlots(chipType, amountNeeded - unequippedModChips.Count));

        return unequippedModChips;
    }

    private List<HackerModChip> AddItemsToInventoryToFillEmptySlots(Item.ItemTypes chipType, int amountToCreate)
    {
        List<HackerModChip> installs = new List<HackerModChip>();
        PlayerData playerData = FindObjectOfType<PlayerData>();
        for (int i = 0; i < amountToCreate; i++)
        {
            switch (chipType)
            {
                case Item.ItemTypes.Wetware:
                    HackerModChip newWetware = CreateInstance<HackerModChip>();
                    newWetware.SetupChip("JuryRigged QwikThink");
                    installs.Add(newWetware);
                    playerData.AddToOwnedItems(newWetware);
                    break;
                case Item.ItemTypes.Software:
                    HackerModChip newSoftware = CreateInstance<HackerModChip>();
                    newSoftware.SetupChip("Cheap Ghost");
                    installs.Add(newSoftware);
                    playerData.AddToOwnedItems(newSoftware);
                    break;
                case Item.ItemTypes.Chipset:
                    HackerModChip newChipset = CreateInstance<HackerModChip>();
                    newChipset.SetupChip("Salvaged Router");
                    installs.Add(newChipset);
                    playerData.AddToOwnedItems(newChipset);
                    break;
            }
        }
        return installs;
    }
}
