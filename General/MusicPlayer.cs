using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    AudioSource audioSource;

    // track list
    [SerializeField] AudioClip homeBase;
    [SerializeField] AudioClip slumsIntro;
    [SerializeField] AudioClip slumsLoop;

    // state
    string currentTrack = "homeBase";
        // homeBase
        // slumsPreLoop, slumsPostLoop

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            switch (currentTrack)
            {
                case "slumsPreLoop":
                    audioSource.clip = slumsLoop;
                    audioSource.loop = true;
                    audioSource.Play();
                    currentTrack = "slumsPostLoop";
                    break;
            }
        }
    }

    public void ChangeTrack(string name)
    {
        switch (name)
        {
            case "homeBase":
                audioSource.clip = homeBase;
                audioSource.loop = true;
                currentTrack = "homeBase";
                audioSource.Play();
                break;
            case "slums":
                audioSource.clip = slumsIntro;
                audioSource.loop = false;
                currentTrack = "slumsPreLoop";
                audioSource.Play();
                break;
        }
    }
}
