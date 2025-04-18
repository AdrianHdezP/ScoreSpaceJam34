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

    [SerializeField] InputActionAsset inputActionAsset;

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
        Object[] subAssets = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(inputActionAsset));
        List<InputActionReference> inputActionReferences = new List<InputActionReference>();

        foreach (Object obj in subAssets)
        {
            // there are 2 InputActionReference returned for each InputAction in the asset, need to filter to not add the hidden one generated for backward compatibility
            if (obj is InputActionReference inputActionReference && (inputActionReference.hideFlags & HideFlags.HideInHierarchy) == 0)
            {
                inputActionReferences.Add(inputActionReference);
            }
        }

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
