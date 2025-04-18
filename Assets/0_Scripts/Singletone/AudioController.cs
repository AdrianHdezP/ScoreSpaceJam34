using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    public AudioMixer audioMixer;
    [SerializeField] float volumeDefault = 0.55f;

    private void Start()
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
}
