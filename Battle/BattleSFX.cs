using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSFX : MonoBehaviour
{
    [SerializeField] BattleSoundObject battleSoundObject;

    [SerializeField] AudioClip airHiss;
    [SerializeField] List<AudioClip> drawCardSounds;
    [SerializeField] List<AudioClip> buttonSounds;
    [SerializeField] AudioClip punchNormal;
    [SerializeField] List<AudioClip> playCardSounds;
    [SerializeField] AudioClip wooshCyber;
    [SerializeField] AudioClip powerUpBuff;
    [SerializeField] AudioClip powerUpHeal;
    [SerializeField] AudioClip weaknessOne;
    [SerializeField] AudioClip powerUpUpgrade;
    [SerializeField] AudioClip wooshFast;
    [SerializeField] AudioClip hitBludgeon;
    [SerializeField] AudioClip cyberOne;
    [SerializeField] AudioClip cyberGlitch;
    [SerializeField] AudioClip cyberTwo;
    [SerializeField] AudioClip cyberThree;

    int soundsCurrentlyPlaying = 0;
    public enum BattleSoundEffect
    {
        None, DrawCard, ButtonSound, PunchNormal, PlayCard, WooshCyber, PowerUpBuff, PowerUpHeal,
        WeaknessOne, PowerUpUpgrade, WooshFast, HitBludgeon, CyberOne, CyberGlitch, CyberTwo, CyberThree,
        AirHiss
    };

    public void PlayBattleSFX(BattleSoundEffect soundEffect)
    {
        if (soundsCurrentlyPlaying < 15)
        {
            switch (soundEffect)
            {
                case BattleSoundEffect.None:
                    Debug.Log("Used for cards with no sound effect...");
                    break;
                case BattleSoundEffect.AirHiss:
                    PlaySound(airHiss);
                    break;
                case BattleSoundEffect.ButtonSound:
                    int randomIndex = Random.Range(0, buttonSounds.Count);
                    PlaySound(buttonSounds[randomIndex]);
                    break;
                case BattleSoundEffect.CyberOne:
                    PlaySound(cyberOne);
                    break;
                case BattleSoundEffect.CyberTwo:
                    PlaySound(cyberTwo);
                    break;
                case BattleSoundEffect.CyberThree:
                    PlaySound(cyberThree);
                    break;
                case BattleSoundEffect.CyberGlitch:
                    PlaySound(cyberGlitch);
                    break;
                case BattleSoundEffect.DrawCard:
                    randomIndex = Random.Range(0, drawCardSounds.Count);
                    PlaySound(drawCardSounds[randomIndex]);
                    break;
                case BattleSoundEffect.HitBludgeon:
                    PlaySound(hitBludgeon);
                    break;
                case BattleSoundEffect.PowerUpBuff:
                    PlaySound(powerUpBuff);
                    break;
                case BattleSoundEffect.PowerUpHeal:
                    PlaySound(powerUpHeal);
                    break;
                case BattleSoundEffect.PunchNormal:
                    PlaySound(punchNormal);
                    break;
                case BattleSoundEffect.PlayCard:
                    randomIndex = Random.Range(0, playCardSounds.Count);
                    PlaySound(playCardSounds[randomIndex]);
                    break;
                case BattleSoundEffect.PowerUpUpgrade:
                    PlaySound(powerUpUpgrade);
                    break;
                case BattleSoundEffect.WeaknessOne:
                    PlaySound(weaknessOne);
                    break;
                case BattleSoundEffect.WooshCyber:
                    PlaySound(wooshCyber);
                    break;
                case BattleSoundEffect.WooshFast:
                    PlaySound(wooshFast);
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
