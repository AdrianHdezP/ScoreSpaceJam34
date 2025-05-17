using System;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class AudioData
{
    public AudioClip[] audioClip;
    public float audioVolume = 1f;
    [Space]
    public bool randomPitch;
    public float minPitch = 0.85f;
    public float maxPitch = 1.15f;
    [Space]
    public AudioMixerGroup mixer;
}

public class AudioClipControl : MonoBehaviour
{
    [Header("UI")]
    public AudioData cursorClick;
}
