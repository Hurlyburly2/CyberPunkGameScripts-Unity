using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutEquipmentMenu : MonoBehaviour
{
    [SerializeField] GameObject runnerContext;
    [SerializeField] GameObject hackerContext;

    Item.HackerRunner hackerOrRunner;

    CharacterData runner;
    HackerData hacker;

    private void DoSetup()
    {
        switch (hackerOrRunner)
        {
            case Item.HackerRunner.Runner:
                hackerContext.SetActive(false);
                runnerContext.SetActive(true);
                break;
            case Item.HackerRunner.Hacker:
                runnerContext.SetActive(false);
                hackerContext.SetActive(true);
                break;
        }
    }

    public void SetupLoadoutEquipmentMenu(CharacterData newRunner)
    {
        runner = newRunner;
        hackerOrRunner = Item.HackerRunner.Runner;
        DoSetup();
    }

    public void SetupLoadoutEquipmentMenu(HackerData newHacker)
    {
        hacker = newHacker;
        hackerOrRunner = Item.HackerRunner.Hacker;
        DoSetup();
    }

    public void CloseLoadoutEquipmentMenu()
    {
        gameObject.SetActive(false);
    }
}
