using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSoundEffectObject : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    bool startedPlaying;
    MapSFX parent;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void PlaySound(AudioClip audioClip, bool loop, MapSFX newParent)
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
