using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionStartMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textField;

    public void SetupMissionStartMenu()
    {
        gameObject.SetActive(true);
        textField.text = FindObjectOfType<MapData>().GetJob().GetJobIntroText();
        FindObjectOfType<MapConfig>().SetIsAMenuOpen(true);
    }

    public void CloseMissionStartWindow()
    {
        gameObject.SetActive(false);
        FindObjectOfType<MapConfig>().SetIsAMenuOpen(false);
    }
}
