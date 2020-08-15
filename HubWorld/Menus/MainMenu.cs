using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] LoadoutMenu loadoutMenu;

    public void CloseMainMenu()
    {
        gameObject.SetActive(false);
    }

    public void OpenLoadoutMenu()
    {
        loadoutMenu.gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        // Test if this actually works on ipad
        Application.Quit();
    }
}
