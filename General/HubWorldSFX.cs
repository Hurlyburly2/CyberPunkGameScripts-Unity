using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubWorldSFX : MonoBehaviour
{
    [SerializeField] SoundEffectObject soundEffectObject;

    [SerializeField] AudioClip startButtonSound;
    [SerializeField] List<AudioClip> buttonSounds;
    [SerializeField] List<AudioClip> selectSounds;
    [SerializeField] List<AudioClip> selectInventorySlotSounds;
    [SerializeField] List<AudioClip> equipItemSounds;
    [SerializeField] AudioClip buyItemSound;
    [SerializeField] AudioClip upgradeItemSound;
    [SerializeField] AudioClip transitionToMissionSound;

    int soundsCurrentlyPlaying = 0;
    public enum HubSoundeffect { StartButton, ButtonPress, Selecting, SelectingInventorySlot, EquipItem, BuyItem, UpgradeItem, TransitionToMission };

    public void PlayHubSoundEffect(HubSoundeffect soundEffect)
    {
        if (soundsCurrentlyPlaying < 15)
        {
            switch (soundEffect)
            {
                case HubSoundeffect.StartButton:
                    PlaySound(startButtonSound);
                    break;
                case HubSoundeffect.ButtonPress:
                    int randomIndex = Random.Range(0, buttonSounds.Count);
                    PlaySound(buttonSounds[randomIndex]);
                    break;
                case HubSoundeffect.Selecting:
                    randomIndex = Random.Range(0, selectSounds.Count);
                    PlaySound(selectSounds[randomIndex]);
                    break;
                case HubSoundeffect.SelectingInventorySlot:
                    randomIndex = Random.Range(0, selectInventorySlotSounds.Count);
                    PlaySound(selectInventorySlotSounds[randomIndex]);
                    break;
                case HubSoundeffect.EquipItem:
                    randomIndex = Random.Range(0, equipItemSounds.Count);
                    PlaySound(equipItemSounds[randomIndex]);
                    break;
                case HubSoundeffect.BuyItem:
                    PlaySound(buyItemSound);
                    break;
                case HubSoundeffect.UpgradeItem:
                    PlaySound(upgradeItemSound);
                    break;
                case HubSoundeffect.TransitionToMission:
                    PlaySound(transitionToMissionSound);
                    break;
            }
        }
    }

    private void PlaySound(AudioClip soundToPlay, bool loop=false)
    {
        // By default, play without a loop
        SoundEffectObject newSound = Instantiate(soundEffectObject);
        soundsCurrentlyPlaying++;
        newSound.PlaySound(soundToPlay, loop, this);
    }

    public void NotifyOfSoundCompletion()
    {
        soundsCurrentlyPlaying--;
    }
}
