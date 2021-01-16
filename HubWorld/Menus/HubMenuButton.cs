using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HubMenuButton : MonoBehaviour
{
    [SerializeField] MainMenu mainMenu;
    [SerializeField] FirstMenu firstMenu;
    [SerializeField] MissionCompleteMenu missionCompleteMenu;

    [SerializeField] private AnimationCurve fadeCurve;
    [SerializeField] TextMeshProUGUI text1;
    [SerializeField] TextMeshProUGUI text2;
    Color color;
    float timer = 0f;

    private void Awake()
    {
        color = text1.color;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        color.a = fadeCurve.Evaluate(timer);
        text1.color = color;
        text2.color = color;
    }

    public void OpenMenu()
    {
        PlayerData playerData = FindObjectOfType<PlayerData>();

        if (playerData.GetIsPlayerLoaded())
        {
            OpenMainMenu();
        } else
        {
            OpenFirstMenu();
        }
    }

    private void OpenFirstMenu()
    {
        firstMenu.gameObject.SetActive(true);
        firstMenu.SetupFirstMenu();
    }

    private void OpenMainMenu()
    {
        mainMenu.gameObject.SetActive(true);
    }

    public MissionCompleteMenu GetMissionCompleteMenu()
    {
        return missionCompleteMenu;
    }
}
