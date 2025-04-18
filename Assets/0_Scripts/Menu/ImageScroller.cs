using UnityEngine;
using UnityEngine.UI;

public class ImageScroller : MonoBehaviour
{
    [SerializeField] private RawImage _img;
    [SerializeField] private float speed;
    [SerializeField] private float angle;

    private void Update()
    {
        Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized ;

        _img.uvRect = new Rect(_img.uvRect.position + direction * speed * Time.deltaTime, _img.uvRect.size);
    }
}
