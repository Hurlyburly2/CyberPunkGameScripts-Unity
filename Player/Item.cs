using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    // items can be hackermods, hackerModInstalls, or runnermods
    // this class is for assistance with inventory screens and such, holding common information that all items have
    public enum HackerRunner { Hacker, Runner };
    public enum ItemTypes { Head, Torso, Exoskeleton, Arm, Leg, Weapon, Rig, NeuralImplant, Uplink, Software, Wetware, Chipset };

    protected int itemId = 1000;
    protected string itemName;
    protected HackerRunner hackerOrRunner;
        // potential types: hacker, runner
    protected ItemTypes itemType;
    // potential types:
    // runnerMods: Head, Torso, Exoskeleton, LeftArm, RightArm, LeftLeg, RightLeg, Weapon
    // hackerMods: Rig, NeuralImplant, Uplink
    // hackerModInstalls: Software, Wetware, Chipset
    protected int itemLevel;
    protected int itemMaxLevel;

    public void PrintItemId()
    {
        Debug.Log("Item Id: " + itemId);
    }

    public HackerRunner GetHackerOrRunner()
    {
        return hackerOrRunner;
    }

    public string GetItemName()
    {
        return itemName;
    }

    public ItemTypes GetItemType()
    {
        return itemType;
    }

    public int GetItemLevel()
    {
        return itemLevel;
    }

    public int GetItemMaxLevel()
    {
        return itemMaxLevel;
    }

    public string GetItemTypeForDisplay()
    {
        switch(itemType)
        {
            case ItemTypes.Head:
                return "Head Mod";
            case ItemTypes.Torso:
                return "Torso Mod";
            case ItemTypes.Exoskeleton:
                return "Exoskeleton";
            case ItemTypes.Arm:
                return "Arm Mod";
            case ItemTypes.Leg:
                return "Leg Mod";
            case ItemTypes.Weapon:
                return "Weapon";
            case ItemTypes.Rig:
                return "Rig";
            case ItemTypes.NeuralImplant:
                return "Neural Implant";
            case ItemTypes.Uplink:
                return "Uplink";
            case ItemTypes.Software:
                return "Software";
            case ItemTypes.Wetware:
                return "Wetware";
            case ItemTypes.Chipset:
                return "Chipset";
        }
        return "";
    }
}
