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
    // Dodge

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

    public void ModifyStatus(int newStacks)
    {
        stacks += newStacks;
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
            default:
                return false;
        }
    }

    public bool IsDebuff()
    {
        switch (statusType)
        {
            default:
                return false;
        }
    }
}
