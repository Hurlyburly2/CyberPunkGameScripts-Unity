using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SaveSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image normalBackRect;
    [SerializeField] Image mouseOverBackRect;
    [SerializeField] TextMeshProUGUI slotNumberText;
    [SerializeField] TextMeshProUGUI emptyText;

    [SerializeField] GameObject content;
    [SerializeField] TextMeshProUGUI chapterNameText;
    [SerializeField] TextMeshProUGUI creditsText;

    public void SetupSaveSlot(int slotNumber)
    {
        slotNumberText.text = slotNumber.ToString();

        string chapterName = SavePrefs.GetSaveSlotArea(slotNumber);
        if (chapterName == "")
        {
            emptyText.gameObject.SetActive(true);
            content.SetActive(false);
        } else
        {
            emptyText.gameObject.SetActive(false);
            content.SetActive(true);
            chapterNameText.text = chapterName;
            creditsText.text = SavePrefs.GetCreditsAmount(slotNumber);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        normalBackRect.gameObject.SetActive(false);
        mouseOverBackRect.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOverBackRect.gameObject.SetActive(false);
        normalBackRect.gameObject.SetActive(true);
    }
}
