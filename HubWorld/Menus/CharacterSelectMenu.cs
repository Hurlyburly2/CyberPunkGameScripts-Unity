using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectMenu : MonoBehaviour
{
    [SerializeField] LoadoutMenu loadoutMenu;
    Item.HackerRunner hackerOrRunner;

    [SerializeField] Sprite selectHackerSprite;
    [SerializeField] Sprite selectRunnerSprite;
    [SerializeField] Image headingTextImage;
    [SerializeField] Image currentSelectionPortrait;

    HackerData currentHacker;
    HackerData shownHacker;
    CharacterData currentRunner;
    CharacterData shownRunner;

    public void SetupCharacterSelectMenu(Item.HackerRunner newHackerOrRunner)
    {
        hackerOrRunner = newHackerOrRunner;
        PlayerData playerData = FindObjectOfType<PlayerData>();

        switch (hackerOrRunner)
        {
            case Item.HackerRunner.Runner:
                headingTextImage.sprite = selectRunnerSprite;
                currentRunner = playerData.GetCurrentRunner();
                shownRunner = currentRunner;
                break;
            case Item.HackerRunner.Hacker:
                headingTextImage.sprite = selectHackerSprite;
                currentHacker = playerData.GetCurrentHacker();
                shownHacker = currentHacker;
                break;
        }
        LoadCharacterPortrait();
    }

    private void LoadCharacterPortrait()
    {
        switch (hackerOrRunner)
        {
            case Item.HackerRunner.Runner:
                currentSelectionPortrait.sprite = Resources.Load<Sprite>("Characters/Runner" + shownRunner.getId() + "-Full");
                break;
            case Item.HackerRunner.Hacker:
                currentSelectionPortrait.sprite = Resources.Load<Sprite>("Characters/Hacker" + shownHacker.GetId() + "-Full");
                break;
        }
    }

    public void ConfirmSelectBtnPress()
    {
        // TODO CHARACTER SELECT STUFF IN HERE....
        CloseCharacterSelectMenu();
    }

    public void CloseCharacterSelectMenu()
    {
        loadoutMenu.SetupLoadoutMenu();
        gameObject.SetActive(false);
    }
}
