using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMenuButtons : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;

    public void BackToTitleButtonClick()
    {
        Debug.Log("Back to Title Click");
    }

    public void AbandonRunButtonClick()
    {
        MapData mapData = FindObjectOfType<MapData>();
        Job job = mapData.GetJob();
        int creditsEarned = mapData.GetEarnedMoneyAmount();
        int goalModifier = mapData.GetGoalModifier();
        int enemiesDefeated = mapData.GetDefeatedEnemyCount();
        int hacksCompleted = mapData.GetCompletedHackCount();

        FindObjectOfType<SceneLoader>().LoadHubFromAbandonedMap(job, creditsEarned, goalModifier, enemiesDefeated, hacksCompleted);
    }

    public void OpenSettingsWindow()
    {
        Debug.Log("Open Settings Window");
    }

    public void OpenLoadoutWindow()
    {
        Debug.Log("Open Loadout Window");
    }

    public void OpenStatusWindow()
    {
        Debug.Log("Open Status Window");
    }

    public void CloseMenu()
    {
        FindObjectOfType<MapConfig>().SetIsAMenuOpen(false);
        mainMenu.SetActive(false);
    }

    public void OpenMenu()
    {
        FindObjectOfType<MapConfig>().SetIsAMenuOpen(true);
        mainMenu.SetActive(true);
    }
}
