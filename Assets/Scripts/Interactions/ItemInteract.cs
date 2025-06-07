using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemInteract : MonoBehaviour
{
    [SerializeField] private UnityEvent OnItemClick;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        OnItemClick?.Invoke();
    }

}
