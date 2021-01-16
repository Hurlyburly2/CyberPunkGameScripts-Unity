using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSlotMenu : MonoBehaviour
{
    [SerializeField] FirstMenu firstMenu;
    [SerializeField] List<SaveSlotUI> saveSlotUIs;

    bool newGame;

    public void SetupSaveSlotMenu(bool newGameFromFirstMenu = false)
    {
        newGame = newGameFromFirstMenu;

        int slotNumber = 0;
        foreach (SaveSlotUI saveSlot in saveSlotUIs)
        {
            slotNumber++;
            saveSlot.SetupSaveSlot(slotNumber);
        }
    }

    public void CloseSaveSelectMenu()
    {
        firstMenu.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
