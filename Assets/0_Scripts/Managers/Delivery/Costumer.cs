using UnityEngine;

public class Costumer : MonoBehaviour
{
    private DeliveryManager deliveryManager;

    private void Awake()
    {
        deliveryManager = FindFirstObjectByType<DeliveryManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out BallController player))
            deliveryManager.CompleteDelivery();
    }
}
