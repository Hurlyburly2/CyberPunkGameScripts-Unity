using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSFX : MonoBehaviour
{
    [SerializeField] BattleSoundObject battleSoundObject;

    [SerializeField] AudioClip airHiss;
    [SerializeField] AudioClip cyberOne;
    [SerializeField] AudioClip cyberTwo;
    [SerializeField] AudioClip cyberThree;
    [SerializeField] AudioClip cyberFour;
    [SerializeField] AudioClip cyberGlitch;
    [SerializeField] List<AudioClip> drawCardSounds;
    [SerializeField] List<AudioClip> buttonSounds;
    [SerializeField] AudioClip heartBeat;
    [SerializeField] AudioClip hitBludgeon;
    [SerializeField] AudioClip gunChargedShot;
    [SerializeField] AudioClip gunPistolBarrage;
    [SerializeField] AudioClip gunPistolShot;
    [SerializeField] AudioClip gunReloadOne;
    [SerializeField] AudioClip gunUnholster;
    [SerializeField] List<AudioClip> playCardSounds;
    [SerializeField] AudioClip powerUpBuff;
    [SerializeField] AudioClip powerUpHeal;
    [SerializeField] AudioClip powerUpUpgrade;
    [SerializeField] AudioClip punchNormal;
    [SerializeField] AudioClip swordEnergy;
    [SerializeField] AudioClip wooshCyber;
    [SerializeField] AudioClip weaknessOne;
    [SerializeField] AudioClip weaknessTwo;
    [SerializeField] AudioClip wooshFast;
    

    int soundsCurrentlyPlaying = 0;
    public enum BattleSoundEffect
    {
        None = 0,
        AirHiss = 16,
        ButtonSound = 2,
        CyberOne = 12,
        CyberTwo = 14,
        CyberThree = 15,
        CyberFour = 18,
        CyberGlitch = 13,
        DrawCard = 1,
        GunChargedShot = 25,
        GunPistolBarrage = 23,
        GunPistolShot = 24,
        GunReloadOne = 19,
        GunUnholster = 20,
        HeartBeat = 17,
        HitBludgeon = 11,
        PlayCard = 4,
        PowerUpHeal = 7,
        PowerUpBuff = 6,
        PowerUpUpgrade = 9,
        PunchNormal = 3,
        SwordEnergy = 21,
        WooshCyber = 5,
        WeaknessOne = 8,
        WeaknessTwo = 22,
        WooshFast = 10
    }; // Highest Number: 25

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
                case BattleSoundEffect.CyberFour:
                    PlaySound(cyberFour);
                    break;
                case BattleSoundEffect.CyberGlitch:
                    PlaySound(cyberGlitch);
                    break;
                case BattleSoundEffect.DrawCard:
                    randomIndex = Random.Range(0, drawCardSounds.Count);
                    PlaySound(drawCardSounds[randomIndex]);
                    break;
                case BattleSoundEffect.GunChargedShot:
                    PlaySound(gunChargedShot);
                    break;
                case BattleSoundEffect.GunPistolBarrage:
                    PlaySound(gunPistolBarrage);
                    break;
                case BattleSoundEffect.GunPistolShot:
                    PlaySound(gunPistolShot);
                    break;
                case BattleSoundEffect.GunReloadOne:
                    PlaySound(gunReloadOne);
                    break;
                case BattleSoundEffect.GunUnholster:
                    PlaySound(gunUnholster);
                    break;
                case BattleSoundEffect.HeartBeat:
                    PlaySound(heartBeat);
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
                case BattleSoundEffect.SwordEnergy:
                    PlaySound(swordEnergy);
                    break;
                case BattleSoundEffect.WeaknessOne:
                    PlaySound(weaknessOne);
                    break;
                case BattleSoundEffect.WeaknessTwo:
                    PlaySound(weaknessTwo);
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
