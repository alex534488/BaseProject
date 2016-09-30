using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraMove : MonoBehaviour {

    public bool mouseInput = true;
    public bool keyboardInput = true;

    [Header("Move")]
    public float margin = 20;
    public float speedCap = 10;
    public Vector2 xBounds;
    public Vector2 zBounds;

    [Header("Zoom")]
    public float zoomSpeed = 0.5f;
    public float minSize;
    public float maxSize;

    private Camera cam;
    private Vector2 currentSpeed = Vector2.zero;
    private float targetSize;

    void Awake()
    {
        cam = GetComponent<Camera>();
        targetSize = cam.orthographicSize;
    }

    void Update()
    {
        CheckMargins();
        CheckScroll();

        //position
        Vector3 newPos = transform.position + new Vector3(currentSpeed.x, 0, currentSpeed.y) * Time.deltaTime * (cam.orthographicSize / 10);
        //Check X bounds
        if (newPos.x < xBounds.x) newPos.x = xBounds.x;
        else if (newPos.x > xBounds.y) newPos.x = xBounds.y;
        //Check Z bounds
        if (newPos.z < zBounds.x) newPos.z = zBounds.x;
        else if (newPos.z > zBounds.y) newPos.z = zBounds.y;

        transform.position = newPos;
        currentSpeed = Vector2.MoveTowards(currentSpeed, Vector2.zero, 0.25f);

        //size
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, 0.1f);
    }

    void CheckMargins()
    {
        Vector3 pos = Input.mousePosition;
        Vector2 request = Vector2.zero;
        if (mouseInput)
        {
            if (pos.x < margin) request.x = -1;
            else if (pos.x > Screen.width - margin) request.x = 1;

            if (pos.y < margin) request.y = -1;
            else if (pos.y > Screen.height - margin) request.y = 1;
        }
        if (keyboardInput || Input.anyKey)
        {

            if (Input.GetKey(KeyCode.A)) request.x = -1;
            else if (Input.GetKey(KeyCode.D)) request.x = 1;

            if (Input.GetKey(KeyCode.S)) request.y = -1;
            else if (Input.GetKey(KeyCode.W)) request.y = 1;
        }

        if (request.x != 0 || request.y != 0) RequestMove(request);
    }

    void CheckScroll()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            targetSize -= scroll * zoomSpeed * (targetSize / 10);
            if (targetSize < minSize) targetSize = minSize;
            else if (targetSize > maxSize) targetSize = maxSize;
        }
    }

    void RequestMove(Vector2 direction)
    {
        currentSpeed = Vector2.MoveTowards(currentSpeed, direction* speedCap, 1f);
    }
}
