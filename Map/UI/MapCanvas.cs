using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCanvas : MonoBehaviour
{
    [SerializeField] MissionStartMenu missionStartMenu;
    [SerializeField] PreBossGoalWindow preBossGoalWindow;
    [SerializeField] PowerUpsMenu powerUpsMenu;

    MapSFX mapSFX;

    private void Awake()
    {
        mapSFX = FindObjectOfType<MapSFX>();
    }

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
        mapSFX.PlayMapSoundSFX(MapSFX.MapSoundEffect.ButtonPress);
        FindObjectOfType<MapConfig>().SetIsAMenuOpen(true);
        powerUpsMenu.gameObject.SetActive(true);
        powerUpsMenu.SetupPowerUpsMenu();
    }
}
