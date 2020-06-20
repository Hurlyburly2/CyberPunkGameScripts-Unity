using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentNodeMenuHacks : MonoBehaviour
{
    [SerializeField] MenuHackButton[] buttons;

    public void SetupButtons(List<HackTarget> hackTargets)
    {
        int count = 0;
        foreach (MenuHackButton button in buttons)
        {
            if (count < hackTargets.Count)
            {
                button.SetupButton(hackTargets[count]);
            } else
            {
                button.SetupButton();
            }
            count++;
        }
    }
}
