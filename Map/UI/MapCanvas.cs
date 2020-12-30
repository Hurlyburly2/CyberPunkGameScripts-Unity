using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCanvas : MonoBehaviour
{
    [SerializeField] MissionStartMenu missionStartMenu;
    [SerializeField] PreBossGoalWindow preBossGoalWindow;
    [SerializeField] PowerUpsMenu powerUpsMenu;

    public MissionStartMenu GetMissionStartMenu()
    {
        return missionStartMenu;
    }

    public PreBossGoalWindow GetPreBossGoalWindow()
    {
        return preBossGoalWindow;
    }

    public void OpenPowerUpsMenu()
    {
        powerUpsMenu.gameObject.SetActive(true);
    }
}
