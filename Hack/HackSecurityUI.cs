using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HackSecurityUI : MonoBehaviour
{
    [SerializeField] List<Sprite> securityIconImages;
    [SerializeField] Image securityIconRenderer;
    [SerializeField] Image securityBarOne;
    [SerializeField] Image securityBarTwo;
    [SerializeField] Image securityBarThree;

    public void UpdateHackSecurityUI(int securityLevel)
    {
        SetSecurityIcon(securityLevel);
        SetSecurityBars(securityLevel);
    }

    private void SetSecurityIcon(int securityLevel)
    {
        switch (securityLevel)
        {
            case 0:
                securityIconRenderer.sprite = securityIconImages[0];
                break;
            case 1:
                securityIconRenderer.sprite = securityIconImages[1];
                break;
            case 2:
                securityIconRenderer.sprite = securityIconImages[2];
                break;
            case 3:
                securityIconRenderer.sprite = securityIconImages[3];
                break;
            default:
                securityIconRenderer.sprite = securityIconImages[4];
                break;
        }
    }

    private void SetSecurityBars(int securityLevel)
    {
        // This is so they can revert to off if the user decides to undo a move
        // that would have raised the security rating
        securityBarOne.gameObject.SetActive(false);
        securityBarTwo.gameObject.SetActive(false);
        securityBarThree.gameObject.SetActive(false);

        if (securityLevel > 0)
            securityBarOne.gameObject.SetActive(true);
        if (securityLevel > 1)
            securityBarTwo.gameObject.SetActive(true);
        if (securityLevel > 2)
            securityBarThree.gameObject.SetActive(true);
    }
}
