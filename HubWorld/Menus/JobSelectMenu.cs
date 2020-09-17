using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JobSelectMenu : MonoBehaviour
{
    [SerializeField] Image runnerPortrait;
    [SerializeField] Image hackerPortrait;
    [SerializeField] LoadoutMenu loadoutMenu;
    [SerializeField] List<JobSelectSquare> jobSelectSquares;

    public void SetupMenu()
    {
        PlayerData playerData = FindObjectOfType<PlayerData>();
        runnerPortrait.sprite = playerData.GetCurrentRunner().GetRunnerPortraitSmall();
        hackerPortrait.sprite = playerData.GetCurrentHacker().GetHackerPortraitSmall();

        SetupJobButtons(playerData.GetCurrentJobsList());
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
        // triggers when 'launch' button is pressed
        Debug.Log("Launch Mission!");
    }

    public void OpenLoadoutMenu()
    {
        loadoutMenu.gameObject.SetActive(true);
        loadoutMenu.SetupLoadoutMenu(ItemDetailsMenu.ItemDetailMenuContextType.JobSelect);
    }

    public void HandleJobSquareButtonPress(JobSelectSquare pressedSquare)
    {
        foreach (JobSelectSquare square in jobSelectSquares)
        {
            if (pressedSquare != square)
            {
                square.SetInactive();
            }
        }
    }

    public void CloseJobSelectMenu()
    {
        gameObject.SetActive(false);
    }
}
