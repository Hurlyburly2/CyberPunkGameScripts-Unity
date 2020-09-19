using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MissionCompleteMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI enemiesDefeated;
    [SerializeField] TextMeshProUGUI hacksCompleted;
    [SerializeField] TextMeshProUGUI baseWage;
    [SerializeField] TextMeshProUGUI multiplier;
    [SerializeField] TextMeshProUGUI totalWages;
    [SerializeField] TextMeshProUGUI bonusCredits;
    [SerializeField] TextMeshProUGUI imageName;
    [SerializeField] Image itemIcon;
    [SerializeField] Image newJobsAndItems;

    public void SetupMissionCompleteMenu()
    {
        Debug.Log("Setup Mission Complete Menu");
    }

    public void CloseMissionCompleteMenu()
    {
        gameObject.SetActive(false);
    }
}
