using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loadout : ScriptableObject
{
    string runnerName;
    string hackerName;

    RunnerMod headMod;
    RunnerMod torsoMod;
    RunnerMod exoskeletonMod;
    RunnerMod leftArm;
    RunnerMod rightArm;
    RunnerMod leftLeg;
    RunnerMod rightLeg;
    RunnerMod weapon;

    public void SetupInitialLoadout(string newRunnerName, string newHackerName)
    {
        runnerName = newRunnerName;
        hackerName = newHackerName;

        headMod = new RunnerMod();
        headMod.SetupMod("Human Eyes");

        torsoMod = new RunnerMod();
        torsoMod.SetupMod("Unmodded Torso");

        exoskeletonMod = new RunnerMod();
        exoskeletonMod.SetupMod("Human Skin");

        leftArm = new RunnerMod();
        leftArm.SetupMod("Unmodded Arm");

        rightArm = new RunnerMod();
        rightArm.SetupMod("Unmodded Arm");

        leftLeg = new RunnerMod();
        leftLeg.SetupMod("Unmodded Leg");

        rightLeg = new RunnerMod();
        rightLeg.SetupMod("Unmodded Leg");

        weapon = new RunnerMod();
        weapon.SetupMod("Spanner");
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
}
