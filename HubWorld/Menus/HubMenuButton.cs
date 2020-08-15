using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HubMenuButton : MonoBehaviour
{
    [SerializeField] MainMenu mainMenu;

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

    public void OpenMainMenu()
    {
        mainMenu.gameObject.SetActive(true);
    }
}
