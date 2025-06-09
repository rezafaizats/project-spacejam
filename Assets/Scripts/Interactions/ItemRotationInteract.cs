using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotationInteract : MonoBehaviour
{
    [SerializeField] private float interactSensitivity = 10f;
    [SerializeField] private Transform cameraTransform;

    private Vector3 rotation;
    private bool isRotating = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1)) RotateItem();
    }


    private void RotateItem()
    {
        float xRot = -Input.GetAxis("Mouse Y") * interactSensitivity;
        float yRot = -Input.GetAxis("Mouse X") * interactSensitivity;
        
        Vector3 right = cameraTransform.right;
        Vector3 up = cameraTransform.up;

        Quaternion pitch = Quaternion.AngleAxis(-xRot, right);
        Quaternion yaw = Quaternion.AngleAxis(yRot, up); 

        transform.rotation = yaw * pitch * transform.rotation;
    }
    
    float ClampAngle(float angle, float from, float to)
    {
         // accepts e.g. -80, 80
         if (angle < 0f) angle = 360 + angle;
         if (angle > 180f) return Mathf.Max(angle, 360 + from);
         return Mathf.Min(angle, to);
    }

}
