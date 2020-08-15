using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPortrait : MonoBehaviour
{
    [SerializeField] Sprite[] images;
    // 0 = default
    // 1 = Runner
    // 2 = Hacker

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().sprite = images[0];
    }

    private void SetPortrait(Sprite image)
    {
        GetComponent<Image>().sprite = image;
    }

    public void SetRunnerPortrait(int runnerId)
    {
        switch(runnerId)
        {
            case 0:
                SetPortrait(images[1]);
                break;
        }
    }

    public void SetHackerPortrait(int hackerId)
    {
        switch(hackerId)
        {
            case 0:
                SetPortrait(images[2]);
                break;
        }
    }
}
