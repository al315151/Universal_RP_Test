using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float dampTime = 0.5f;   
    public float sizeMargin = 5f;           
    public float minSize = 5f;
    public Transform[] targets;                  

    private new Camera camera;
    private float zoomVelocity;
    private Vector3 moveVelocity;

    private Vector3 targetPosition;
    private float targetSize;


    private void Awake ()
    {
        camera = GetComponentInChildren<Camera> ();
    }

    public void CenterCamera()
    {
        FindTargetPosition();
        transform.position = targetPosition;

        FindTargetSize();
        camera.orthographicSize = targetSize;
    }

    private void FixedUpdate()
    {
        Move ();
        Zoom ();
    }


    private void Move ()
    {
        FindTargetPosition();
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref moveVelocity, dampTime);
    }


    private void FindTargetPosition ()
    {
        Vector3 averagePosition = new Vector3 ();
        int targetCount = 0;

        for (int i = 0; i < targets.Length; i++)
        {
            if (!targets[i].gameObject.activeSelf)
            {
                continue;
            }

            averagePosition += targets[i].position;
            targetCount++;
        }

        if (targetCount > 0)
        {
            averagePosition /= targetCount;
        }

        // averagePosition.y = transform.position.y
        targetPosition = averagePosition;
    }


    private void Zoom ()
    {
        FindTargetSize();
        camera.orthographicSize = Mathf.SmoothDamp (camera.orthographicSize, targetSize, ref zoomVelocity, dampTime);
    }


    private void FindTargetSize ()
    {
        Vector3 screenCenterLocalSpace = camera.transform.InverseTransformPoint(targetPosition);
        targetSize = 0f;

       
        for (int i = 0; i < targets.Length; i++)
        {
            
            if (!targets[i].gameObject.activeSelf)
            {
                continue;
            }

            Vector3 targetPositionLocalSpace = camera.transform.InverseTransformPoint(targets[i].position);
            Vector3 localPosition = targetPositionLocalSpace - screenCenterLocalSpace;

            targetSize = Mathf.Max(targetSize, Mathf.Abs(localPosition.y));
            targetSize = Mathf.Max(targetSize, Mathf.Abs(localPosition.x) / camera.aspect);
        }

        targetSize += sizeMargin;
        targetSize = Mathf.Max (targetSize, minSize);
    }

}
