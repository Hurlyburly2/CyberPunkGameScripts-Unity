using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHackMenu : MonoBehaviour
{
    HackTarget hackTarget;

    public void InitializeMapHackMenu(HackTarget newHackTarget)
    {
        hackTarget = newHackTarget;
    }
}
