using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstMenu : MonoBehaviour
{
    public void NewGameBtnPress()
    {
        FindObjectOfType<PlayerData>().SetupNewGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void CloseFirstMenu()
    {
        gameObject.SetActive(false);
    }
}
