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
    // TODO: USE RESOURCES.LOAD PATTERN FOR MUSIC
    [SerializeField] AudioClip downtownIntro;
    [SerializeField] AudioClip downtownLoop;

    // state
    string currentTrack = "homeBase";
        // homeBase
        // slumsPreLoop, slumsPostLoop
        // downtownPreLoop, downtownPostLoop

    private void Awake()
    {
        int count = FindObjectsOfType<MusicPlayer>().Length;
        if (count > 1)
        {
            Destroy(gameObject);
        }
    }

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
                case "downtownPreLoop":
                    audioSource.clip = downtownLoop;
                    audioSource.loop = true;
                    audioSource.Play();
                    currentTrack = "downtownPostLoop";
                    break;
            }
        }
    }

    public void ChangeTrack(Job.JobArea name)
    {
        switch (name)
        {
            case Job.JobArea.HomeBase:
                audioSource.clip = homeBase;
                audioSource.loop = true;
                currentTrack = "homeBase";
                audioSource.Play();
                break;
            case Job.JobArea.Slums:
                audioSource.clip = slumsIntro;
                audioSource.loop = false;
                currentTrack = "slumsPreLoop";
                audioSource.Play();
                break;
            case Job.JobArea.Downtown:
                audioSource.clip = downtownIntro;
                audioSource.loop = false;
                currentTrack = "downtownPreLoop";
                audioSource.Play();
                break;
        }
    }
}
