using DG.Tweening;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

public class Tab : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Transform sectionButton;
    [SerializeField] Animator anim;
    [SerializeField] RectTransform canvasRTF;
    [SerializeField] RectTransform rectTF;

    [Header("Settings")]
    [SerializeField] Vector2 OpenSizeRange;
    [SerializeField] bool randomizePosition;

    [Header("Physics")]
    [SerializeField] float drag;
    [SerializeField] float bounceDrag;

    //PHYSICS
    Vector2 velocity;
    Vector2 lastVelPos;

    //GRAB
    [SerializeField] Vector3? lastPos;
    Vector2 cursorOffset;
    public bool maximized {  get; private set; }
    bool holding;

    private void Update()
    {
        //Vector3 newPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (holding && Physics.Raycast (ray, out RaycastHit hit))
        {
            transform.position = hit.point - (Vector3)cursorOffset;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
            LimitMovement();
        }
        else
        {
            transform.position += (Vector3) velocity;
            Bounce();
        }

        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
    }
    private void FixedUpdate()
    {
        if (holding)
        {
            velocity = ((Vector2)transform.position - lastVelPos) / transform.lossyScale * Time.fixedDeltaTime;
            lastVelPos = transform.position;
        }
        else
        {
            velocity *= 1 - (drag * Time.fixedDeltaTime);
        }
    }

    public void StartGrab()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            holding = true;
            BringToFront();
            cursorOffset = hit.point - transform.position;
        }
    }
    public void EndGrab()
    {
        holding = false;
        cursorOffset = Vector2.zero;
    }

    public void Minimize()
    {
        if (maximized)
        {
            Vector3 buttonPos = sectionButton.position;
            buttonPos.z = transform.position.z;

            maximized = false;
            lastPos = transform.position;
            anim.SetTrigger("Close");

            transform.DOKill();
            transform.DOMove(buttonPos, 0.5f).SetEase(Ease.OutCubic).OnComplete(()=> gameObject.SetActive(false));
        }
    }
    public void Maximize()
    {
        if (!maximized)
        {
            Vector3 buttonPos = sectionButton.position;
            buttonPos.z = transform.position.z;

            transform.position = buttonPos;
            gameObject.SetActive(true);

            //RANDOMIZATION
            transform.localScale = Vector3.one * Random.Range(OpenSizeRange.x, OpenSizeRange.y);
            if (lastPos == null || randomizePosition)
            {
                float[] xBounds = 
                    { 
                    canvasRTF.position.x - (canvasRTF.rect.width * 0.5f * canvasRTF.lossyScale.x) + (rectTF.rect.width * 0.5f * rectTF.lossyScale.x),
                    canvasRTF.position.x + (canvasRTF.rect.width * 0.5f * canvasRTF.lossyScale.x) - (rectTF.rect.width * 0.5f * rectTF.lossyScale.x)
                };
                float[] yBounds = 
                    { 
                    canvasRTF.position.y - (canvasRTF.rect.height * 0.4f * canvasRTF.lossyScale.y) + (rectTF.rect.height * 0.5f * rectTF.lossyScale.y),
                    canvasRTF.position.y + (canvasRTF.rect.height * 0.5f * canvasRTF.lossyScale.y) - (rectTF.rect.height * 0.5f * rectTF.lossyScale.y)
                };

                lastPos = new Vector3(Random.Range(xBounds[0], xBounds[1]), Random.Range(yBounds[0], yBounds[1]), transform.position.z);
            }

            maximized = true;
            anim.SetTrigger("Open");

            transform.DOKill();
            if (velocity.magnitude < 1) transform.DOMove((Vector3) lastPos, 0.5f).SetEase(Ease.OutCubic);

            BringToFront();
        }
    }

    public void ToogleMaximize()
    {
        if (!maximized)
        {
            Maximize();
        }
        else
        {
            Minimize();
        }
    }

    void Bounce()
    {
        float[] xBounds = { canvasRTF.position.x - (canvasRTF.rect.width * 0.5f * canvasRTF.lossyScale.x), canvasRTF.position.x + (canvasRTF.rect.width * 0.5f * canvasRTF.lossyScale.x) };    
        float[] yBounds = { canvasRTF.position.y - (canvasRTF.rect.height * 0.5f * canvasRTF.lossyScale.y), canvasRTF.position.y + (canvasRTF.rect.height * 0.5f * canvasRTF.lossyScale.y) };

        float[] xSelfBounds = { rectTF.position.x - (rectTF.rect.width * 0.5f * rectTF.lossyScale.x), rectTF.position.x + (rectTF.rect.width * 0.5f * rectTF.lossyScale.x) };
        float[] ySelfBounds = { rectTF.position.y - (rectTF.rect.height * 0.5f * rectTF.lossyScale.y), rectTF.position.y + (rectTF.rect.height * 0.5f * rectTF.lossyScale.y) };

        Vector2 newVel = velocity;

        if (xSelfBounds[0] < xBounds[0]) newVel.x = Mathf.Abs(newVel.x);
        else if (xSelfBounds[1] > xBounds[1]) newVel.x = -Mathf.Abs(newVel.x);

        if (ySelfBounds[0] < yBounds[0]) newVel.y = Mathf.Abs(newVel.y);
        else if (ySelfBounds[1] > yBounds[1]) newVel.y = -Mathf.Abs(newVel.y);

        if (velocity != newVel)
        {
            newVel *= 1 - (bounceDrag * Time.fixedDeltaTime);
            velocity = newVel;
        }
    }
    void LimitMovement()
    {
        float[] xBounds = { canvasRTF.position.x - (canvasRTF.rect.width * 0.4f * canvasRTF.lossyScale.x), canvasRTF.position.x + (canvasRTF.rect.width * 0.4f * canvasRTF.lossyScale.x) };
        float[] yBounds = { canvasRTF.position.y - (canvasRTF.rect.height * 0.4f * canvasRTF.lossyScale.y), canvasRTF.position.y + (canvasRTF.rect.height * 0.48f * canvasRTF.lossyScale.y) };

        float[] xSelfBounds = { rectTF.position.x - (rectTF.rect.width * 0.5f * rectTF.lossyScale.x), rectTF.position.x + (rectTF.rect.width * 0.5f * rectTF.lossyScale.x) };
        float[] ySelfBounds = { rectTF.position.y - (rectTF.rect.height * 0.5f * rectTF.lossyScale.y), rectTF.position.y + (rectTF.rect.height * 0.5f * rectTF.lossyScale.y) };

        Vector3 position = transform.position;

        if (xSelfBounds[1] < xBounds[0]) position.x = xBounds[0] - (rectTF.rect.width * 0.5f * rectTF.lossyScale.x);
        else if (xSelfBounds[0] > xBounds[1]) position.x = xBounds[1] + (rectTF.rect.width * 0.5f * rectTF.lossyScale.x);

        if (ySelfBounds[1] < yBounds[0]) position.y = yBounds[0] - (rectTF.rect.height * 0.5f * rectTF.lossyScale.y);
        else if (ySelfBounds[1] > yBounds[1]) position.y = yBounds[1] - (rectTF.rect.height * 0.5f * rectTF.lossyScale.y);

        transform.position = position;
    }

    void BringToFront()
    {
        transform.SetSiblingIndex(transform.parent.childCount - 1);
    }
}
