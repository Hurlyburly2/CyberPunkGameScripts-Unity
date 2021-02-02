using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSFX : MonoBehaviour
{
    [SerializeField] MapSoundEffectObject soundEffectObject;

    [SerializeField] List<AudioClip> buttonSounds;
    [SerializeField] List<AudioClip> playerMove;

    int soundsCurrentlyPlaying = 0;
    public enum MapSoundEffect { ButtonPress, PlayerMove };

    public void PlayMapSoundSFX(MapSoundEffect soundEffect)
    {
        if (soundsCurrentlyPlaying < 15)
        {
            switch (soundEffect)
            {
                case MapSoundEffect.ButtonPress:
                    int randomIndex = Random.Range(0, buttonSounds.Count);
                    PlaySound(buttonSounds[randomIndex]);
                    break;
                case MapSoundEffect.PlayerMove:
                    randomIndex = Random.Range(0, playerMove.Count);
                    PlaySound(playerMove[randomIndex]);
                    break;
            }
        }
    }

    private void PlaySound(AudioClip soundToPlay, bool loop = false)
    {
        // By default, play without a loop
        MapSoundEffectObject newSound = Instantiate(soundEffectObject);
        newSound.transform.SetParent(this.transform);
        soundsCurrentlyPlaying++;
        newSound.PlaySound(soundToPlay, loop, this);
    }

    public void NotifyOfSoundCompletion()
    {
        soundsCurrentlyPlaying--;
    }
}
