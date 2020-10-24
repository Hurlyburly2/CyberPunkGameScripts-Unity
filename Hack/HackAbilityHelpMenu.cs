using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackAbilityHelpMenu : MonoBehaviour
{
    [SerializeField] AbilityHelpMenuAbilityHolder rigAbilityHolder;
    [SerializeField] AbilityHelpMenuAbilityHolder neuralImplantAbilityHolder;
    [SerializeField] AbilityHelpMenuAbilityHolder uplinkAbilityHolder;

    public void OpenMenu()
    {
        gameObject.SetActive(true);
        FindObjectOfType<CheckClickController>().SetOverlayState();
        AbilityButton[] abilityButtons = FindObjectsOfType<AbilityButton>();
        foreach (AbilityButton abilityButton in abilityButtons)
        {
            HackerMod hackerMod = abilityButton.GetHackerMod();
            switch (hackerMod.GetItemType())
            {
                case Item.ItemTypes.NeuralImplant:
                    neuralImplantAbilityHolder.SetupAbilityHolder(hackerMod, abilityButton);
                    break;
                case Item.ItemTypes.Rig:
                    rigAbilityHolder.SetupAbilityHolder(hackerMod, abilityButton);
                    break;
                case Item.ItemTypes.Uplink:
                    uplinkAbilityHolder.SetupAbilityHolder(hackerMod, abilityButton);
                    break;
            }
        }
    }

    public void CloseMenu()
    {
        FindObjectOfType<CheckClickController>().SetNormalState();
        gameObject.SetActive(false);
    }
}
