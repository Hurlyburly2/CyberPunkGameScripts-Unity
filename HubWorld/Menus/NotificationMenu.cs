using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NotificationMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI notificationMessage;
    [SerializeField] Button okButton;
    [SerializeField] Button cancelButton;
    [SerializeField] SaveSlotMenu saveSlotMenu;

    public enum HubNotificationType { NewGameOverwrite, DeletGameConfirm };
    HubNotificationType notificationType;

    int saveSlot;

    public void SetupNotification(HubNotificationType newNotificationType, int newSaveSlot)
    {
        notificationType = newNotificationType;
        saveSlot = newSaveSlot;

        switch (notificationType)
        {
            case HubNotificationType.NewGameOverwrite:
                notificationMessage.text = "Are you sure you want to overwrite the save in slot " + saveSlot.ToString() + "?";
                okButton.gameObject.SetActive(true);
                cancelButton.gameObject.SetActive(true);
                break;
            case HubNotificationType.DeletGameConfirm:
                notificationMessage.text = "Are you sure you want to delete the save in slot " + saveSlot.ToString() + "?";
                okButton.gameObject.SetActive(true);
                cancelButton.gameObject.SetActive(true);
                break;
        }
    }

    public void ClickOkButton()
    {
        saveSlotMenu.DeleteSaveBySlot(saveSlot);
        gameObject.SetActive(false);
    }

    public void ClickCancelButton()
    {
        gameObject.SetActive(false);
    }
}
