using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeMenuActiveAbilityHolder : MonoBehaviour
{
    [SerializeField] Sprite activeAbilityHolderOn;
    [SerializeField] Sprite activeAbilityHolderOff;

    [SerializeField] Image activeAbilityHolder;
    [SerializeField] Image activeAbilityIcon;
    [SerializeField] TextMeshProUGUI activeAbilityUses;
    [SerializeField] TextMeshProUGUI activeAbilityDescription;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] Image currentLevelMarker;
    [SerializeField] Image overlay;

    public void SetupAbilityHolder(Sprite abilityImage, int itemCurrentLevel, int counter, bool isCurrentLevel, string abilityDescription, int abilityUses)
    {
        if (isCurrentLevel)
            currentLevelMarker.gameObject.SetActive(true);
        else
            currentLevelMarker.gameObject.SetActive(false);

        activeAbilityIcon.sprite = abilityImage;
        if (abilityUses < 2)
            activeAbilityUses.text = abilityUses.ToString() + " use";
        else
            activeAbilityUses.text = abilityUses.ToString() + " uses";

        activeAbilityDescription.text = abilityDescription;
        SetupOverlay(itemCurrentLevel, counter);
    }

    private void SetupOverlay(int itemCurrentLevel, int counter)
    {
        if (itemCurrentLevel <= counter)
            overlay.gameObject.SetActive(false);
        else
            overlay.gameObject.SetActive(true);
    }
}
