using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MissionCompleteMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI enemiesDefeatedField;
    [SerializeField] TextMeshProUGUI hacksCompletedField;
    [SerializeField] TextMeshProUGUI baseWageField;
    [SerializeField] TextMeshProUGUI multiplierField;
    [SerializeField] TextMeshProUGUI totalWagesField;
    [SerializeField] TextMeshProUGUI bonusCreditsField;
    [SerializeField] TextMeshProUGUI itemNameField;
    [SerializeField] Image itemIcon;
    [SerializeField] Image newJobsAndItems;

    public void SetupMissionCompleteMenu(Job oldJob, int creditsEarned, int goalModifier, int enemiesDefeated, int hacksCompleted)
    {
        Debug.Log("Setup Mission Complete Menu");
        enemiesDefeatedField.text = enemiesDefeated.ToString();
        hacksCompletedField.text = hacksCompleted.ToString();
        baseWageField.text = oldJob.GetRewardMoney().ToString();

        int totalWages = 0;
        if (goalModifier != 0)
        {
            multiplierField.text = "+" + goalModifier.ToString() + "%";
            totalWages = oldJob.GetRewardMoney() + oldJob.GetRewardMoney() / 100 * goalModifier;
        } else
        {
            multiplierField.text = "+0%";
            totalWages = oldJob.GetRewardMoney();
        }
        
        totalWagesField.text = totalWages.ToString();
        bonusCreditsField.text = creditsEarned.ToString();

        // Generate New Jobs
        FindObjectOfType<PlayerData>().GenerateJobOptions();

        // TODO: GENERATE NEW SHOP ITEMS
        // TODO: SAVE EARNED MONEY
        // TODO: GENERATE REWARD ITEM
    }

    public void CloseMissionCompleteMenu()
    {
        gameObject.SetActive(false);
    }
}
