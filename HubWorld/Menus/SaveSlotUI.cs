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
    [SerializeField] Button mainButton;
    [SerializeField] Button deleteButton;

    [SerializeField] GameObject content;
    [SerializeField] TextMeshProUGUI chapterNameText;
    [SerializeField] TextMeshProUGUI creditsText;
    [SerializeField] Image runnerPortrait;
    [SerializeField] Image hackerPortrait;

    int slotNumber;
    bool isNewGame;
    SaveSlotMenu parentMenu;

    public void SetupSaveSlot(int newSlotNumber, bool newIsNewGame, SaveSlotMenu newParentMenu)
    {
        parentMenu = newParentMenu;
        slotNumber = newSlotNumber;
        isNewGame = newIsNewGame;
        slotNumberText.text = slotNumber.ToString();

        string chapterName = SavePrefs.GetSaveSlotArea(slotNumber);
        if (isNewGame)
        {
            if (chapterName == "")
            {
                DeactivateContent();
                mainButton.interactable = true;
                deleteButton.gameObject.SetActive(false);
            }
            else
            {
                ActivateContent(chapterName);
                mainButton.interactable = true;
                deleteButton.gameObject.SetActive(true);
            }
        } else
        {
            if (chapterName == "")
            {
                DeactivateContent();
                mainButton.interactable = false;
                deleteButton.gameObject.SetActive(false);
            } else
            { 
                ActivateContent(chapterName);
                mainButton.interactable = true;
                deleteButton.gameObject.SetActive(true);
            }
        }
    }

    private void ActivateContent(string chapterName)
    {
        emptyText.gameObject.SetActive(false);
        content.SetActive(true);
        chapterNameText.text = chapterName;
        creditsText.text = SavePrefs.GetCreditsAmount(slotNumber);
        runnerPortrait.sprite = Resources.Load<Sprite>("Characters/Runner" + SavePrefs.GetCurrentRunnerId(slotNumber).ToString() + "-Portrait");
        hackerPortrait.sprite = Resources.Load<Sprite>("Characters/Hacker" + SavePrefs.GetCurrentRunnerId(slotNumber).ToString() + "-Portrait");
    }

    private void DeactivateContent()
    {
        emptyText.gameObject.SetActive(true);
        content.SetActive(false);
    }

    public void ClickSaveSlot()
    {
        if (isNewGame)
        {
            PlayerData playerData = FindObjectOfType<PlayerData>();
            playerData.SetupNewGame(slotNumber);
            playerData.SavePlayer();

            FindObjectOfType<HubMenuButton>().OpenMenu();
            parentMenu.gameObject.SetActive(false);
        } else
        {
            PlayerData playerData = FindObjectOfType<PlayerData>();
            playerData.LoadPlayer(slotNumber);
            FindObjectOfType<HubMenuButton>().OpenMenu();
            parentMenu.gameObject.SetActive(false);
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

    public void DeleteSave()
    {
        NotificationMenu notificationMenu = parentMenu.GetNotificationMenu();
        notificationMenu.gameObject.SetActive(true);
        notificationMenu.SetupNotification(NotificationMenu.HubNotificationType.DeletGameConfirm, slotNumber);
    }
}
