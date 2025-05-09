using UnityEngine;
using UnityEngine.SceneManagement;

public class Transitioner : MonoBehaviour
{
    [SerializeField] SceneControl controller;


    public void LoadScene()
    {
        SceneManager.LoadScene(controller.selectedScene);
    }

    public void DeactivateSelf()
    {
        gameObject.SetActive(false);
    }
}
