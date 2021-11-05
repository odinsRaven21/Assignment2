using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camManager : MonoBehaviour
{
    private float zoomSpeed = 10f;
    private float targetZoom;
    private float min = 3f;
    private float max = 30f;
    private float smoothSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        targetZoom = Camera.main.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        float zoom = Input.GetAxis("Mouse ScrollWheel");

        if (zoom != 0.0f)
        {
            targetZoom -= zoom * zoomSpeed;
            targetZoom = Mathf.Clamp(targetZoom, min, max);
        }

        Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, targetZoom, smoothSpeed * Time.deltaTime);
    }
}
