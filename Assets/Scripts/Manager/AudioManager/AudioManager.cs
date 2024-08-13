// using UnityEngine.Audio;
// using UnityEngine;
// using System;

// // This script is for audio in general.
// public class AudioManager : MonoBehaviour
// {
//     public Sound[] sounds;

//     public static AudioManager instance;
    
//     void Awake()
//     {
//         if(instance == null)
//         {
//             instance = this;
//         }
//         else
//         {
//             Destroy(gameObject);
//             return;
//         }

//         DontDestroyOnLoad(gameObject);

//         foreach (Sound s in sounds)
//         {
//             if (s.source == null)
//             {
//                 s.source = gameObject.AddComponent<AudioSource>();
//             }
//             s.source.clip = s.clip;

//             s.source.volume = s.volume;
//             s.source.pitch = s.pitch;
//             //s.source.priority = s.priority;
//             //s.source.spatialBlend = s.spatialBlend;
//             s.source.loop = s.loop;
//             s.source.playOnAwake = s.playOnAwake;
//         }
//     }

//     public void Play(String name)
//     {
//         Sound s = Array.Find(sounds, sound => sound.name == name);
//         if (s == null)
//         {
//             Debug.LogError($"Sound with name {name} not found.");
//             return;
//         }
//         if (s.source == null)
//         {
//             Debug.LogError($"AudioSource for sound {name} is not set.");
//             return;
//         }
//         //s.source.Play();
//         s.source.PlayOneShot(s.clip);
//     }

//     public void Stop(String name)
//     {
//         Sound s =Array.Find(sounds, sound => sound.name == name);
//         s.source.Stop();
//     }
// }
using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public int maxSimultaneousPlays = 3; // Maximum number of the same sound that can play simultaneously.

    public static AudioManager instance;

    private Dictionary<string, int> soundPlayCounts = new Dictionary<string, int>();

    void Awake()
    {
        if (instance == null)
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
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;

            // Initialize the play count dictionary
            soundPlayCounts[s.name] = 0;

            // Subscribe to the AudioSource's `Play` event to track when a sound stops playing
            s.source.loop = false;
            s.source.playOnAwake = false;
            s.source.Stop();
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError($"Sound with name {name} not found.");
            return;
        }

        if (soundPlayCounts[name] >= maxSimultaneousPlays)
        {
            Debug.Log($"Maximum simultaneous plays reached for sound {name}.");
            return;
        }

        if (s.source == null)
        {
            Debug.LogError($"AudioSource for sound {name} is not set.");
            return;
        }

        // Increase the play count and play the sound
        soundPlayCounts[name]++;
        s.source.PlayOneShot(s.clip);

        // Start a coroutine to decrease the play count once the sound finishes
        StartCoroutine(ResetPlayCount(s, name));
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError($"Sound with name {name} not found.");
            return;
        }

        s.source.Stop();
        soundPlayCounts[name] = 0; // Reset the play count when stopped manually
    }

    private IEnumerator ResetPlayCount(Sound sound, string name)
    {
        // Wait for the sound to finish playing
        yield return new WaitForSeconds(sound.clip.length);

        // Decrease the play count
        soundPlayCounts[name]--;
    }
}
