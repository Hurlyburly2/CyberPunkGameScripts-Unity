using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class StatusEffect : MonoBehaviour
{
    TextMeshProUGUI numberOfStacksTextField;

    public enum StatusType { None, Default, Dodge, Momentum, DamageResist, AutoCrit, Vulnerable };
    // PREVIOUSLY: DAMAGE RESIST, CRITUP
    int remainingDuration;
    int stacks;
    StatusType statusType;
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
        statusType = StatusType.None;
        numberOfStacksTextField = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnMouseDown()
    {
        DisplayHelperText();
    }

    private void OnMouseUp()
    {
        HideHelperText();
    }

    public void DisplayHelperText()
    {
        if (statusType != StatusType.None)
        {
            FindObjectOfType<PopupHolder>().SpawnStatusPopup(GetMessageText());
        }
    }

    private string GetMessageText()
    {
        switch (statusType)
        {
            case StatusType.Dodge:
                int dodgeChance = GetComponentInParent<StatusEffectHolder>().GetDodgeChance();
                return dodgeChance + "% Chance to Dodge Attacks";
            case StatusType.Momentum:
                return "Deal +" + stacks + " damage";
            case StatusType.DamageResist:
                return "Take -" + stacks + " damage";
            case StatusType.AutoCrit:
                return "Your next " + stacks + " attacks will be critical hits";
            case StatusType.Vulnerable:
                return "Take +" + stacks + " damage";
            default:
                return "THIS AIN'T IT, CHIEF";
        }
    }

    public void HideHelperText()
    {
        if (statusType != StatusType.None)
        {
            FindObjectOfType<PopupHolder>().DestroyAllPopups();
        }
    }

    public void TickDownDuration()
    {
        remainingDuration--;
        UpdateStackText();
    }

    public void SetupStatus(StatusType newStatusType, int incomingStacks, int duration, Sprite statusIcon, string newInflictedBy)
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
        statusType = StatusType.None;
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


    public StatusType GetStatusType()
    {
        return statusType;
    }

    public bool IsBuff()
    {
        switch (statusType)
        {
            case StatusType.Dodge:
                return true;
            case StatusType.Momentum:
                return true;
            case StatusType.DamageResist:
                return true;
            case StatusType.AutoCrit:
                return true;
            default:
                return false;
        }
    }

    public bool IsDebuff()
    {
        switch (statusType)
        {
            case StatusType.Vulnerable:
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
