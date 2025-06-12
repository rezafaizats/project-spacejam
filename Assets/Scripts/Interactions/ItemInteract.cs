using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemInteract : MonoBehaviour
{
    [SerializeField] private UnityEvent OnItemClick;

    void OnMouseDown()
    {
        OnItemClick?.Invoke();
    }

}
