using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera cam;
    [SerializeField]
    float sizeTarget;
    [SerializeField]
    Vector3 posTarget;
    float zoomSpeed = 0.01f;
    float moveSpeed = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        sizeTarget = cam.orthographicSize;
        posTarget = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setSmoothCamera(float st, Vector3 pt)
    {
        sizeTarget = st;
        posTarget = pt;
        StartCoroutine(changeCam());
    }

    public void setZoomSpeed(float f)
    {
        zoomSpeed = f;
    }
    public void setMoveSpeed(float f)
    {
        moveSpeed = f;
    }

    IEnumerator changeCam()
    {
        while (cam.orthographicSize != sizeTarget || transform.position != posTarget)
        {
            cam.orthographicSize -= (cam.orthographicSize - sizeTarget) * zoomSpeed;
            transform.position -= (transform.position - posTarget) * moveSpeed;
            if (Mathf.Abs( cam.orthographicSize - sizeTarget) < 0.001f)
            {
                cam.orthographicSize = sizeTarget;
            }
            if (Mathf.Abs((transform.position - posTarget).magnitude) < 0.001f)
            {
                transform.position = posTarget;
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
