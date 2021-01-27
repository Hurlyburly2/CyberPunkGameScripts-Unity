using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubWorldSFX : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    [SerializeField] AudioClip startButtonSound;
    [SerializeField] List<AudioClip> buttonSounds;

    public enum HubSoundeffect { StartButton, ButtonPress };

    public void PlayHubSoundEffect(HubSoundeffect soundEffect)
    {
        switch (soundEffect)
        {
            case HubSoundeffect.StartButton:
                audioSource.clip = startButtonSound;
                audioSource.loop = false;
                audioSource.Play();
                break;
            case HubSoundeffect.ButtonPress:
                int randomIndex = Random.Range(0, buttonSounds.Count);
                audioSource.clip = buttonSounds[randomIndex];
                audioSource.loop = false;
                audioSource.Play();
                break;
        }
    }
}
