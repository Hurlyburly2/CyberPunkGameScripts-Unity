using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutEquipmentMenu : MonoBehaviour
{
    Item.HackerRunner hackerOrRunner;

    CharacterData runner;
    HackerData hacker;

    private void DoSetup()
    {

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
