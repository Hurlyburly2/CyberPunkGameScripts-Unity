using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityHelpMenuAbilityHolder : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI abilityText;
    [SerializeField] Image abilityIcon;
    [SerializeField] TextMeshProUGUI usesText;

    HackerMod mod;
    AbilityButton abilityButton;

    public void SetupAbilityHolder(HackerMod hackerMod, AbilityButton newAbilityButton)
    {
        abilityButton = newAbilityButton;
        mod = hackerMod;
        abilityText.text = mod.GetItemAbilityDescription();
        if (abilityButton.GetRemainingUses() == 1)
            usesText.text = "1 use";
        else
            usesText.text = abilityButton.GetRemainingUses().ToString() + " uses";
        abilityIcon.sprite = FindObjectOfType<AllHackAbilityIcons>().GetAbilityIconById(mod.GetActiveAbilityId());
    }
}
