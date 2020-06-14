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
        PlayerPortrait[] portraits = FindObjectsOfType<PlayerPortrait>();
        foreach (PlayerPortrait portrait in portraits)
        {
            if (portrait.name == "RunnerPortrait")
            {
                portrait.SetPortrait(runner.GetRunnerName());
            } else if (portrait.name == "HackerPortrait")
            {
                portrait.SetPortrait(hacker.GetName());
            }
        }
    }

    public void SetCharacterData(CharacterData characterToSet, HackerData hackerToSet, string newMapType)
    {
        runner = characterToSet;
        hacker = hackerToSet;
        mapType = newMapType;
    }
}
