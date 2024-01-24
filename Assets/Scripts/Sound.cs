using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public enum SoundTypes
    {
        background,
        effect,
    };

    [HideInInspector]
    public float originalVolume;

    [HideInInspector]
    public float originalPitch;

    public string name;
    public SoundTypes soundType;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
