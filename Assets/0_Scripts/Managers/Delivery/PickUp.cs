using UnityEngine;

public class PickUp : MonoBehaviour
{
    private DeliveryManager deliveryManager;

    private void Awake()
    {
        deliveryManager = FindFirstObjectByType<DeliveryManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController player))
        {
            deliveryManager.CompletePickUp();
        }
    }
}
