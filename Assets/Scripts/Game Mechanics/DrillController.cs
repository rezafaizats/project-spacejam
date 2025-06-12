using System;
using UnityEngine;

namespace Game_Mechanics
{
    public class DrillController : MonoBehaviour, IToolController
    {
        private const string ID = "drill";
        
        [SerializeField] private float damageMultiplier = 1f;

        private bool isActive;
        private Camera cam;
        
        public string ToolId => ID;
        public void SetIsEquipped(bool isEquipped)
        {
            isActive = isEquipped;
        }

        private void Start()
        {
            cam = Camera.main;
        }


        private void Update()
        {
            if (!isActive) return;
            if (!Input.GetMouseButton(0)) return;

            var hits = Physics.RaycastAll(cam.ScreenPointToRay(Input.mousePosition));
            if (hits.Length == 0) return;

            foreach (var hit in hits)
            {
                Debug.Log($"Hitting {hit.collider.name}");
                if (!hit.collider.TryGetComponent<Rock>(out var rock)) continue;
            
                rock.Damage(Time.deltaTime * damageMultiplier);
                break;
            }
        }
    }
}
