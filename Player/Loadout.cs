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
                Debug.Log("left arm id: " + leftArm.GetInstanceID());

                rightArm = CreateInstance<RunnerMod>();
                rightArm.SetupMod("Unmodded Arm");
                Debug.Log("right arm id: " + rightArm.GetInstanceID());

                leftLeg = CreateInstance<RunnerMod>();
                leftLeg.SetupMod("Unmodded Leg");

                rightLeg = CreateInstance<RunnerMod>();
                rightLeg.SetupMod("Unmodded Leg");

                weapon = CreateInstance<RunnerMod>();
                weapon.SetupMod("Spanner");
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
}
