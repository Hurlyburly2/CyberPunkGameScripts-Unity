using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JobSelectMenu : MonoBehaviour
{
    [SerializeField] Image runnerPortrait;
    [SerializeField] Image hackerPortrait;
    [SerializeField] LoadoutMenu loadoutMenu;

    public void SetupMenu()
    {
        Debug.Log("setup job select menu...");
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

    public void CloseJobSelectMenu()
    {
        gameObject.SetActive(false);
    }
}
