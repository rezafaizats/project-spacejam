using System;
using Interactions;
using UnityEngine;

namespace Game_Mechanics
{
    public class ItemHighlightController : MonoBehaviour
    {
        private ItemHighlight current = null;
        private Camera cam;
        
        private void Start()
        {
            cam = Camera.main;
        }

        private void Update()
        {
            if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                DisableCurrent();
                return;
            }

            if (!hit.collider.TryGetComponent<ItemHighlight>(out var highlight))
            {
                DisableCurrent();
                return;
            }
            
            DisableCurrent();
            current = highlight;
            current.SetShow(true);
        }

        private void DisableCurrent()
        {
            if (current != null) current.SetShow(false);
            current = null;
        }
    }
}