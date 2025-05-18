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

public class AudioControl : MonoBehaviour
{
    public AudioMixer audioMixer;
    [SerializeField] float volumeDefault = 0.55f;

    [Header("UI")]
    public AudioData cursorClick;

    private void Start()
    {
        //AÑADE CATEGORIAS DE SONIDOS
        if (!PlayerPrefs.HasKey("MasterVolume")) PlayerPrefs.SetFloat("MasterVolume", 1);
        if (!PlayerPrefs.HasKey("MusicVolume")) PlayerPrefs.SetFloat("MusicVolume", volumeDefault);
        if (!PlayerPrefs.HasKey("EffectsVolume")) PlayerPrefs.SetFloat("EffectsVolume", volumeDefault);

        //SETEA AL ULTIMO VALOR GUARDADO
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume")) * 20);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
        audioMixer.SetFloat("EffectsVolume", Mathf.Log10(PlayerPrefs.GetFloat("EffectsVolume")) * 20);
    }
    private void OnEnable()
    {
        //AÑADE CATEGORIAS DE SONIDOS
        if (!PlayerPrefs.HasKey("MasterVolume")) PlayerPrefs.SetFloat("MasterVolume", volumeDefault);
        if (!PlayerPrefs.HasKey("MusicVolume")) PlayerPrefs.SetFloat("MusicVolume", 1);
        if (!PlayerPrefs.HasKey("EffectsVolume")) PlayerPrefs.SetFloat("EffectsVolume", 1);

        //SETEA AL ULTIMO VALOR GUARDADO
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume")) * 20);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
        audioMixer.SetFloat("EffectsVolume", Mathf.Log10(PlayerPrefs.GetFloat("EffectsVolume")) * 20);
    }

    //USE FOR EXTERNAL FUNCTION CALLS
    //public void PlaySoundClip() => GameST.inst.audioControl.PlaySoundClip(GameST.inst.audioClips.cursorClick, transform.position);
    public void PlaySoundClip(AudioData _audioData, Vector3 spawnPosition)
    {
        GameObject audioGameobject = new GameObject();
        AudioSource audioSource = audioGameobject.AddComponent<AudioSource>();
        audioGameobject.transform.position = spawnPosition;

        audioSource.outputAudioMixerGroup = _audioData.mixer;

        int randomChoice = UnityEngine.Random.Range(0, _audioData.audioClip.Length);

        audioSource.clip = _audioData.audioClip[randomChoice];
        audioSource.volume = _audioData.audioVolume;

        if (_audioData.randomPitch)
        {
            float myRandomPitch = UnityEngine.Random.Range(_audioData.minPitch, _audioData.maxPitch);
            audioSource.pitch = myRandomPitch;
        }

        audioSource.Play();

        AutoDestroy autoDestroy = audioGameobject.AddComponent<AutoDestroy>();
        float clipLength = audioSource.clip.length;
        autoDestroy.time = clipLength;
    }
}
