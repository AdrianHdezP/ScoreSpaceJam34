using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class GameST : MonoBehaviour
{
    public static GameST inst { get; private set; }
    public StorageControl storageControl { get; private set; }
    public LanguageControl language { get; private set; }
    public AudioControl audioControl { get; private set; }
    public SceneControl sceneControl { get; private set; }

    [SerializeField] InputActionReference[] inputActionReferences;

    private void Awake()
    {
        if (inst == null) inst = this;
        else Destroy(this.gameObject);

        storageControl = GetComponent<StorageControl>();
        language = GetComponent<LanguageControl>();
        audioControl = GetComponent<AudioControl>();
        sceneControl = GetComponent<SceneControl>();

        LoadActionsBindings();
    }

    private void LoadActionsBindings()
    {
       foreach (InputActionReference m_Action in inputActionReferences)
       {
           string savedBindings = PlayerPrefs.GetString(m_Action.action.name);
   
           if (!string.IsNullOrEmpty(savedBindings))
           {
               m_Action.action.actionMap.LoadBindingOverridesFromJson(savedBindings);
                Debug.Log("Binging Loaded: " + m_Action.action.name);
           }
       }
    }
}
