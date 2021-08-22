using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Static instance of AudioManager which allows it to be accessed by any other script.
    public static AudioManager Instance = null;
    [SerializeField] private Sound[] sounds_array;

    // Start is called before the first frame update
    void Awake ()
    {
        // check there is only one instance of this and that it is not destroyed on load
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy (gameObject);
        DontDestroyOnLoad (gameObject);

        foreach (Sound sound in sounds_array)
        {
            sound.audio_source = this.gameObject.AddComponent<AudioSource> ();
            sound.audio_source.playOnAwake = false;
            sound.audio_source.clip = sound.clip;

            //sound.name = sound.clip.name;
            sound.audio_source.volume = sound.volume;
            sound.audio_source.pitch = sound.pitch;
            sound.audio_source.priority = sound.priority;
            sound.audio_source.loop = sound.play_looped;

        }
    }

    // Update is called once per frame
    void Start ()
    {
        //Play ("Theme");
        Play (SoundNames.UnderwaterGrowl);
    }

    private void Update ()
    {
        if (Time.time > 10f && Time.time < 12f)
            Play (SoundNames.Theme);
    }

    public void Play (int sound)
    {
        Sound sound_found = null;

        if (sounds_array[sound].clip)
            sound_found = sounds_array[sound];

        if (sound_found == null || sound_found.audio_source.isPlaying) return;

        sound_found.audio_source.Play ();
    }

    public void Play (SoundNames sound_name)
    {
        Sound sound_found = null;

        if (Array.Find (sounds_array, sound => sound.name == sound_name).clip)
            sound_found = Array.Find (sounds_array, sound => sound.name == sound_name);

        if (sound_found == null || sound_found.audio_source.isPlaying) return;

        if (sound_found.clip)
            sound_found.audio_source.Play ();
    }
}
