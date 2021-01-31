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

    [SerializeField] Sprite jobCompleteSprite;
    [SerializeField] Sprite jobFailedSprite;
    [SerializeField] Image jobStatusTxtImg;

    HubWorldSFX hubWorldSFX;

    private void Awake()
    {
        hubWorldSFX = FindObjectOfType<HubWorldSFX>();
    }

    public void SetupMissionCompleteMenu(Job oldJob, int creditsEarned, int goalModifier, int enemiesDefeated, int hacksCompleted, bool victory)
    {
        Debug.Log("Setup Mission Complete Menu");
        enemiesDefeatedField.text = enemiesDefeated.ToString();
        hacksCompletedField.text = hacksCompleted.ToString();

        int totalWages;
        int baseWages;
        if (goalModifier != 0)
        {
            multiplierField.text = "+" + goalModifier.ToString() + "%";
            totalWages = oldJob.GetRewardMoney() + oldJob.GetRewardMoney() / 100 * goalModifier;
        } else
        {
            multiplierField.text = "+0%";
            totalWages = oldJob.GetRewardMoney();
        }

        // Victory vs Defeat stuff
        if (victory)
        {
            jobStatusTxtImg.sprite = jobCompleteSprite;
            baseWages = oldJob.GetRewardMoney();
        }
        else
        {
            jobStatusTxtImg.sprite = jobFailedSprite;
            totalWages = 0;
            baseWages = 0;
        }

        totalWagesField.text = totalWages.ToString();
        bonusCreditsField.text = creditsEarned.ToString();
        baseWageField.text = baseWages.ToString();

        // Generate New Jobs
        PlayerData playerData = FindObjectOfType<PlayerData>();
        playerData.GenerateJobOptions();

        // Save earned money
        playerData.CreditsGain(totalWages);
        playerData.CreditsGain(creditsEarned);

        // Generate New Shop and Shop Items
        playerData.GenerateNewShop();

        // TODO: GENERATE REWARD ITEM
    }

    public void CloseMissionCompleteMenu()
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.ButtonPress);
        gameObject.SetActive(false);
    }
}
