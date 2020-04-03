using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectHolder : MonoBehaviour
{
    [SerializeField] StatusEffect[] statusEffects;
    [SerializeField] Sprite[] images;

    public void InflictStatus(string statusType, int stacks, int duration = 0)
    {
        int indexOfStatus = GetStatusIndex(statusType);
        if (indexOfStatus == -1)
        {
            NewStatus(statusType, stacks, duration);
        } else
        {
            statusEffects[indexOfStatus].ModifyStatus(stacks, duration);
        }
    }

    public int GetMomentumStacks()
    {
        int momentumIndex = GetStatusIndex("Momentum");
        if (momentumIndex == -1)
        {
            return 0;
        } else
        {
            return statusEffects[momentumIndex].GetStacks();
        }
    }

    private void NewStatus(string statusType, int stacks, int duration)
    {
        int statusDuration = 0;
        if (duration == 0)
        {
             statusDuration = GetDefaultStatusDuration(statusType);
        } else
        {
            statusDuration = duration;
        }
        int firstAvailableStatusSlot = FindFirstAvailableStatusSlot();
        Sprite statusIcon = GetStatusIcon(statusType);
        statusEffects[firstAvailableStatusSlot].SetupStatus(statusType, stacks, statusDuration, statusIcon);
    }

    private Sprite GetStatusIcon(string statusType)
    {
        switch(statusType)
        {
            case "Dodge":
                return images[1];
            case "Momentum":
                return images[2];
            case "Damage Resist":
                return images[3];
            default:
                // default empty status
                return images[0];
        }
    }

    private int FindFirstAvailableStatusSlot()
    {
        int currentIndex = 0;
        foreach(StatusEffect statusEffect in statusEffects)
        {
            if (statusEffect.GetStatusType() == "")
            {
                return currentIndex;
            }
            currentIndex++;
        }
        return -1;
    }

    private int GetDefaultStatusDuration(string statusType)
    {
        switch(statusType)
        {
            case "Dodge":
                return 1;
            case "Momentum":
                return 1;
            case "Damage Resist":
                return 1;
            default:
                return 1;
        }
    }

    private int GetStatusIndex(string statusToCheckFor)
    {
        // look for and return index of status effect in holder
        // if status effect does not exist, return -1
        int currentIndex = 0;
        foreach(StatusEffect statusEffect in statusEffects)
        {
            if (statusEffect.GetStatusType() == statusToCheckFor)
            {
                return currentIndex;
            }
            currentIndex++;
        }
        return -1;
    }

    public StatusEffect[] GetAllStatusEffects()
    {
        return statusEffects;
    }
}
