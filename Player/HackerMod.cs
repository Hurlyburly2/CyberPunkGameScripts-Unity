using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerMod : Item
{
    int level1Slots;
    int level2Slots;
    int level3Slots;
    int level4Slots;
    int level5Slots;

    int level1AbilityUses;
    int level2AbilityUses;
    int level3AbilityUses;
    int level4AbilityUses;
    int level5AbilityUses;

    int activeAbilityId;

    List<HackerModChip> modChips = new List<HackerModChip>();

    public void SetupMod(string newModName)
    {
        itemName = newModName;
        GetModProperties();
        itemLevel = 1;
        itemMaxLevel = 5;
        hackerOrRunner = HackerRunner.Hacker;
        CreateItemId();
    }

    private void GetModProperties()
    {
        switch(itemName)
        {
            // ABILITY FUNCTIONALITY IS LISTED IN ABILITYBUTTON.CS
            // TODO: MUST WRITE ALL FUNCTIONALITY FOR NEW ABILITIES IN THERE
            // TODO: PERHAPS WE CAN PASS A FUNCTION FROM HERE TO IT INSTEAD OF SEPARATING THINGS
            case "Basic Rig":
                level1Slots = 1;
                level2Slots = 1;
                level3Slots = 2;
                level4Slots = 2;
                level5Slots = 3;
                itemType = ItemTypes.Rig;
                level1AbilityUses = 1;
                level2AbilityUses = 1;
                level3AbilityUses = 1;
                level4AbilityUses = 2;
                level5AbilityUses = 2;
                activeAbilityId = 0;
                itemDescription = "Ol' reliable. Last decade's Cyris900 still gets the job done... usually.";
                levelOneItemAbilityDescription = "Add one Red connection to your active card.";
                levelTwoItemAbilityDescription = "Add one Red connection to your active card.";
                levelThreeItemAbilityDescription = "Add one Red connection to your active card.";
                levelFourItemAbilityDescription = "Add one Red connection to your active card.";
                levelFiveItemAbilityDescription = "Add one Red connection to your active card.";
                break;
            case "Basic Cranial Dock":
                level1Slots = 1;
                level2Slots = 1;
                level3Slots = 1;
                level4Slots = 2;
                level5Slots = 2;
                itemType = ItemTypes.NeuralImplant;
                level1AbilityUses = 1;
                level2AbilityUses = 1;
                level3AbilityUses = 2;
                level4AbilityUses = 2;
                level5AbilityUses = 3;
                activeAbilityId = 1;
                itemDescription = "One of the early cognition receiver models. Running it too long gives you a headache.";
                levelOneItemAbilityDescription = "For your next action, pick from your top two cards. Discard the other.";
                levelTwoItemAbilityDescription = "For your next action, pick from your top two cards. Discard the other.";
                levelThreeItemAbilityDescription = "For your next action, pick from your top two cards. Discard the other.";
                levelFourItemAbilityDescription = "For your next action, pick from your top two cards. Discard the other.";
                levelFiveItemAbilityDescription = "For your next action, pick from your top two cards. Discard the other.";
                break;
            case "Basic Uplink":
                level1Slots = 1;
                level2Slots = 1;
                level3Slots = 2;
                level4Slots = 2;
                level5Slots = 3;
                itemType = ItemTypes.Uplink;
                activeAbilityId = 2;
                level1AbilityUses = 2;
                level2AbilityUses = 2;
                level3AbilityUses = 2;
                level4AbilityUses = 2;
                level5AbilityUses = 3;
                itemDescription = "A salvaged crypto-crawler. It's a little bit jank.";
                levelOneItemAbilityDescription = "Put the top card of your discard back onto the top of your deck.";
                levelTwoItemAbilityDescription = "Put the top card of your discard back onto the top of your deck.";
                levelThreeItemAbilityDescription = "Put the top card of your discard back onto the top of your deck.";
                levelFourItemAbilityDescription = "Put the top card of your discard back onto the top of your deck.";
                levelFiveItemAbilityDescription = "Put the top card of your discard back onto the top of your deck.";
                break;
            default:
                break;
        }
        SetupEmptyMods();
    }

    public void UseAbility()
    {
        switch(itemName)
        {
            case "Basic Rig":
                switch (itemLevel)
                {
                    case 1:
                        // Add a R connection to the active card
                        AddConnectionsToActiveCard(1, "red");
                        break;
                    case 2:
                        AddConnectionsToActiveCard(1, "red");
                        break;
                    case 3:
                        AddConnectionsToActiveCard(1, "red");
                        break;
                    case 4:
                        AddConnectionsToActiveCard(1, "red");
                        break;
                    case 5:
                        AddConnectionsToActiveCard(1, "red");
                        break;
                }
                break;
            case "Basic Cranial Dock":
                // For your next action, pick from your top two cards. Discard the other.
                switch (itemLevel)
                {
                    case 1:
                        SelectFromTopOfDeck(2, 1);
                        break;
                    case 2:
                        SelectFromTopOfDeck(2, 1);
                        break;
                    case 3:
                        SelectFromTopOfDeck(2, 1);
                        break;
                    case 4:
                        SelectFromTopOfDeck(2, 1);
                        break;
                    case 5:
                        SelectFromTopOfDeck(2, 1);
                        break;
                }
                break;
            case "Basic Uplink":
                // Add the top card of your discard to the top of your deck
                switch (itemLevel)
                {
                    case 1:
                        MoveCardsFromDiscardToDeck(1);
                        break;
                    case 2:
                        MoveCardsFromDiscardToDeck(1);
                        break;
                    case 3:
                        MoveCardsFromDiscardToDeck(1);
                        break;
                    case 4:
                        MoveCardsFromDiscardToDeck(1);
                        break;
                    case 5:
                        MoveCardsFromDiscardToDeck(1);
                        break;
                }
                break;
        }
    }

    public List<int> GetCardIds()
    {
        List<int> cardIds = new List<int>();
        foreach(HackerModChip modChip in modChips)
        {
            cardIds.AddRange(modChip.GetCardIds());
        }
        return cardIds;
    }

    private void SetupEmptyMods()
    {
        for (int i = 0; i < GetMaxSlotCount(); i++)
        {
            HackerModChip emptyMod = CreateInstance<HackerModChip>();
            emptyMod.SetupChip("Empty");
            modChips.Add(emptyMod);
        }
    }

    public void InstallChip(HackerModChip newHackerModChip, int slot)
    {
        // check we're installing the right kind of chip in the mod
        ItemTypes newChipType = newHackerModChip.GetItemType();
        switch(itemType)
        {
            case ItemTypes.Rig:
                if (newChipType != ItemTypes.Software)
                {
                    Debug.LogError("Can only install software in Rig");
                }
                break;
            case ItemTypes.NeuralImplant:
                if (newChipType != ItemTypes.Wetware)
                {
                    Debug.LogError("Can only install wetware in Neural Implant");
                }
                break;
            case ItemTypes.Uplink:
                if (newChipType != ItemTypes.Chipset)
                {
                    Debug.LogError("Can only install chipsets in Uplink");
                }
                break;
        }
        modChips[slot] = newHackerModChip;
    }

    public int GetActiveAbilityId()
    {
        return activeAbilityId;
    }

    public int GetActiveAbilityUses()
    {
        return level1AbilityUses;
    }

    public List<HackerModChip> GetAttachedChips()
    {
        return modChips;
    }

    public int GetMaxSlotCount()
    {
        switch (itemLevel)
        {
            case 1:
                return level1Slots;
            case 2:
                return level2Slots;
            case 3:
                return level3Slots;
            case 4:
                return level4Slots;
            case 5:
                return level5Slots;
            default:
                return level1Slots;
        }
    }

    // HACK ABILITY FUNCTIONALITY

    private void AddConnectionsToActiveCard(int number, string color)
    {
        HackDeck hackDeck = FindObjectOfType<HackDeck>();
        if (hackDeck.GetCardCount() > 0)
        {
            hackDeck.AddConnectionsToActiveCard(number, color);
        }
    }

    private void AddSpikesToActiveCard()
    {
        Debug.Log("Add spikes to active card");
    }

    private void MoveCardsFromDiscardToDeck(int numberOfCards)
    {
        FindObjectOfType<HackDiscard>().SendCardsFromDiscardToTopOfDeck(numberOfCards);
    }

    private void SelectFromTopOfDeck(int pickFromHowMany, int pickHowMany)
    {
        List<HackCard> cardsToPickFrom = FindObjectOfType<HackDeck>().GetTopXHackCards(pickFromHowMany);

        if (cardsToPickFrom.Count > 0)
        {
            FindObjectOfType<CheckClickController>().SetTilePickerState();
            HackTilePicker hackTilePicker = FindObjectOfType<HackHolder>().GetHackTilePicker();
            hackTilePicker.gameObject.SetActive(true);
            hackTilePicker.Initialize(cardsToPickFrom, pickHowMany, "pickAndDiscard");
        }
    }

    public int GetCurrentLevelSlotCount()
    {
        switch (itemLevel)
        {
            case 1:
                return level1Slots;
            case 2:
                return level2Slots;
            case 3:
                return level3Slots;
            case 4:
                return level4Slots;
            case 5:
                return level5Slots;
        }
        return 0;
    }

    public int GetLevel1SlotCount()
    {
        return level1Slots;
    }

    public int GetLevel2SlotCount()
    {
        return level2Slots;
    }

    public int GetLevel3SlotCount()
    {
        return level3Slots;
    }

    public int GetLevel4SlotCount()
    {
        return level4Slots;
    }

    public int GetLevel5SlotCount()
    {
        return level5Slots;
    }

    public int GetLevel1AbilityUses()
    {
        return level1AbilityUses;
    }

    public int GetLevel2AbilityUses()
    {
        return level2AbilityUses;
    }

    public int GetLevel3AbilityUses()
    {
        return level3AbilityUses;
    }

    public int GetLevel4AbilityUses()
    {
        return level4AbilityUses;
    }

    public int GetLevel5AbilityUses()
    {
        return level5AbilityUses;
    }
}
