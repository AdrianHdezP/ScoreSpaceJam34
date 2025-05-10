using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageControl : MonoBehaviour
{
    [Tooltip("0 = english, 1 = spanish")]
    public int LanguageId { get; private set; }
    private void Awake()
    {
        LoadLanguage();
    }
    void LoadLanguage()
    {
        if (PlayerPrefs.HasKey("Language")) SetLanguage(PlayerPrefs.GetInt("Language"));
        else
        {
            //DEFAULTS IT TO ENGLISH
            SetLanguage(0);
        }
    }
    public void SetLanguage(int newId)
    {
        LanguageId = newId;
        StartCoroutine(SetLocale(newId));    
        PlayerPrefs.SetInt("Language", LanguageId);
    }

    IEnumerator SetLocale(int _localeID)
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];
    }
}
