using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtractionWindow : MonoBehaviour
{
    public void PressExtractionButton()
    {
        MapData mapData = FindObjectOfType<MapData>();
        Job job = mapData.GetJob();
        int creditsEarned = mapData.GetEarnedMoneyAmount();
        int goalModifier = mapData.GetGoalModifier();
        int enemiesDefeated = mapData.GetDefeatedEnemyCount();
        int hacksCompleted = mapData.GetCompletedHackCount();

        SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
        sceneLoader.LoadHubFromCompletedMap(job, creditsEarned, goalModifier, enemiesDefeated, hacksCompleted);
    }

    public void OpenExtractionWindow()
    {
        gameObject.SetActive(true);
        FindObjectOfType<MapConfig>().SetIsAMenuOpen(true);
    }
}
