using UnityEngine.Audio;
using UnityEngine;
using System;

// This script is for audio in general.
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;
    
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            if (s.source == null)
            {
                s.source = gameObject.AddComponent<AudioSource>();
            }
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            //s.source.priority = s.priority;
            //s.source.spatialBlend = s.spatialBlend;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
        }
    }

    public void Play(String name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError($"Sound with name {name} not found.");
            return;
        }
        if (s.source == null)
        {
            Debug.LogError($"AudioSource for sound {name} is not set.");
            return;
        }
        s.source.Play();
    }

    public void Stop(String name)
    {
        Sound s =Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }
}
