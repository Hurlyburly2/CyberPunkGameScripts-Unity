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

    private void Start()
    {
        statusType = "";
    }

    public void SetupStatus(string newStatusType, int incomingStacks, int duration, Sprite statusIcon)
    {
        statusType = newStatusType;
        stacks = incomingStacks;
        remainingDuration = duration;

        numberOfStacksTextField = GetComponentInChildren<TextMeshProUGUI>();
        GetComponentInChildren<Image>().sprite = statusIcon;
        UpdateStackText();
    }

    public void ModifyStatus(int newStacks, int duration = 0)
    {
        stacks += newStacks;
        remainingDuration += duration;
        UpdateStackText();
    }

    public int GetStacks()
    {
        return stacks;
    }

    private void UpdateStackText()
    {
        numberOfStacksTextField.text = stacks.ToString();
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
}
