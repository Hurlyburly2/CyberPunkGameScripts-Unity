using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerMod : Item
{
    int slots;
    int abilityUses;
    int activeAbilityId;

    List<HackerModChip> modChips = new List<HackerModChip>();

    public void SetupMod(string newModName)
    {
        itemName = newModName;
        GetModProperties();
        itemLevel = 1;
        itemMaxLevel = 5;
        hackerOrRunner = HackerRunner.Hacker;
    }

    private void GetModProperties()
    {
        switch(itemName)
        {
            // ABILITY FUNCTIONALITY IS LISTED IN ABILITYBUTTON.CS
            // TODO: MUST WRITE ALL FUNCTIONALITY FOR NEW ABILITIES IN THERE
            // TODO: PERHAPS WE CAN PASS A FUNCTION FROM HERE TO IT INSTEAD OF SEPARATING THINGS
            case "Basic Rig":
                slots = 1;
                itemType = ItemTypes.Rig;
                abilityUses = 1;
                activeAbilityId = 0;
                itemDescription = "Ol' reliable. Last decade's Cyris900 still gets the job done... usually.";
                levelOneItemAbilityDescription = "Add one Red connection to your active card.";
                levelTwoItemAbilityDescription = "Add one Red connection to your active card.";
                levelThreeItemAbilityDescription = "Add one Red connection to your active card.";
                levelFourItemAbilityDescription = "Add one Red connection to your active card.";
                levelFiveItemAbilityDescription = "Add one Red connection to your active card.";
                break;
            case "Basic Cranial Dock":
                slots = 1;
                itemType = ItemTypes.NeuralImplant;
                abilityUses = 1;
                activeAbilityId = 1;
                itemDescription = "One of the early cognition receiver models. Running it too long gives you a headache.";
                levelOneItemAbilityDescription = "For your next action, pick from your top two cards. Discard the other.";
                levelTwoItemAbilityDescription = "For your next action, pick from your top two cards. Discard the other.";
                levelThreeItemAbilityDescription = "For your next action, pick from your top two cards. Discard the other.";
                levelFourItemAbilityDescription = "For your next action, pick from your top two cards. Discard the other.";
                levelFiveItemAbilityDescription = "For your next action, pick from your top two cards. Discard the other.";
                break;
            case "Basic Uplink":
                slots = 1;
                itemType = ItemTypes.Uplink;
                activeAbilityId = 2;
                abilityUses = 2;
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
                // Add a R connection to the active card
                AddConnectionsToActiveCard(1, "red");
                break;
            case "Basic Cranial Dock":
                // For your next action, pick from your top two cards. Discard the other.
                SelectFromTopOfDeck(2, 1);
                break;
            case "Basic Uplink":
                // Add the top card of your discard to the top of your deck
                MoveCardsFromDiscardToDeck(1);
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
        for (int i = 0; i < slots; i++)
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
        return abilityUses;
    }

    public List<HackerModChip> GetAttachedChips()
    {
        return modChips;
    }

    public int GetMaxSlotCount()
    {
        return slots;
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
}
