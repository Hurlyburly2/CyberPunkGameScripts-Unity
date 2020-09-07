﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutSlotBtn : MonoBehaviour
{
    [SerializeField] Button activeButton;
    [SerializeField] Button inactiveButton;

    [SerializeField] LoadoutEquipmentMenu parentMenu;
    [SerializeField] Loadout.LeftOrRight leftOrRight;

    [SerializeField] Item.ItemTypes itemType;

    bool active;

    public void SetupButton()
    {
        active = false;
        inactiveButton.gameObject.SetActive(true);
        activeButton.gameObject.SetActive(false);
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
        if (active)
        {
            SetInactive();
        } else
        {
            SetActive();
            parentMenu.HandlePressedSlotButton(itemType, leftOrRight);
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
}