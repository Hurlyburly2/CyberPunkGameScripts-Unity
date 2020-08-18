using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour
{
    [SerializeField] Sprite[] filterButtonsOn;
    [SerializeField] Sprite[] filterButtonsOff;

    [SerializeField] Image runnerFilterImage;
    [SerializeField] Image hackerFilterImage;
    [SerializeField] Image modFilterImage;
    [SerializeField] Image installFilterImage;

    // Filters
    bool runnerFilterOn = true;
    bool hackerFilterOn = true;
    bool modFilterOn = true;
    bool installFilterOn = true;

    public void SetupInventoryMenu()
    {
        SetFilterButtonImages();
    }

    public void CloseInventoryMenu()
    {
        gameObject.SetActive(false);
    }

    public void PressRunnerFilterBtn()
    {
        if (runnerFilterOn)
        {
            runnerFilterOn = false;
        } else
        {
            runnerFilterOn = true;
        }
        SetFilterButtonImages();
    }

    public void PressHackerFilterBtn()
    {
        if (hackerFilterOn)
        {
            hackerFilterOn = false;
            installFilterOn = false;
        } else
        {
            hackerFilterOn = true;
        }
        SetFilterButtonImages();
    }

    public void PressModFilterBtn()
    {
        if (modFilterOn)
        {
            modFilterOn = false;
        } else
        {
            modFilterOn = true;
        }
        SetFilterButtonImages();
    }

    public void PressInstallFilterBtn()
    {
        if (installFilterOn)
        {
            installFilterOn = false;
        } else
        {
            installFilterOn = true;
            hackerFilterOn = true;
        }
        SetFilterButtonImages();
    }

    private void SetFilterButtonImages()
    {
        if (runnerFilterOn)
        {
            runnerFilterImage.sprite = filterButtonsOn[0];
        } else
        {
            runnerFilterImage.sprite = filterButtonsOff[0];
        }

        if (hackerFilterOn)
        {
            hackerFilterImage.sprite = filterButtonsOn[1];
        } else
        {
            hackerFilterImage.sprite = filterButtonsOff[1];
        }

        if (modFilterOn)
        {
            modFilterImage.sprite = filterButtonsOn[2];
        } else
        {
            modFilterImage.sprite = filterButtonsOff[2];
        }

        if (installFilterOn)
        {
            installFilterImage.sprite = filterButtonsOn[3];
        } else
        {
            installFilterImage.sprite = filterButtonsOff[3];
        }
    }
}
