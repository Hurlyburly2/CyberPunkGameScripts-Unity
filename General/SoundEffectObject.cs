using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectObject : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    bool startedPlaying;
    HubWorldSFX parent;

    public void PlaySound(AudioClip audioClip, bool loop, HubWorldSFX newParent)
    {
        parent = newParent;
        audioSource.clip = audioClip;
        audioSource.loop = loop;
        audioSource.Play();
        startedPlaying = true;
    }

    private void Update()
    {
        if (startedPlaying && !audioSource.isPlaying)
        {
            parent.NotifyOfSoundCompletion();
            Destroy(gameObject);
        }
    }
}
