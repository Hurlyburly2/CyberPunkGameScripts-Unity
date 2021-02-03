using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMenuButtons : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;

    MapSFX mapSFX;

    private void Awake()
    {
        mapSFX = FindObjectOfType<MapSFX>();
    }

    public void BackToTitleButtonClick()
    {
        Debug.Log("Back to Title Click");
    }

    public void AbandonRunButtonClick()
    {
        // TODO: play a fail sound here!
        MapData mapData = FindObjectOfType<MapData>();
        Job job = mapData.GetJob();
        int creditsEarned = mapData.GetEarnedMoneyAmount();
        int goalModifier = mapData.GetGoalModifier();
        int enemiesDefeated = mapData.GetDefeatedEnemyCount();
        int hacksCompleted = mapData.GetCompletedHackCount();

        mainMenu.SetActive(false);
        FindObjectOfType<SceneLoader>().LoadHubFromAbandonedMap(job, creditsEarned, goalModifier, enemiesDefeated, hacksCompleted);
    }

    public void OpenSettingsWindow()
    {
        mapSFX.PlayMapSoundSFX(MapSFX.MapSoundEffect.ButtonPress);
        Debug.Log("Open Settings Window");
    }

    public void OpenLoadoutWindow()
    {
        mapSFX.PlayMapSoundSFX(MapSFX.MapSoundEffect.ButtonPress);
        Debug.Log("Open Loadout Window");
    }

    public void OpenStatusWindow()
    {
        mapSFX.PlayMapSoundSFX(MapSFX.MapSoundEffect.ButtonPress);
        Debug.Log("Open Status Window");
    }

    public void CloseMenu()
    {
        mapSFX.PlayMapSoundSFX(MapSFX.MapSoundEffect.ButtonPress);
        FindObjectOfType<MapConfig>().SetIsAMenuOpen(false);
        mainMenu.SetActive(false);
    }

    public void OpenMenu()
    {
        mapSFX.PlayMapSoundSFX(MapSFX.MapSoundEffect.ButtonPress);
        FindObjectOfType<MapConfig>().SetIsAMenuOpen(true);
        mainMenu.SetActive(true);
    }
}
