using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerLoadout : ScriptableObject
{
    string hackerName;
    // HOLD THREE PIECES OF EQUIPMENT, EACH PIECE HAS ABILITIES AND SLOTS, EACH SLOT HOLDS MODS WHICH HOLD CARDS
    HackerMod rig;
    HackerMod neuralImplant;
    HackerMod uplink;

    List<HackerMod> allMods = new List<HackerMod>();

    public void SetupInitialLoadout(string newHackerName)
    {
        hackerName = newHackerName;

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
}
