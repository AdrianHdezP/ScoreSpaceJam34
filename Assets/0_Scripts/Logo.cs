using UnityEngine;
using UnityEngine.SceneManagement;

public class Logo : MonoBehaviour
{
    [SerializeField] AudioSource staticSound;
    [SerializeField] AudioSource thudSound;
    [SerializeField] AudioSource blinkSound;

    public void LoadMenuScene()
    {
        FindFirstObjectByType<SceneControl>().TVOffOut(1);
    }
    public void PlayStatic()
    {
        staticSound.Play();
    }
    public void StopStatic()
    {
        staticSound.Stop();
    }

    public void PlayThud()
    {
        thudSound.Play();
    }

    public void PlayBlick()
    {
        blinkSound.Stop();
    }
}
