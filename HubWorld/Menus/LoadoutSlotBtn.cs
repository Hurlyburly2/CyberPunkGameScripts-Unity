using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutSlotBtn : MonoBehaviour
{
    [SerializeField] Button activeButton;
    [SerializeField] Button inactiveButton;

    [SerializeField] LoadoutEquipmentMenu parentMenu;
    [SerializeField] Loadout.LeftOrRight leftOrRight;
    [SerializeField] int slotNumber; // 1, 2, or 3. 0 is default for things without slots.

    [SerializeField] Item.ItemTypes itemType;

    bool active;
    bool waitingForInput;

    public void SetupButton()
    {
        active = false;
        waitingForInput = false;
        inactiveButton.gameObject.SetActive(true);
        activeButton.gameObject.SetActive(false);
    }

    public void SetButtonToAskForInput()
    {
        waitingForInput = true;
    }

    public void ClearWaitingForInput()
    {
        waitingForInput = false;
    }

    public void SetActive()
    {
        active = true;
        inactiveButton.gameObject.SetActive(false);
        activeButton.gameObject.SetActive(true);
    }

    public void SetInactive()
    {
        active = false;
        inactiveButton.gameObject.SetActive(true);
        activeButton.gameObject.SetActive(false);
    }

    public void PressButton()
    {
        if (!waitingForInput)
        {
            // Normal State
            if (active)
            {
                SetInactive();
                parentMenu.HandlePressedSlotButton(itemType, leftOrRight, slotNumber);
            }
            else
            {
                SetActive();
                parentMenu.HandlePressedSlotButton(itemType, leftOrRight, slotNumber);
            }
        } else
        {
            // Tried to equip an arm/leg/install without having selected a slot
            waitingForInput = false;
            SetActive();
            parentMenu.HandlePressedSlotButton(itemType, leftOrRight, slotNumber);
            parentMenu.EquipItem();
        }
    }

    public Item.ItemTypes GetItemType()
    {
        return itemType;
    }

    public Loadout.LeftOrRight GetLeftOrRight()
    {
        return leftOrRight;
    }

    public bool GetIsActive()
    {
        return active;
    }

    public int GetSlotNumber()
    {
        return slotNumber;
    }
}
