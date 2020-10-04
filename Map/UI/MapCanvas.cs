using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCanvas : MonoBehaviour
{
    [SerializeField] MissionStartMenu missionStartMenu;

    public MissionStartMenu GetMissionStartMenu()
    {
        return missionStartMenu;
    }
}
