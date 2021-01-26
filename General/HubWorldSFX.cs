using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubWorldSFX : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    [SerializeField] AudioClip startButtonSound;

    public enum HubSoundeffect { StartButton };

    public void PlayHubSoundEffect(HubSoundeffect soundEffect)
    {
        switch (soundEffect)
        {
            case HubSoundeffect.StartButton:
                audioSource.clip = startButtonSound;
                audioSource.loop = false;
                audioSource.Play();
                break;
        }
    }
}
