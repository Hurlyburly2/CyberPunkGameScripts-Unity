using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JobSelectMenu : MonoBehaviour
{
    [SerializeField] Image runnerPortrait;
    [SerializeField] Image hackerPortrait;
    [SerializeField] LoadoutMenu loadoutMenu;
    [SerializeField] List<JobSelectSquare> jobSelectSquares;
    [SerializeField] TextMeshProUGUI jobName;
    [SerializeField] TextMeshProUGUI rewardField;
    [SerializeField] TextMeshProUGUI jobDescriptionField;

    HubWorldSFX hubWorldSFX;

    private void Awake()
    {
        hubWorldSFX = FindObjectOfType<HubWorldSFX>();
    }

    public void SetupMenu()
    {
        PlayerData playerData = FindObjectOfType<PlayerData>();
        runnerPortrait.sprite = playerData.GetCurrentRunner().GetRunnerPortraitSmall();
        hackerPortrait.sprite = playerData.GetCurrentHacker().GetHackerPortraitSmall();

        SetupJobButtons(playerData.GetCurrentJobsList());
        jobSelectSquares[1].ButtonPress(); // Select the middle square to populate the info fields
    }

    private void SetupJobButtons(List<Job> currentJobsList)
    {
        int counter = 0;
        foreach (JobSelectSquare jobSelectSquare in jobSelectSquares)
        {
            jobSelectSquare.SetupJobSelectSquare(currentJobsList[counter]);
            counter++;
        }
    }

    public void LaunchMission()
    {
        SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
        Job selectedJob = GetSelectedJob();
        sceneLoader.LoadMap(selectedJob.GetJobArea(), selectedJob.GetMapSize(), selectedJob);
    }

    private Job GetSelectedJob()
    {
        foreach (JobSelectSquare jobSelectSquare in jobSelectSquares)
        {
            if (jobSelectSquare.GetIsSelected())
                return jobSelectSquare.GetJob();
        }
        return ScriptableObject.CreateInstance<Job>();
    }

    public void OpenLoadoutMenu()
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.ButtonPress);
        loadoutMenu.gameObject.SetActive(true);
        loadoutMenu.SetupLoadoutMenu(ItemDetailsMenu.ItemDetailMenuContextType.JobSelect);
    }

    public void HandleJobSquareButtonPress(JobSelectSquare pressedSquare)
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.Selecting);
        foreach (JobSelectSquare square in jobSelectSquares)
        {
            if (pressedSquare != square)
            {
                square.SetInactive();
            }
        }
        jobName.text = pressedSquare.GetJob().GetJobName();
        string rewardText = TurnJobTypeIntoDisplayText(pressedSquare.GetJob().GetRewardItemType());
        rewardText = rewardText + ", " + pressedSquare.GetJob().GetRewardMoney() + " Credits";
        rewardField.text = rewardText;
        jobDescriptionField.text = pressedSquare.GetJob().GetJobDescription();
    }

    private string TurnJobTypeIntoDisplayText(Item.ItemTypes rewardType)
    {
        switch (rewardType)
        {
            case Item.ItemTypes.Arm:
                return "Arm Mod";
            case Item.ItemTypes.Chipset:
                return "Chipset";
            case Item.ItemTypes.Exoskeleton:
                return "Exoskeleton Mod";
            case Item.ItemTypes.Head:
                return "Head Mod";
            case Item.ItemTypes.Leg:
                return "Leg Mod";
            case Item.ItemTypes.NeuralImplant:
                return "Neural Implant";
            case Item.ItemTypes.Rig:
                return "Rig";
            case Item.ItemTypes.Software:
                return "Software";
            case Item.ItemTypes.Torso:
                return "Torso Mod";
            case Item.ItemTypes.Uplink:
                return "Uplink";
            case Item.ItemTypes.Weapon:
                return "Weapon";
            case Item.ItemTypes.Wetware:
                return "Wetware";
            default:
                return "";
        }
    }

    public void CloseJobSelectMenu()
    {
        hubWorldSFX.PlayHubSoundEffect(HubWorldSFX.HubSoundeffect.ButtonPress);
        gameObject.SetActive(false);
    }
}
