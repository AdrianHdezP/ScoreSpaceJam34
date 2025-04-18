using UnityEngine;

public class LanguageControl : MonoBehaviour
{
    [Tooltip("0 = null, 1 = english, 2 = spanish")]
    public int LanguageId { get; private set; }

    private void Awake()
    {
        LoadLanguage();
    }


    public void SetLanguage(int newId)
    {
        LanguageId = newId;

        foreach (TextTranslator text in FindObjectsByType<TextTranslator>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID))
        {
            text.SetText();
        }

        PlayerPrefs.SetInt("Language", LanguageId);
    }


    void LoadLanguage()
    {
        if (PlayerPrefs.HasKey("Language")) SetLanguage(PlayerPrefs.GetInt("Language"));
        else
        {
            //DEFAULTS IT TO ENGLISH
            SetLanguage(1);
        }
    }
}
