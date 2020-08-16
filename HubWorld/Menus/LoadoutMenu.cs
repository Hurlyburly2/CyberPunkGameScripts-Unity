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
    CharacterData currentRunner;
    HackerData currentHacker;

    public void SetupLoadoutMenu()
    {
        RefreshPlayerData();
    }

    private void RefreshPlayerData()
    {
        PlayerData playerData = FindObjectOfType<PlayerData>();
        currentRunner = playerData.GetCurrentRunner();
        currentHacker = playerData.GetCurrentHacker();

        string runnerImagePath = "Characters/Runner" + currentRunner.getId() + "-Full";
        runnerPortraitImage.sprite = Resources.Load<Sprite>(runnerImagePath);
        runnerName.text = currentRunner.GetRunnerName();

        string hackerImagePath = "Characters/Hacker" + currentHacker.GetId() + "-Full";
        hackerPortraitImage.sprite = Resources.Load<Sprite>(hackerImagePath);
        hackerName.text = currentHacker.GetName();
    }

    public void CloseLoadoutMenu()
    {
        gameObject.SetActive(false);
    }
}
