using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public Sound[] sounds;
    public static AudioManager Instance
    {
        get
        {
            return instance;
        }
    }
    private void Awake()
    {
        if (instance)
        {
            Destroy(instance);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        foreach(Sound s in sounds) // �� sound ��ü�� AudioSource ������Ʈ ���� �� ���� ����.
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.originalVolume = s.volume;

            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
    
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name); // sounds �迭 �� name�� ���� �̸��� ���� sound ã��.

        if(s == null) // sound�� ã�� ���� ���,
        {
            Debug.LogWarning("Sound : " + name + " Not Found!");
            return;
        }
        s.source.Play();
    }

    public void SetPitch(string name, float value)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name); // sounds �迭 �� name�� ���� �̸��� ���� sound ã��.

        if (s == null) // sound�� ã�� ���� ���,
        {
            Debug.LogWarning("Sound : " + name + " Not Found!");
            return;
        }
        s.source.pitch = value;
    }

    public float GetPitch(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name); // sounds �迭 �� name�� ���� �̸��� ���� sound ã��.

        if (s == null) // sound�� ã�� ���� ���,
        {
            Debug.LogWarning("Sound : " + name + " Not Found!");
            return 1f;
        }
        return s.source.pitch;
    }

    public void ToggleBackground(bool isOn)
    {
        foreach (Sound s in sounds)
        {
            if(s.soundType == Sound.SoundTypes.background)
            {
                if (isOn)
                {
                    s.source.volume = s.originalVolume;
                }
                else
                {
                    s.source.volume = 0f;
                }
            }
        }
    }

    public void ToggleSoundEffect(bool isOn)
    {
        foreach (Sound s in sounds)
        {
            if (s.soundType == Sound.SoundTypes.effect)
            {
                if (isOn)
                {
                    s.source.volume = s.originalVolume;
                }
                else
                {
                    s.source.volume = 0f;
                }
            }
        }
    }
}
