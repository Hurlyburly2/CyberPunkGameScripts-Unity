using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstMenu : MonoBehaviour
{
    [SerializeField] SaveSlotMenu saveSlotMenu;

    public void ContinueButtonPress()
    {

    }

    public void NewGameBtnPress()
    {
        // TODO: REMOVE THIS
        FindObjectOfType<PlayerData>().SetupNewGame();
        saveSlotMenu.gameObject.SetActive(true);
        saveSlotMenu.SetupSaveSlotMenu(true);
        gameObject.SetActive(false);
    }

    public void LoadGameBtnPress()
    {
        saveSlotMenu.gameObject.SetActive(true);
        saveSlotMenu.SetupSaveSlotMenu();
        gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void CloseFirstMenu()
    {
        gameObject.SetActive(false);
    }
}
