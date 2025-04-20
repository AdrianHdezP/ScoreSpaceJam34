using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource effectsSource;

    [Header("Music")]
    public AudioClip[] levelMusic;

    [Header("Effects")]
    public AudioClip footSteps;
    public AudioClip pickUp; //
    public AudioClip costumer; //
    public AudioClip bonus; //?//
    public AudioClip hit; //
    public AudioClip explosion; //
    public AudioClip clock; //?//
    public AudioClip[] aldeano; //?//

    public void PlayOneShoot(AudioSource audioSource, AudioClip newClip, float volume)
    {
        audioSource.volume = volume;
        audioSource.PlayOneShot(newClip);
    }

    public AudioClip ReturnRandomAudio(AudioClip[] audClipArray) => audClipArray[Random.Range(0, audClipArray.Length)];

}
