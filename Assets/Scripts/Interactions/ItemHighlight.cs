using UnityEngine;

namespace Interactions
{
    public class ItemHighlight : MonoBehaviour
    {
        [SerializeField] private Outline[] outlines;

        public void SetShow(bool show)
        {
            foreach (var outline in outlines)
                outline.enabled = show;
        }
    }
}