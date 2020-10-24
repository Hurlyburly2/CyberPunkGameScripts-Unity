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

        // FILL IN THE REST HERE!!!
    }

    public void CloseMenu()
    {
        FindObjectOfType<CheckClickController>().SetNormalState();
        gameObject.SetActive(false);
    }
}
