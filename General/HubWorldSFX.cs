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

    int soundsCurrentlyPlaying = 0;
    public enum HubSoundeffect { StartButton, ButtonPress, Selecting, SelectingInventorySlot, EquipItem };

    public void PlayHubSoundEffect(HubSoundeffect soundEffect)
    {
        if (soundsCurrentlyPlaying < 15)
        {
            switch (soundEffect)
            {
                case HubSoundeffect.StartButton:
                    PlaySound(startButtonSound, false);
                    break;
                case HubSoundeffect.ButtonPress:
                    int randomIndex = Random.Range(0, buttonSounds.Count);
                    PlaySound(buttonSounds[randomIndex], false);
                    break;
                case HubSoundeffect.Selecting:
                    randomIndex = Random.Range(0, selectSounds.Count);
                    PlaySound(selectSounds[randomIndex], false);
                    break;
                case HubSoundeffect.SelectingInventorySlot:
                    randomIndex = Random.Range(0, selectInventorySlotSounds.Count);
                    PlaySound(selectInventorySlotSounds[randomIndex], false);
                    break;
                case HubSoundeffect.EquipItem:
                    randomIndex = Random.Range(0, equipItemSounds.Count);
                    PlaySound(equipItemSounds[randomIndex], false);
                    break;
            }
        }
    }

    private void PlaySound(AudioClip soundToPlay, bool loop)
    {
        SoundEffectObject newSound = Instantiate(soundEffectObject);
        newSound.transform.SetParent(this.transform);
        soundsCurrentlyPlaying++;
        newSound.PlaySound(soundToPlay, loop, this);
    }

    public void NotifyOfSoundCompletion()
    {
        soundsCurrentlyPlaying--;
    }
}
