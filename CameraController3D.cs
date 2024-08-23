using UnityEngine;
using System.Collections;

public class CameraController3D : MonoBehaviour
{
    private const float Y_ANGLE_MIN = 0.0f;
    private const float Y_ANGLE_MAX = 50.0f;

    public Transform lookAt;
    public Transform camTransform;

    private Camera cam;

    private float distance = 2.0f;
    private float currentX = 0.0f;
    private float currentY = 2.0f;
    private float sesitivityX = 4.0f;
    private float sesitivityY = 1.0f;

    public bool first_person;

    Vector3 ahead;

    private void Start()
    {
        camTransform = transform;
        cam = Camera.main;
        ahead = new Vector3(0, 0.2f, 0);
    }

    private void Update()
    {
       currentX += Input.GetAxis("Mouse X");
       currentY += Input.GetAxis("Mouse Y");

       currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
    }
    private void LateUpdate()
    {
        if (!first_person)
        {
            Vector3 dir = new Vector3(0, 1, distance);
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
            camTransform.position = lookAt.position + rotation * dir;
            camTransform.LookAt(lookAt.position);
        }
        else
        {
            camTransform.position = lookAt.position;
            camTransform.Translate(ahead);
        }
    }
}



