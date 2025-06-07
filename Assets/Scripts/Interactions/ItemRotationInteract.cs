using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotationInteract : MonoBehaviour
{
    [SerializeField] private float interactSensitivity = 10f;

    private Vector3 rotation;
    private bool isRotating;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnMouseDrag()
    {
        float xRot = Input.GetAxis("Mouse Y") * interactSensitivity;
        float yRot = Input.GetAxis("Mouse X") * interactSensitivity;

        Vector3 rot = transform.rotation.eulerAngles + new Vector3(-xRot, -yRot, 0f); //use local if your char is not always oriented Vector3.up
        
        transform.eulerAngles = rot;
    }
    
    float ClampAngle(float angle, float from, float to)
    {
         // accepts e.g. -80, 80
         if (angle < 0f) angle = 360 + angle;
         if (angle > 180f) return Mathf.Max(angle, 360 + from);
         return Mathf.Min(angle, to);
    }

}
