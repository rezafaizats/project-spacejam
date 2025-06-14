using System;
using UnityEngine;

namespace Game_Mechanics
{
    public class DrillController : MonoBehaviour, IToolController
    {
        private const string ID = "drill";
        
        [SerializeField] private float damageMultiplier = 1f;
        [SerializeField] private GameObject drillObject;
        [SerializeField] private GameObject drillCursor;

        private bool isActive;
        private Camera cam;
        
        public string ToolId => ID;
        public void SetIsEquipped(bool isEquipped)
        {
            isActive = isEquipped;
            drillObject.SetActive(!isEquipped);
        }

        private void Start()
        {
            cam = Camera.main;
        }


        private void Update()
        {
            if (!isActive) return;

            bool hasRock = TryGetRock(out var rock);
            drillCursor.SetActive(hasRock);

            if (!hasRock) return;
            
            if (!Input.GetMouseButton(0)) return;
            rock.Damage(Time.deltaTime * damageMultiplier);

        }

        private bool TryGetRock(out Rock rock)
        {
            rock = null;
            
            var hits = Physics.RaycastAll(cam.ScreenPointToRay(Input.mousePosition));

            if (hits.Length == 0) return false;

            foreach (var hit in hits)
                if (!hit.collider.TryGetComponent(out rock)) return true;

            return false;
        }
    }
}
