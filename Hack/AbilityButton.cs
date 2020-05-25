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
                    // Add a R connection to the active card
                    AddConnectionsToActiveCard(1, "red");
                    break;
                case 1:
                    //1: For your next action, pick from your top two cards.Discard the other
                    SelectFromTopOfDeck(2, 1);
                    break;
                case 2:
                    // 2: add the top card of your discard to the top of your deck
                    MoveCardsFromDiscardToDeck(1);
                    break;
            }
            currentAbilityUses--;
            SetCurrentUseIcons();
        }
    }

    private void AddConnectionsToActiveCard(int number, string color)
    {
        HackDeck hackDeck = FindObjectOfType<HackDeck>();
        if (hackDeck.GetCardCount() > 0)
        {
            hackDeck.AddConnectionsToActiveCard(number, color);
        }
    }

    private void AddSpikesToActiveCard()
    {
        Debug.Log("Add spikes to active card");
    }

    private void MoveCardsFromDiscardToDeck(int numberOfCards)
    {
        FindObjectOfType<HackDiscard>().SendCardsFromDiscardToTopOfDeck(numberOfCards);
    }

    private void SelectFromTopOfDeck(int pickFromHowMany, int pickHowMany)
    {
        List<HackCard> cardsToPickFrom = FindObjectOfType<HackDeck>().GetTopXHackCards(pickFromHowMany);

        if (cardsToPickFrom.Count > 0)
        {
            FindObjectOfType<CheckClickController>().SetTilePickerState();
            HackTilePicker hackTilePicker = FindObjectOfType<HackHolder>().GetHackTilePicker();
            hackTilePicker.gameObject.SetActive(true);
            hackTilePicker.Initialize(cardsToPickFrom, pickHowMany, "pickAndDiscard");
        }
    }
}
