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

    public void PrintItemId()
    {
        Debug.Log("Item Id: " + itemId);
    }

    public string GetItemName()
    {
        return itemName;
    }

    public ItemTypes GetItemType()
    {
        return itemType;
    }
}
