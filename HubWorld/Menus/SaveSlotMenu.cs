using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSlotMenu : MonoBehaviour
{
    [SerializeField] FirstMenu firstMenu;

    public void SetupSaveSlotMenu(bool newGame = false)
    {
        Debug.Log("Doesn't do anything yet...");
    }

    public void CloseSaveSelectMenu()
    {
        firstMenu.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
