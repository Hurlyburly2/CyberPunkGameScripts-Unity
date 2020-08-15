using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void CloseMainMenu()
    {
        gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        // Test if this actually works on ipad
        Application.Quit();
    }
}
