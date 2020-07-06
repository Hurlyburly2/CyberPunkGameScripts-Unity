using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapConfig : MonoBehaviour
{
    // config
    MapData mapData;
    [SerializeField] NeighboringNodeMenu neighboringNodeMenu;
    [SerializeField] CurrentNodeMenu currentNodeMenu;
    [SerializeField] MapHackMenu hackMenu;
    [SerializeField] TrapSpringMenu trapSpringMenu;
    [SerializeField] ObjectNotification objectNotificationMenu;

    // pip manager config
    [SerializeField] string healthPipManagerName = "HealthPipManager";
    [SerializeField] string energyPipManagerName = "EnergyPipManager";
    [SerializeField] string securityPipManagerName = "SecurityPipManager";
    PipManager healthPipManager;
    PipManager energyPipManager;
    PipManager securityPipManager;
    int maximumNumberOfPips = 28;
    float distanceBetweenPips = 18f;

    // text config
    [SerializeField] string healthTextFieldName = "HealthText";
    [SerializeField] string energyTextFieldName = "EnergyText";

    //state
    bool isAMenuOpen;

    private void Start()
    {
        mapData = FindObjectOfType<MapData>();
        isAMenuOpen = false;
    }

    public void SetupPipManagers(CharacterData runner, float setupTimeInSeconds, int currentSecurityLevel)
    {
        PipManager[] pipManagers = FindObjectsOfType<PipManager>();
        float maxX = GameObject.Find(healthTextFieldName).transform.position.x;
        float maxWidth = maxX - pipManagers[0].transform.position.x;

        foreach (PipManager pipManager in pipManagers)
        {
            if (pipManager.name == healthPipManagerName)
            {
                healthPipManager = pipManager;
            } else if (pipManager.name == energyPipManagerName)
            {
                energyPipManager = pipManager;
            } else if (pipManager.name == securityPipManagerName)
            {
                securityPipManager = pipManager;
            }
        }

        healthPipManager.Setup(this, runner.GetMaximumHealth(), runner.GetCurrentHealth());
        energyPipManager.Setup(this, runner.GetMaximumEnergy(), runner.GetCurrentEnergy());
        securityPipManager.Setup(this, 100, currentSecurityLevel);
    }

    public int GetMaximumNumberOfPips()
    {
        return maximumNumberOfPips;
    }

    public float GetDistanceBetweenPips()
    {
        return distanceBetweenPips;
    }

    public GameObject GetHealthTextField()
    {
        return GameObject.Find(healthTextFieldName);
    }

    public GameObject GetEnergyTextField()
    {
        return GameObject.Find(energyTextFieldName);
    }

    public string GetSecurityPipManagerName()
    {
        return securityPipManagerName;
    }

    public NeighboringNodeMenu GetNeighboringNodeMenu()
    {
        return neighboringNodeMenu;
    }

    public CurrentNodeMenu GetCurrentNodeMenu()
    {
        return currentNodeMenu;
    }

    public bool GetIsAMenuOpen()
    {
        return isAMenuOpen;
    }

    public void SetIsAMenuOpen(bool newState)
    {
        isAMenuOpen = newState;
    }

    public MapHackMenu GetMapHackMenu()
    {
        return hackMenu;
    }

    public PipManager GetSecurityPipManager()
    {
        return securityPipManager;
    }

    public PipManager GetHealthPipManager()
    {
        return healthPipManager;
    }

    public PipManager GetEnergyPipManager()
    {
        return energyPipManager;
    }

    public TrapSpringMenu GetTrapSpringMenu()
    {
        return trapSpringMenu;
    }

    public ObjectNotification GetObjectNotificationMenu()
    {
        return objectNotificationMenu;
    }
}
