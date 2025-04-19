using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] float time;
    float t;

    private void Update()
    {
        t += Time.deltaTime;
        if (t >= time) Destroy(gameObject);
    }
}
