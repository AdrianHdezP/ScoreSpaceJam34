using UnityEngine;

public class LanguageMenu : MonoBehaviour
{
    public void SetLenguage(int index)
    {
        MainSingletone.inst.language.SetLanguage(index);
    }
}
