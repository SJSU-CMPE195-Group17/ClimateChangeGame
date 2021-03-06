﻿using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] sounds;
    void Awake()
    {
        instance = this;
        /*
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        */
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s !=null)
        {
            s.source.Play();
        }
        else
        {
            Debug.LogWarning("Sound " + name + " not found");
        }
    }

    public void PlayPitch (string name, float pitch)
    {
        print("Pitch:" + pitch);
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found");
            return;
        }
        s.source.pitch = pitch;
        s.source.Play();
        //s.source.pitch = s.pitch;
    }
}
