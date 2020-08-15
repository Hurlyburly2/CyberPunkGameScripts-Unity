using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadoutMenu : MonoBehaviour
{
    [SerializeField] Image runnerPortraitImage;
    [SerializeField] TextMeshProUGUI runnerName;
    [SerializeField] Image hackerPortraitImage;
    [SerializeField] TextMeshProUGUI hackerName;

    PlayerData playerData;

    public void SetupLoadoutMenu()
    {
        RefreshPlayerData();
    }

    private void RefreshPlayerData()
    {
        PlayerData playerData = FindObjectOfType<PlayerData>();
    }

    public void CloseLoadoutMenu()
    {
        gameObject.SetActive(false);
    }
}
