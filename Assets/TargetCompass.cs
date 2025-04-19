using UnityEngine;

public class TargetCompass : MonoBehaviour
{
    [SerializeField] Vector2 alphaRange;
    [SerializeField] Vector2 sizeRange;
    [SerializeField] Vector2 distanceRange;

    [SerializeField] SpriteRenderer compassSprite;

    DeliveryManager manager;

    private void Awake()
    {
        manager = FindFirstObjectByType<DeliveryManager>();
    }

    private void Update()
    {
        Vector2 pos = manager.ReturnCurrentObjective();
        float distance = Vector2.Distance(pos, transform.position);

        if (distance > distanceRange.x)
        {
            compassSprite.gameObject.SetActive(true);

            transform.up = ((Vector3) pos - transform.position).normalized;
            transform.localScale = Vector3.one * Mathf.Clamp(distance * sizeRange.y / distanceRange.y ,sizeRange.x, sizeRange.y);

            Color color = compassSprite.color;
            color.a = Mathf.Clamp(distance * alphaRange.y / distanceRange.y, alphaRange.x, alphaRange.y);
            compassSprite.color = color;
        }
        else
        {
            compassSprite.gameObject.SetActive(false);
        }

    }
}
