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
}
