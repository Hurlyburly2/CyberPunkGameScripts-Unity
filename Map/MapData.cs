using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
    // config
    CharacterData runner;
    HackerData hacker;
    string mapType;
        // possible: "slums"
    MapConfig mapConfig;
    [SerializeField] float setupTimeInSeconds = 1f;
    MapGrid mapGrid;
    int mapSize;

    // state
    int securityLevel;

    private void Awake()
    {
        int count = FindObjectsOfType<MapData>().Length;

        if (count > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void SetUpMap()
    {
        SetupPlayerPortraits();
        runner.MapSetup();

        mapConfig = FindObjectOfType<MapConfig>();
        mapConfig.SetupPipManagers(runner, setupTimeInSeconds, securityLevel);

        mapGrid = FindObjectOfType<MapGrid>();
        mapGrid.SetupGrid(mapType, mapSize);
    }

    public void SetMapData(CharacterData characterToSet, HackerData hackerToSet, string newMapType, int newSecurityLevel, int newMapSize)
    {
        runner = characterToSet;
        hacker = hackerToSet;
        mapType = newMapType;
        securityLevel = newSecurityLevel;
        mapSize = newMapSize;
    }

    private void SetupPlayerPortraits()
    {
        PlayerPortrait[] portraits = FindObjectsOfType<PlayerPortrait>();
        foreach (PlayerPortrait portrait in portraits)
        {
            if (portrait.name == "RunnerPortrait")
            {
                portrait.SetPortrait(runner.GetRunnerName());
            }
            else if (portrait.name == "HackerPortrait")
            {
                portrait.SetPortrait(hacker.GetName());
            }
        }
    }

    public float GetSetupTimeInSeconds()
    {
        return setupTimeInSeconds;
    }

    public string GetMapType()
    {
        return mapType;
    }
}
