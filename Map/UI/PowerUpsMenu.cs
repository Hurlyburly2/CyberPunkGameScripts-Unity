using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsMenu : MonoBehaviour
{
    List<PowerUp> powerUps;
    [SerializeField] PowerUpListControl powerUpListControl;

    MapSFX mapSFX;

    private void Awake()
    {
        mapSFX = FindObjectOfType<MapSFX>();
    }

    public void SetupPowerUpsMenu()
    {
        powerUps = new List<PowerUp>();
        powerUps.AddRange(FindObjectOfType<MapData>().GetPowerUps());
        powerUpListControl.GenerateList(powerUps);
    }

    public void ClosePowerUpsMenu()
    {
        mapSFX.PlayMapSoundSFX(MapSFX.MapSoundEffect.ButtonPress);
        FindObjectOfType<MapConfig>().SetIsAMenuOpen(false);
        powerUpListControl.ClearList();
        gameObject.SetActive(false);
    }
}
