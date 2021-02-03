using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSoundObject : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    bool startedPlaying;
    BattleSFX parent;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void PlaySound(AudioClip audioClip, bool loop, BattleSFX newParent)
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
