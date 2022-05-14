using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void SetMasterVolume(float volume) // control the master volume
    {
        audioMixer.SetFloat("MasterVolume", volume);

    }

    public void SetMusicVolume(float volume) // control the Music volume
    {
        audioMixer.SetFloat("MusicVolume", volume);

    }

    public void SetSoundEffectVolume(float volume) // control the sound effect volume
    {
        audioMixer.SetFloat("SoundVolume", volume);

    }
}
