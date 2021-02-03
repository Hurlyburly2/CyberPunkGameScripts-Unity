using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PreBossGoalWindow : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textField;

    MapSFX mapSFX;

    private void Awake()
    {
        mapSFX = FindObjectOfType<MapSFX>();
    }

    public void OpenPreBossGoalWindow(Job job)
    {
        gameObject.SetActive(true);
        textField.text = job.GetJobMiddleTextOne();
    }

    public void ClosePreBossGoalWindow()
    {
        mapSFX.PlayMapSoundSFX(MapSFX.MapSoundEffect.ButtonPress);
        gameObject.SetActive(false);
        FindObjectOfType<MapData>().StartBattleIfEnemyExists(FindObjectOfType<PlayerMarker>().GetCurrentSquare());
    }
}
