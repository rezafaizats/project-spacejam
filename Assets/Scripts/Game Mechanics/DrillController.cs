using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game_Mechanics
{
    public class DrillController : MonoBehaviour, IToolController
    {
        private const string ID = "drill";
        
        [SerializeField] private float damageMultiplier = 1f;
        [SerializeField] private GameObject drillObject;
        [SerializeField] private GameObject drillCursor;
        [SerializeField] private GameObject drillCursorVisual;

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

            bool hasRock = TryGetRockNew(out var rock, out var hit);
            drillCursor.SetActive(hasRock);

            if (!hasRock) return;

            drillCursor.transform.position = hit.point;
            
            if (!Input.GetMouseButton(0)) return;
            rock.Damage(Time.deltaTime * damageMultiplier);
            drillCursorVisual.transform.localPosition = Random.insideUnitCircle * 0.01f;

        }

        private bool TryGetRockNew(out Rock rock, out RaycastHit hit)
        {
            rock = null;
            hit = default;

            var ray = cam.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out hit)) return false;
            return hit.transform.TryGetComponent(out rock);
        }

        private bool TryGetRock(out Rock rock, out RaycastHit raycastHit)
        {
            rock = null;
            raycastHit = default;

            var ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = new RaycastHit[8];
            int size = Physics.RaycastNonAlloc(ray, hits);

            float closestSqrDistance = float.MaxValue;
            RaycastHit closestHit = default;
            Rock foundRock = null;

            for (int i = 0; i < size; i++)
            {
                var hit = hits[i];
                float sqrDistance = (ray.origin - hit.point).sqrMagnitude;

                if (sqrDistance < closestSqrDistance && hit.collider.TryGetComponent(out Rock tempRock))
                {
                    closestSqrDistance = sqrDistance;
                    closestHit = hit;
                    foundRock = tempRock;
                }
            }

            if (foundRock != null)
            {
                rock = foundRock;
                raycastHit = closestHit;
                return true;
            }

            return false;
        }
    }
}
