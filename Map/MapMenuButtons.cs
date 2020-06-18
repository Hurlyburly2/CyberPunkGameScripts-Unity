using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMenuButtons : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;

    public void BackToTitleButtonClick()
    {
        Debug.Log("Back to Title Click");
    }

    public void AbandonRunButtonClick()
    {
        Debug.Log("Attempt to Abandon Run");
    }

    public void OpenSettingsWindow()
    {
        Debug.Log("Open Settings Window");
    }

    public void OpenLoadoutWindow()
    {
        Debug.Log("Open Loadout Window");
    }

    public void OpenStatusWindow()
    {
        Debug.Log("Open Status Window");
    }

    public void CloseMenu()
    {
        FindObjectOfType<MapConfig>().SetIsAMenuOpen(false);
        mainMenu.SetActive(false);
    }

    public void OpenMenu()
    {
        FindObjectOfType<MapConfig>().SetIsAMenuOpen(true);
        mainMenu.SetActive(true);
    }
}
