using UnityEngine;

public class LanguageMenu : MonoBehaviour
{
    public void SetLenguage(int index)
    {
        GameST.inst.language.SetLanguage(index);
    }
}
