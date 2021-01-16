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

    public void SetupSaveSlot(int slotNumber)
    {
        slotNumberText.text = slotNumber.ToString();

        string chapterName = SavePrefs.GetSaveSlotArea(slotNumber);
        if (chapterName == "")
            emptyText.gameObject.SetActive(true);
        else
            emptyText.gameObject.SetActive(false);
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
