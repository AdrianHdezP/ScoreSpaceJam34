using UnityEngine;

public class Tilt : MonoBehaviour
{
    Vector2 cursorPos;
    [SerializeField] Vector2 maxTilt;


    private void Update()
    {
        cursorPos.x = Input.mousePosition.x / Screen.width;
        cursorPos.y = Input.mousePosition.y / Screen.height;

        Quaternion targetRot = Quaternion.identity;

        targetRot *= Quaternion.AngleAxis(-maxTilt.x * 0.5f + maxTilt.x * cursorPos.x, -Vector3.up);
        targetRot *= Quaternion.AngleAxis(-maxTilt.y * 0.5f + maxTilt.y * cursorPos.y, Vector3.right);


        transform.localRotation = targetRot;
    }

}
