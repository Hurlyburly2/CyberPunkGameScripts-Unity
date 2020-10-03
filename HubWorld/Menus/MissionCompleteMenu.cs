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

        int totalWages;
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
        PlayerData playerData = FindObjectOfType<PlayerData>();
        playerData.GenerateJobOptions();

        // Save earned money
        playerData.CreditsGain(totalWages);
        playerData.CreditsGain(creditsEarned);

        // TODO: GENERATE NEW SHOP ITEMS - This will be handled on playerData
        // TODO: GENERATE REWARD ITEM
    }

    public void CloseMissionCompleteMenu()
    {
        gameObject.SetActive(false);
    }
}
