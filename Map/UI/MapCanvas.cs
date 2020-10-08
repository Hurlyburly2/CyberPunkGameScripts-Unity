using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCanvas : MonoBehaviour
{
    [SerializeField] MissionStartMenu missionStartMenu;
    [SerializeField] PreBossGoalWindow preBossGoalWindow;

    public MissionStartMenu GetMissionStartMenu()
    {
        return missionStartMenu;
    }

    public PreBossGoalWindow GetPreBossGoalWindow()
    {
        return preBossGoalWindow;
    }
}
