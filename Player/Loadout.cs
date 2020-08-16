﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loadout : ScriptableObject
{
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
}
