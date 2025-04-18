using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMenu : MonoBehaviour
{
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider effectsVolumeSlider;

    private void OnEnable()
    {
        SetValues();
    }

    public void SetMainVolume(float sliderValue)
    {
        MainSingletone.inst.audioControl.audioMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
    }
    public void SetMusicVolume(float sliderValue)
    {
        MainSingletone.inst.audioControl.audioMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }
    public void SetEffectsVolume(float sliderValue)
    {
        MainSingletone.inst.audioControl.audioMixer.SetFloat("EffectsVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("EffectsVolume", sliderValue);
    }

    void SetValues()
    {
        //ALINEA EL VALOR DEL SLIDER AL VALOR GUARDADO
        if (masterVolumeSlider) masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        if (musicVolumeSlider) musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        if (effectsVolumeSlider) effectsVolumeSlider.value = PlayerPrefs.GetFloat("EffectsVolume");
    }
}
