using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class MainSingletone : MonoBehaviour
{
    public static MainSingletone inst { get; private set; }
    public Score score { get; private set; }
    public LanguageControl language { get; private set; }
    public AudioController audioControl { get; private set; }
    public SceneControl sceneControl { get; private set; }

    [SerializeField] InputActionReference[] inputActionReferences;

    private void Awake()
    {
        if (inst == null) inst = this;
        else Destroy(this.gameObject);

        score = GetComponent<Score>();
        language = GetComponent<LanguageControl>();
        audioControl = GetComponent<AudioController>();
        sceneControl = GetComponent<SceneControl>();

        LoadActionsBindings();
    }

    private void LoadActionsBindings()
    {
       foreach (InputActionReference m_Action in inputActionReferences)
       {
           var savedBindings = PlayerPrefs.GetString(m_Action.action.name);
   
           if (!string.IsNullOrEmpty(savedBindings))
           {
               m_Action.action.actionMap.LoadBindingOverridesFromJson(savedBindings);
           }
       }
    }
}
