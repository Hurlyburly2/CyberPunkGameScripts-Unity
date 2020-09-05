using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectMenu : MonoBehaviour
{
    [SerializeField] LoadoutMenu loadoutMenu;
    Item.HackerRunner hackerOrRunner;

    [SerializeField] Sprite selectHackerSprite;
    [SerializeField] Sprite selectRunnerSprite;
    [SerializeField] Image headingTextImage;
    [SerializeField] Image currentSelectionPortrait;
    [SerializeField] TextMeshProUGUI characterBio;
    [SerializeField] TextMeshProUGUI characterName;

    HackerData currentHacker;
    HackerData shownHacker;
    CharacterData currentRunner;
    CharacterData shownRunner;

    List<CharacterData> runners;
    List<HackerData> hackers;
    int currentIndex;

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
                runners = playerData.GetRunnerList();
                for (int i = 0; i < runners.Count; i++)
                {
                    if (runners[i] == currentRunner)
                    {
                        currentIndex = i;
                        break;
                    }
                }
                break;
            case Item.HackerRunner.Hacker:
                headingTextImage.sprite = selectHackerSprite;
                currentHacker = playerData.GetCurrentHacker();
                shownHacker = currentHacker;
                hackers = playerData.GetHackerList();
                for (int i = 0; i < hackers.Count; i++)
                {
                    if (hackers[i] == currentHacker)
                    {
                        currentIndex = i;
                        break;
                    }
                }
                break;
        }
        UpdateDisplayedCharacter();
    }

    public void UpdateDisplayedCharacter()
    {
        switch (hackerOrRunner)
        {
            case Item.HackerRunner.Runner:
                characterBio.text = shownRunner.GetBio();
                characterName.text = shownRunner.GetRunnerName();
                break;
            case Item.HackerRunner.Hacker:
                characterBio.text = shownHacker.GetBio();
                characterName.text = shownHacker.GetName();
                break;
        }
        LoadCharacterPortrait();
    }

    public void LeftBtnPress()
    {
        switch (hackerOrRunner)
        {
            case Item.HackerRunner.Runner:
                if (currentIndex - 1 < 0)
                {
                    currentIndex = runners.Count - 1;
                } else
                {
                    currentIndex--;
                }
                shownRunner = runners[currentIndex];
                break;
            case Item.HackerRunner.Hacker:
                if (currentIndex - 1 < 0)
                {
                    currentIndex = hackers.Count - 1;
                } else
                {
                    currentIndex--;
                }
                shownHacker = hackers[currentIndex];
                break;
        }
        UpdateDisplayedCharacter();
    }

    public void RightBtnPress()
    {
        switch (hackerOrRunner)
        {
            case Item.HackerRunner.Runner:
                if (currentIndex + 1 >= runners.Count)
                {
                    currentIndex = 0;
                } else
                {
                    currentIndex++;
                }
                shownRunner = runners[currentIndex];
                break;
            case Item.HackerRunner.Hacker:
                if (currentIndex + 1 >= hackers.Count)
                {
                    currentIndex = 0;
                } else
                {
                    currentIndex++;
                }
                shownHacker = hackers[currentIndex];
                break;
        }
        UpdateDisplayedCharacter();
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
