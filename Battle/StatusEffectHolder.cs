using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectHolder : MonoBehaviour
{
    [SerializeField] string playerOrEnemy;
    [SerializeField] StatusEffect[] statusEffects;
    [SerializeField] Sprite[] images;

    public void InflictStatus(StatusEffect.StatusType statusType, int stacks, string inflictedBy, int duration = 0)
    {
        int indexOfStatus = GetStatusIndex(statusType);
        if (indexOfStatus == -1)
        {
            NewStatus(statusType, stacks, duration, inflictedBy);
        } else
        {
            StatusEffect currentStatus = statusEffects[indexOfStatus];
            currentStatus.ModifyStatus(stacks, duration);

            CheckDestroyStatus(currentStatus);
            RejiggerStatusIcons();
        }
    }

    public void PowerupStatus(string typeToBuff, int addToStacks, int addToDuration)
    {
        foreach (StatusEffect statusEffect in statusEffects)
        {
            switch(typeToBuff)
            {
                case "buffs":
                    if (statusEffect.IsBuff())
                    {
                        statusEffect.ModifyStatus(addToStacks, addToDuration);
                    }
                    break;
            }
        }
    }

    public int GetMomentumStacks()
    {
        int momentumIndex = GetStatusIndex(StatusEffect.StatusType.Momentum);
        if (momentumIndex == -1)
        {
            return 0;
        } else
        {
            return statusEffects[momentumIndex].GetStacks();
        }
    }

    public int GetWeaknessStacks()
    {
        int weaknessIndex = GetStatusIndex(StatusEffect.StatusType.Weakness);
        if (weaknessIndex == -1)
            return 0;
        else
            return statusEffects[weaknessIndex].GetStacks();
    }

    public int GetCritUpStacks()
    {
        int critUpIndex = GetStatusIndex(StatusEffect.StatusType.AutoCrit);
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
        int vulnerableIndex = GetStatusIndex(StatusEffect.StatusType.Vulnerable);
        if (vulnerableIndex == -1)
        {
            return 0;
        }
        else
        {
            return statusEffects[vulnerableIndex].GetStacks();
        }
    }

    public int GetDamageResistStacks()
    {
        int damageResistIndex = GetStatusIndex(StatusEffect.StatusType.DamageResist);
        if (damageResistIndex == -1)
        {
            return 0;
        } else
        {
            return statusEffects[damageResistIndex].GetStacks();
        }
    }

    public int GetCritChanceStacks()
    {
        int critChanceIndex = GetStatusIndex(StatusEffect.StatusType.CritChance);
        if (critChanceIndex == -1)
        {
            return 0;
        } else
        {
            return statusEffects[critChanceIndex].GetStacks();
        }
    }

    public int GetDodgeChance()
    {
        int dodgeIndex = GetStatusIndex(StatusEffect.StatusType.Dodge);
        if (dodgeIndex == -1)
        {
            return 0;
        } else
        {
            int dodgeChanceStacks = statusEffects[dodgeIndex].GetStacks();
            float modifier = 1;
            int dodgeChance = 0;

            for (int i = 0; i < dodgeChanceStacks; i++)
            {
                dodgeChance += Mathf.CeilToInt(30 * modifier);
                modifier /= 2;
            }
            return dodgeChance;
        }
    }

    public int GetFizzleChance()
    {
        int fizzleIndex = GetStatusIndex(StatusEffect.StatusType.FizzleChance);
        if (fizzleIndex == -1)
        {
            return 0;
        } else
        {
            return statusEffects[fizzleIndex].GetStacks();
        }
    }

    public int GetRetaliateStacks()
    {
        int retaliateIndex = GetStatusIndex(StatusEffect.StatusType.Retaliate);
        if (retaliateIndex == -1)
        {
            return 0;
        } else
        {
            return statusEffects[retaliateIndex].GetStacks();
        }
    }

    public void TickDownStatusEffects(string whoseEffectsToTick)
    {
        foreach(StatusEffect statusEffect in statusEffects)
        {
            if (statusEffect.GetPlayerOrEnemy() == whoseEffectsToTick)
            {
                statusEffect.TickDownDuration();
            }
            CheckDestroyStatus(statusEffect);
        }

        RejiggerStatusIcons();
    }

    public void HealDebuffs(int amountOfDebuffsToHeal)
    {
        List<StatusEffect> debuffs = FindDebuffs();
        if (debuffs.Count < 1)
            return;

        int randomIndex = Mathf.FloorToInt(Random.Range(0, debuffs.Count));

        StatusEffect statusToHeal = debuffs[randomIndex];
        DestroyStatus(statusToHeal);
        RejiggerStatusIcons();
    }

    public bool PurgeBuff()
    {
        // Return true if purge successful, false if there were no debuffs
        List<StatusEffect> buffs = FindBuffs();
        if (buffs.Count < 1)
            return false;

        int randomIndex = Mathf.FloorToInt(Random.Range(0, buffs.Count));

        StatusEffect buffToPurge = buffs[randomIndex];
        DestroyStatus(buffToPurge);
        RejiggerStatusIcons();
        return true;
    }

    private List<StatusEffect> FindDebuffs()
    {
        List<StatusEffect> debuffs = new List<StatusEffect>();
        foreach(StatusEffect statusEffect in statusEffects)
        {
            if (statusEffect.IsDebuff())
                debuffs.Add(statusEffect);
        }

        return debuffs;
    }

    private List<StatusEffect> FindBuffs()
    {
        List<StatusEffect> buffs = new List<StatusEffect>();
        foreach (StatusEffect statusEffect in statusEffects)
        {
            if (statusEffect.IsBuff())
                buffs.Add(statusEffect);
        }

        return buffs;
    }

    private void CheckDestroyStatus(StatusEffect statusEffect)
    {
        if (statusEffect.GetRemainingDuration() < 1 || statusEffect.GetStacks() < 1)
        {
            DestroyStatus(statusEffect);
        }
    }

    private void DestroyStatus(StatusEffect statusToDestroy)
    {
        statusToDestroy.DestroyStatus(GetStatusIcon(StatusEffect.StatusType.Default));
    }

    private void NewStatus(StatusEffect.StatusType statusType, int stacks, int duration, string inflictedBy)
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
        statusEffects[firstAvailableStatusSlot].SetupStatus(statusType, stacks, statusDuration, statusIcon, inflictedBy);
    }

    private Sprite GetStatusIcon(StatusEffect.StatusType statusType)
    {
        switch(statusType)
        {
            case StatusEffect.StatusType.Dodge:
                return images[1];
            case StatusEffect.StatusType.Momentum:
                return images[2];
            case StatusEffect.StatusType.DamageResist:
                return images[3];
            case StatusEffect.StatusType.AutoCrit:
                return images[4];
            case StatusEffect.StatusType.Vulnerable:
                return images[5];
            case StatusEffect.StatusType.FizzleChance:
                return images[6];
            case StatusEffect.StatusType.CritChance:
                return images[7];
            case StatusEffect.StatusType.Retaliate:
                return images[8];
            case StatusEffect.StatusType.Weakness:
                return images[9];
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
            if (statusEffect.GetStatusType() == StatusEffect.StatusType.None)
            {
                return currentIndex;
            }
            currentIndex++;
        }
        return -1;
    }

    public int GetDefaultStatusDuration(StatusEffect.StatusType statusType)
    {
        switch(statusType)
        {
            case StatusEffect.StatusType.Dodge:
                return 1;
            case StatusEffect.StatusType.Momentum:
                return 1;
            case StatusEffect.StatusType.DamageResist:
                return 1;
            case StatusEffect.StatusType.AutoCrit:
                return 100;
            case StatusEffect.StatusType.Vulnerable:
                return 1;
            case StatusEffect.StatusType.FizzleChance:
                return 1;
            case StatusEffect.StatusType.CritChance:
                return 1;
            default:
                return 1;
        }
    }

    private int GetStatusIndex(StatusEffect.StatusType statusToCheckFor)
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

    private void RejiggerStatusIcons()
    {
        while (!IsCorrectOrder())
        {
            StatusEffect previousEffect;
            StatusEffect currentEffect = statusEffects[statusEffects.Length - 1];
            for (int i = statusEffects.Length - 1; i >= 0; i--)
            {
                if (i == statusEffects.Length - 1)
                {
                    // On the first loop, we just store the current effect
                    currentEffect = statusEffects[i];
                }
                else
                {
                    previousEffect = statusEffects[i + 1];
                    currentEffect = statusEffects[i];
                    CopyEffect(previousEffect, currentEffect);
                }
            }
        } 
    }

    private void CopyEffect(StatusEffect previousEffect, StatusEffect currentEffect)
    {
        if (currentEffect.GetStatusType() == StatusEffect.StatusType.None)
        {
            int remainingDuration = previousEffect.GetRemainingDuration();
            int stacks = previousEffect.GetStacks();
            StatusEffect.StatusType statusType = previousEffect.GetStatusType();
            string inflictedBy = previousEffect.GetPlayerOrEnemy();
            Sprite statusIcon = GetStatusIcon(statusType);

            previousEffect.DestroyStatus(GetStatusIcon(StatusEffect.StatusType.Default));
            currentEffect.SetupStatus(statusType, stacks, remainingDuration, statusIcon, inflictedBy);
        }
    }

    private bool IsCorrectOrder()
    {
        bool foundEmpty = false;
        foreach(StatusEffect statusEffect in statusEffects)
        {
            if (statusEffect.GetStatusType() != StatusEffect.StatusType.None && foundEmpty == true)
            {
                return false;
            } else if (statusEffect.GetStatusType() == StatusEffect.StatusType.None)
            {
                foundEmpty = true;
            }
        }
        return true;
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
