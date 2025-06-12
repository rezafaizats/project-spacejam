using DG.Tweening;
using UnityEngine;

namespace Game_Mechanics
{
    public class Rock : MonoBehaviour
    {
        [SerializeField] private Transform visualRoot;
        [SerializeField] private float baseHealth = 1.5f;
        [SerializeField] private float minScale = 0.4f;
        
        private float health, maxHealth;
        private Vector3 initScale;

        public void Initialize()
        {
            var s = initScale = transform.localScale;
            var healthScale = (s.x + s.y + s.z) / 3f;
            maxHealth = health = baseHealth * healthScale;
        }
        
        public void Damage(float damage)
        {
            var healthAlpha = health / maxHealth;
            var healthLeftAlpha = 1f - healthAlpha;
            visualRoot.localPosition = GetRandomShakePosition(healthLeftAlpha);
            health -= damage;

            transform.localScale = initScale * Mathf.Lerp(minScale, 1f, healthAlpha);

            if (health > 0f) return;
            DestroyInternal();
        }

        private static Vector3 GetRandomShakePosition(float healthLeftAlpha)
        {
            var pos = Random.insideUnitCircle * (0.02f * healthLeftAlpha);
            return new Vector3(pos.x, 0f, pos.y);
        }

        private void DestroyInternal()
        {
            Destroy(gameObject);
        }
    }
}
