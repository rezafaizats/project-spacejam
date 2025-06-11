using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float speed = 50f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InfiniteRotate();
    }

    public void InfiniteRotate()
    {
        Vector3 rotation = Vector3.zero;
        rotation += Vector3.up;
        transform.Rotate(rotation * speed * Time.deltaTime);
    }
}
