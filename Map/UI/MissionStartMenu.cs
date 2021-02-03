using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionStartMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textField;

    MapSFX mapSFX;

    private void Awake()
    {
        mapSFX = FindObjectOfType<MapSFX>();
    }

    public void SetupMissionStartMenu()
    {
        gameObject.SetActive(true);
        textField.text = FindObjectOfType<MapData>().GetJob().GetJobIntroText();
        FindObjectOfType<MapConfig>().SetIsAMenuOpen(true);
    }

    public void CloseMissionStartWindow()
    {
        mapSFX.PlayMapSoundSFX(MapSFX.MapSoundEffect.ButtonPress);
        gameObject.SetActive(false);
        FindObjectOfType<MapConfig>().SetIsAMenuOpen(false);
    }
}
