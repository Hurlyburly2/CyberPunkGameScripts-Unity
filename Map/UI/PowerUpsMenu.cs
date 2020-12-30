using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsMenu : MonoBehaviour
{
    List<PowerUp> powerUps;
    [SerializeField] PowerUpListControl powerUpListControl;

    public void SetupPowerUpsMenu()
    {
        powerUps = new List<PowerUp>();
        powerUps.AddRange(FindObjectOfType<MapData>().GetPowerUps());
        powerUpListControl.GenerateList(powerUps);
    }

    public void ClosePowerUpsMenu()
    {
        FindObjectOfType<MapConfig>().SetIsAMenuOpen(false);
        powerUpListControl.ClearList();
        gameObject.SetActive(false);
    }
}
