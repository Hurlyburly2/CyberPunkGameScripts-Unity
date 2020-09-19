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
        }
    }
}
