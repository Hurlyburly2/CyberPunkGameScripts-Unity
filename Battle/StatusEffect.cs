using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusEffect : MonoBehaviour
{
    TextMeshProUGUI numberOfStacksTextField;

    int remainingDuration;
    int stacks;
    string statusType = "";
    // Options:
        // Dodge: +X% chance dodge for each stack
        // Momentum: +1 damage per stack
        // Damage: Resist -1 damage taken per stack
        // CritUp: Next hit automatically crits, then stacks -1
        // Vulnerable: +1 damage taken per stack
    string inflictedBy;
    // Player or Enemy -
        // Tracks when duration ticks down
            // Player ticks down at beginning of player turn, enemy at enemy turn

    private void Start()
    {
        statusType = "";
        numberOfStacksTextField = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void TickDownDuration()
    {
        remainingDuration--;
        UpdateStackText();
    }

    public void SetupStatus(string newStatusType, int incomingStacks, int duration, Sprite statusIcon, string newInflictedBy)
    {
        statusType = newStatusType;
        stacks = incomingStacks;
        remainingDuration = duration;
        inflictedBy = newInflictedBy;

        GetComponentInChildren<Image>().sprite = statusIcon;
        if (stacks > 0)
        {
            UpdateStackText();
        }
    }

    public void ModifyStatus(int newStacks, int duration = 0)
    {
        stacks += newStacks;
        remainingDuration += duration;
        UpdateStackText();
    }

    public void DestroyStatus(Sprite defaultImage)
    {
        // RESET IT ALL
        remainingDuration = 0;
        stacks = 0;
        statusType = "";
        inflictedBy = "";
        GetComponentInChildren<Image>().sprite = defaultImage;
        ClearStackText();
    }

    public int GetStacks()
    {
        return stacks;
    }

    private void UpdateStackText()
    {
        numberOfStacksTextField.text = stacks.ToString();
    }

    private void ClearStackText()
    {
        numberOfStacksTextField.text = "";
    }


    public string GetStatusType()
    {
        return statusType;
    }

    public bool IsBuff()
    {
        switch (statusType)
        {
            case "Dodge":
                return true;
            case "Momentum":
                return true;
            case "Damage Resist":
                return true;
            case "CritUp":
                return true;
            default:
                return false;
        }
    }

    public bool IsDebuff()
    {
        switch (statusType)
        {
            case "Vulnerable":
                return true;
            default:
                return false;
        }
    }

    public string GetPlayerOrEnemy()
    {
        return inflictedBy;
    }

    public int GetRemainingDuration()
    {
        return remainingDuration;
    }
}
