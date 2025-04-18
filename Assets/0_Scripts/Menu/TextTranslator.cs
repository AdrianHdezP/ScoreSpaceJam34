using System;
using TMPro;
using UnityEngine;

[RequireComponent (typeof(TextMeshProUGUI))]
public class TextTranslator : MonoBehaviour
{
    TextMeshProUGUI textMesh;

    [SerializeField] string textsEnglish;
    [SerializeField] string textsSpanish;

    private void Awake()
    {
        if (textMesh == null) textMesh = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        //SetText();
    }

    private void Start()
    {
        SetText();
    }

    public void SetText()
    {
        if (textMesh == null) textMesh = GetComponent<TextMeshProUGUI>();

        if (MainSingletone.inst.language.LanguageId == 1) textMesh.text = textsEnglish;
        else if (MainSingletone.inst.language.LanguageId == 2) textMesh.text = textsSpanish;
    }
}
