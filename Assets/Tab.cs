using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

public class Tab : MonoBehaviour
{
    public bool holding;
    public Vector2 cursorOffset;

    private void Update()
    {
        //Vector3 newPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (holding && Physics.Raycast (ray, out RaycastHit hit))
        {
            transform.position = hit.point - (Vector3)cursorOffset;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
        }
    }


    public void StartGrab()
    {
        holding = true;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            cursorOffset = hit.point - transform.position;
        }
    }

    public void EndGrab()
    {
        holding = false;
        cursorOffset = Vector2.zero;
    }

}
