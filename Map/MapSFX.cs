using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSFX : MonoBehaviour
{
    [SerializeField] MapSoundEffectObject soundEffectObject;

    [SerializeField] List<AudioClip> buttonSounds;
    [SerializeField] List<AudioClip> playerMove;
    [SerializeField] AudioClip gainReward;
    [SerializeField] AudioClip powerUp;
    [SerializeField] AudioClip upgrade;
    [SerializeField] AudioClip firstAid;
    [SerializeField] AudioClip trap;
    [SerializeField] List<AudioClip> doHack;
    [SerializeField] AudioClip transitionToHack;

    int soundsCurrentlyPlaying = 0;
    public enum MapSoundEffect { ButtonPress, PlayerMove, GainReward, PowerUp, Upgrade,
        FirstAid, Trap, DoHack, TransitionToHack };

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
                case MapSoundEffect.GainReward:
                    PlaySound(gainReward);
                    break;
                case MapSoundEffect.PowerUp:
                    PlaySound(powerUp);
                    break;
                case MapSoundEffect.Upgrade:
                    PlaySound(upgrade);
                    break;
                case MapSoundEffect.FirstAid:
                    PlaySound(firstAid);
                    break;
                case MapSoundEffect.Trap:
                    PlaySound(trap);
                    break;
                case MapSoundEffect.DoHack:
                    randomIndex = Random.Range(0, doHack.Count);
                    PlaySound(doHack[randomIndex]);
                    break;
                case MapSoundEffect.TransitionToHack:
                    PlaySound(transitionToHack);
                    break;
            }
        }
    }

    private void PlaySound(AudioClip soundToPlay, bool loop = false)
    {
        // By default, play without a loop
        MapSoundEffectObject newSound = Instantiate(soundEffectObject);
        soundsCurrentlyPlaying++;
        newSound.PlaySound(soundToPlay, loop, this);
    }

    public void NotifyOfSoundCompletion()
    {
        soundsCurrentlyPlaying--;
    }
}
