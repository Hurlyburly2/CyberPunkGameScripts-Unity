using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectHolder : MonoBehaviour
{
    [SerializeField] string playerOrEnemy;
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
            StatusEffect currentStatus = statusEffects[indexOfStatus];
            currentStatus.ModifyStatus(stacks, duration);
            CheckDestroyStatus(currentStatus);
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

    public int GetCritUpStacks()
    {
        int critUpIndex = GetStatusIndex("CritUp");
        if (critUpIndex == -1)
        {
            return 0;
        } else
        {
            return 1;
        }
    }

    public int GetVulnerableStacks()
    {
        int vulnerableIndex = GetStatusIndex("Vulnerable");
        if (vulnerableIndex == -1)
        {
            return 0;
        }
        else
        {
            return statusEffects[vulnerableIndex].GetStacks();
        }
    }

    private void CheckDestroyStatus(StatusEffect currentStatus)
    {
        // Check if the status has zero stacks
        // If yes, DestroyStatus()
        // If no, no effect
    }

    private void DestroyStatus(StatusEffect currentStatus)
    {
        // currentStatus.destroyStatus();
        // Rejigger the rest so there are no empty spaces
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
            case "CritUp":
                return images[4];
            case "Vulnerable":
                return images[5];
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
            case "CritUp":
                return 1;
            case "Vulnerable":
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

    public string IsPlayerOrEnemy()
    {
        return playerOrEnemy;
    }
}
