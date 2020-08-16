using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPortrait : MonoBehaviour
{
    void Start()
    {
        //GetComponent<Image>().sprite = Resources.Load<Sprite>();
    }

    private void SetPortrait(string imageToLoad)
    {
        GetComponent<Image>().sprite = Resources.Load<Sprite>(imageToLoad);
    }

    public void SetRunnerPortrait(int runnerId)
    {
        string resourceToLoad = "Characters/Runner";
        switch(runnerId)
        {
            case 0:
                // First Runner
                resourceToLoad += "0-Portrait";
                SetPortrait(resourceToLoad);
                break;
        }
    }

    public void SetHackerPortrait(int hackerId)
    {
        string resourceToLoad = "Characters/Hacker";
        switch(hackerId)
        {
            case 0:
                // First Hacker
                resourceToLoad += "0-Portrait";
                SetPortrait(resourceToLoad);
                break;
        }
    }
}
