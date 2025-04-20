using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayFootSteps()
    {
        float randomPitch = Random.Range(0.75f, 1.25f);
        audioSource.pitch = randomPitch;
        audioSource.Play();
    }
}
