using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    [SerializeField] string whichAbility;
        // possibilities: rig, neuralImplant, uplink
    [SerializeField] Image abilityIcon;
    HackerMod hackerMod;

    [SerializeField] Image abilityUsePip1;
    [SerializeField] Image abilityUsePip2;
    [SerializeField] Image abilityUsePip3;

    Color disabledColor;
    Color normalColor;

    int maxAbilityUses;
    int currentAbilityUses;

    public void SetupAbility(HackerMod newHackerMod)
    {
        hackerMod = newHackerMod;
        maxAbilityUses = hackerMod.GetActiveAbilityUses();
        currentAbilityUses = maxAbilityUses;

        Button button = GetComponent<Button>();
        disabledColor = button.colors.disabledColor;
        normalColor = button.colors.normalColor;

        SetAbilityIcon();
        SetCurrentUseIcons();
    }

    private void SetAbilityIcon()
    {
        abilityIcon.sprite = FindObjectOfType<AllHackAbilityIcons>().GetAbilityIconById(hackerMod.GetActiveAbilityId());
    }

    public string GetWhichAbility()
    {
        return whichAbility;
    }

    private void SetCurrentUseIcons()
    {
        if (currentAbilityUses > 0)
        {
            abilityUsePip1.gameObject.SetActive(true);
            GetComponent<Button>().interactable = true;
            abilityIcon.color = normalColor;
        } else
        {
            abilityUsePip1.gameObject.SetActive(false);
            GetComponent<Button>().interactable = false;
            abilityIcon.color = disabledColor;
        }
        if (currentAbilityUses > 1)
        {
            abilityUsePip2.gameObject.SetActive(true);
        } else
        {
            abilityUsePip2.gameObject.SetActive(false);
        }
        if (currentAbilityUses > 2)
        {
            abilityUsePip3.gameObject.SetActive(true);
        } else
        {
            abilityUsePip3.gameObject.SetActive(false);
        }
    }

    public void UseAbility()
    {
        if (currentAbilityUses > 0)
        {
            hackerMod.UseAbility();
            currentAbilityUses--;
            SetCurrentUseIcons();
        }
    }
}
