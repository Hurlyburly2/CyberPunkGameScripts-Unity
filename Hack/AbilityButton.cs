using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    [SerializeField] string whichAbility;
        // possibilities: rig, neuralImplant, uplink
    [SerializeField] Image abilityIcon;

    [SerializeField] Image abilityUsePip1;
    [SerializeField] Image abilityUsePip2;
    [SerializeField] Image abilityUsePip3;

    Color disabledColor;
    Color normalColor;

    int abilityId;
    int maxAbilityUses;
    int currentAbilityUses;

    public void SetupAbility(int newAbilityId, int newAbilityMaxUses)
    {
        abilityId = newAbilityId;
        maxAbilityUses = newAbilityMaxUses;
        currentAbilityUses = maxAbilityUses;

        Button button = GetComponent<Button>();
        disabledColor = button.colors.disabledColor;
        normalColor = button.colors.normalColor;

        SetAbilityIcon();
        SetCurrentUseIcons();
    }

    private void SetAbilityIcon()
    {
        abilityIcon.sprite = FindObjectOfType<AllHackAbilityIcons>().GetAbilityIconById(abilityId);
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
            switch (abilityId)
            {
                case 0:
                    // Add a R connection and two R spikes to the active card
                    AddConnectionsToActiveCard(1, "red");
                    AddSpikesToActiveCard();
                    Debug.Log("Total uses: " + maxAbilityUses);
                    break;
                case 1:
                    //1: For your next action, pick from your top two cards.Discard the other
                    SelectFromTopOfDeck();
                    Debug.Log("Total uses: " + maxAbilityUses);
                    break;
                case 2:
                    // 2: you may play the top card of your discard as if it was your active card
                    PlayCardsFromTopOfDiscard();
                    Debug.Log("Total uses: " + maxAbilityUses);
                    break;
            }
            currentAbilityUses--;
            SetCurrentUseIcons();
        }
    }

    private void AddConnectionsToActiveCard(int number, string color)
    {
        Debug.Log("Add Red connection to active card");
        HackDeck hackDeck = FindObjectOfType<HackDeck>();
        if (hackDeck.GetCardCount() > 0)
        {
            hackDeck.AddConnectionsToActiveCard(number, color);
        }
    }

    private void AddSpikesToActiveCard()
    {
        Debug.Log("Add Red spikes to active card");
    }

    private void PlayCardsFromTopOfDiscard()
    {
        Debug.Log("play cards from top of discard");
    }

    private void SelectFromTopOfDeck()
    {
        Debug.Log("Select cards from top of deck");
    }
}
