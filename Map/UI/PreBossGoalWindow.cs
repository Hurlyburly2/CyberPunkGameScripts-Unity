using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PreBossGoalWindow : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textField;

    public void OpenPreBossGoalWindow(Job job)
    {
        gameObject.SetActive(true);
        textField.text = job.GetJobMiddleTextOne();
    }

    public void ClosePreBossGoalWindow()
    {
        gameObject.SetActive(false);
        FindObjectOfType<MapData>().StartBattleIfEnemyExists(FindObjectOfType<PlayerMarker>().GetCurrentSquare());
    }
}
