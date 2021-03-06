﻿using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1;
    [Range(0f, 3f)] public float pitch = 1;
    [Range(0, 256)] public int priority = 128;
    public bool play_looped;
    [HideInInspector] public AudioSource audio_source;

    void play ()
    {
        audio_source.Play ();
    }
}
