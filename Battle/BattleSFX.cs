using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSFX : MonoBehaviour
{
    [SerializeField] BattleSoundObject battleSoundObject;

    [SerializeField] List<AudioClip> drawCardSounds;

    int soundsCurrentlyPlaying = 0;
    public enum BattleSoundEffect
    {
        DrawCard
    };

    public void PlayMapSoundSFX(BattleSoundEffect soundEffect)
    {
        if (soundsCurrentlyPlaying < 15)
        {
            switch (soundEffect)
            {
                case BattleSoundEffect.DrawCard:
                    int randomIndex = Random.Range(0, drawCardSounds.Count);
                    PlaySound(drawCardSounds[randomIndex]);
                    break;
            }
        }
    }

    private void PlaySound(AudioClip soundToPlay, bool loop = false)
    {
        // By default, play without a loop
        BattleSoundObject newSound = Instantiate(battleSoundObject);
        soundsCurrentlyPlaying++;
        newSound.PlaySound(soundToPlay, loop, this);
    }

    public void NotifyOfSoundCompletion()
    {
        soundsCurrentlyPlaying--;
    }
}
