using UnityEngine;

public class Randomizer : MonoBehaviour
{
    [SerializeField] Vector2 sizeRange;
    [SerializeField] Vector2 angleRange;

    void Start()
    {
        transform.localScale = Vector3.one * Random.Range(sizeRange.x, sizeRange.y);
        transform.localRotation = transform.localRotation * Quaternion.AngleAxis(Random.Range(angleRange.x, angleRange.y), Vector3.forward);
    }

}
