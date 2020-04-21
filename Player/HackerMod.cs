using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerMod : ScriptableObject
{
    string modName;
    string type;
        // Rig, Neural Dock, or Uplink
    int slots;
    List<HackerModChip> modChips = new List<HackerModChip>();


}
