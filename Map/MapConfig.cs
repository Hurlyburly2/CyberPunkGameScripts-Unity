using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapConfig : MonoBehaviour
{
    // config
    MapData mapData;

    // pip manager config

    // text config
    [SerializeField] string healthTextFieldName = "HealthText";
    [SerializeField] string energyTextFieldName = "EnergyText";

    private void Start()
    {
        mapData = FindObjectOfType<MapData>();
    }

    public void SetupPipManagers(CharacterData runner, float setupTimeInSeconds, int currentSecurityLevel)
    {

    }

    public GameObject GetHealthTextField()
    {
        return GameObject.Find(healthTextFieldName);
    }

    public GameObject GetEnergyTextField()
    {
        return GameObject.Find(energyTextFieldName);
    }
}
