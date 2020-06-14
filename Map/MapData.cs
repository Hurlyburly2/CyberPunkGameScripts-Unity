using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
    // config
    CharacterData runner;
    HackerData hacker;
    string mapType;
        // possible: "city"
    MapConfig mapConfig;
    [SerializeField] float setupTimeInSeconds = 1f;

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
    }

    public void SetCharacterData(CharacterData characterToSet, HackerData hackerToSet, string newMapType, int newSecurityLevel)
    {
        runner = characterToSet;
        hacker = hackerToSet;
        mapType = newMapType;
        securityLevel = newSecurityLevel;
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
}
