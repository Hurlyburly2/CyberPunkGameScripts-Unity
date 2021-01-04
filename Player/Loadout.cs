using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loadout : ScriptableObject
{
    public enum LeftOrRight { None, Left, Right };
    int runnerId;

    RunnerMod headMod;
    RunnerMod torsoMod;
    RunnerMod exoskeletonMod;
    RunnerMod leftArm;
    RunnerMod rightArm;
    RunnerMod leftLeg;
    RunnerMod rightLeg;
    RunnerMod weapon;

    public void SetupInitialLoadout(int newRunnerId)
    {
        runnerId = newRunnerId;

        switch(runnerId)
        {
            case 0:
                headMod = CreateInstance<RunnerMod>();
                headMod.SetupMod("Human Eyes");

                torsoMod = CreateInstance<RunnerMod>();
                torsoMod.SetupMod("Unmodded Torso");

                exoskeletonMod = CreateInstance<RunnerMod>();
                exoskeletonMod.SetupMod("Human Skin");

                leftArm = CreateInstance<RunnerMod>();
                leftArm.SetupMod("Unmodded Arm");

                rightArm = CreateInstance<RunnerMod>();
                rightArm.SetupMod("Unmodded Arm");

                leftLeg = CreateInstance<RunnerMod>();
                leftLeg.SetupMod("Unmodded Leg");

                rightLeg = CreateInstance<RunnerMod>();
                rightLeg.SetupMod("Unmodded Leg");

                weapon = CreateInstance<RunnerMod>();
                weapon.SetupMod("Spanner");
                break;
        }
    }

    public void EquipItem(RunnerMod newMod)
    {
        switch (newMod.GetItemType())
        {
            case Item.ItemTypes.Arm:
                // TODO: DO THIS DIFFERENTLY IN AN OVERLOADED FUNCTION
                break;
            case Item.ItemTypes.Exoskeleton:
                exoskeletonMod = newMod;
                break;
            case Item.ItemTypes.Head:
                headMod = newMod;
                break;
            case Item.ItemTypes.Leg:
                // TODO: DO THIS DIFFERENTLY IN AN OVERLOADED FUNCTION WITH LEFT/RIGHT
                break;
            case Item.ItemTypes.Torso:
                torsoMod = newMod;
                break;
            case Item.ItemTypes.Weapon:
                weapon = newMod;
                break;
        }
    }

    public void EquipItem(RunnerMod newMod, LeftOrRight leftOrRight)
    {
        switch (newMod.GetItemType())
        {
            case Item.ItemTypes.Arm:
                if (leftOrRight == LeftOrRight.Left)
                    leftArm = newMod;
                else if (leftOrRight == LeftOrRight.Right)
                    rightArm = newMod;
                break;
            case Item.ItemTypes.Leg:
                if (leftOrRight == LeftOrRight.Left)
                    leftLeg = newMod;
                else if (leftOrRight == LeftOrRight.Right)
                    rightLeg = newMod;
                break;
        }
    }

    public List<int> GetAllCardIds()
    {
        RunnerMod[] allMods = GetAllMods();
        List<int> allCardIds = new List<int>();

        foreach(RunnerMod mod in allMods)
        {
            allCardIds.AddRange(mod.GetCardIds());
        }

        return allCardIds;
    }

    public List<int> GetAllCardIds(List<Item.ItemTypes> disabledMods)
    {
        RunnerMod[] allMods = GetAllMods();
        List<int> allCardIds = new List<int>();

        foreach(RunnerMod mod in allMods)
        {
            if (!disabledMods.Contains(mod.GetItemType()))
                allCardIds.AddRange(mod.GetCardIds());
        }

        return allCardIds;
    }

    private RunnerMod[] GetAllMods()
    {
        RunnerMod[] allMods = new RunnerMod[] { headMod, torsoMod, exoskeletonMod, leftArm, rightArm, leftLeg, rightLeg, weapon };
        return allMods;
    }

    public List<Item> GetModsAsItems()
    {
        return new List<Item>(GetAllMods());
    }

    public RunnerMod GetEquippedModByItemType(Item.ItemTypes itemType, Loadout.LeftOrRight leftOrRight)
    {
        switch (itemType)
        {
            case Item.ItemTypes.Arm:
                if (leftOrRight == Loadout.LeftOrRight.Left)
                    return leftArm;
                else
                    return rightArm;
            case Item.ItemTypes.Exoskeleton:
                return exoskeletonMod;
            case Item.ItemTypes.Head:
                return headMod;
            case Item.ItemTypes.Leg:
                if (leftOrRight == Loadout.LeftOrRight.Left)
                    return leftLeg;
                else
                    return rightLeg;
            case Item.ItemTypes.Torso:
                return torsoMod;
            case Item.ItemTypes.Weapon:
                return weapon;
        }
        // we shouldn't ever hit this...
        return weapon;
    }

    public bool IsItemEquipped(RunnerMod modToCheck)
    {
        RunnerMod[] runnerMods = GetAllMods();
        foreach (RunnerMod mod in runnerMods)
        {
            if (mod.GetInstanceID() == modToCheck.GetInstanceID())
                return true;
        }
        return false;
    }
}
