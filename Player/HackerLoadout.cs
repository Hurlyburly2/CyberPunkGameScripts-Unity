using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerLoadout : ScriptableObject
{
    string hackerName;
    // HOLD THREE PIECES OF EQUIPMENT, EACH PIECE HAS ABILITIES AND SLOTS, EACH SLOT HOLDS MODS WHICH HOLD CARDS
    List<HackerMod> mods = new List<HackerMod>();
        // 0: Rig
        // 1: Neural Dock
        // 2: Uplink

    public void SetupInitialLoadout(string newHackerName)
    {
        hackerName = newHackerName;

        // FILL ALL OF THE MOD SLOTS HERE
    }
}
