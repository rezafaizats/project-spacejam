using UnityEngine;
using UnityEngine.Serialization;

namespace Game_Mechanics
{
    public class BrushController : MonoBehaviour, IToolController
    {
        private const string ID = "brush";
        
        [SerializeField] private Texture2D dirtyMaskBase;
        [SerializeField] private Texture2D brushTexture;
        [SerializeField] private Material objectMaterial;
        [SerializeField] private float brushScale = 1f;
        [SerializeField] private GameObject tableBrushObject;

        private Texture2D dirtyMask;
        private bool isBrushActive;
        private float dirtAmountTotal;
        private float dirtAmount;
        private Vector2Int lastPaintPixelPosition;
        private Camera cam;

        public string ToolId => ID;
        public void SetIsEquipped(bool isEquipped)
        {
            isBrushActive = isEquipped;
            tableBrushObject.SetActive(!isBrushActive);
        }

        void Start()
        {
            cam = Camera.main;
            dirtyMask = new Texture2D(dirtyMaskBase.width, dirtyMaskBase.height);
            dirtyMask.SetPixels(dirtyMaskBase.GetPixels());
            dirtyMask.Apply();

            objectMaterial.SetTexture("_DirtyMask", dirtyMask);
        
            dirtAmountTotal = 0f;
            for (int x = 0; x < dirtyMaskBase.width; x++) {
                for (int y = 0; y < dirtyMaskBase.height; y++) {
                    dirtAmountTotal += dirtyMaskBase.GetPixel(x, y).g;
                }
            }
            dirtAmount = dirtAmountTotal;
        }

        void Update()
        {
            if (!isBrushActive) return;
            if (!Input.GetMouseButton(0)) return;
            if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit)) return;
            //Debug.Log(hit.transform.name + " " + hit.textureCoord);
            Vector2 textureCoord = hit.textureCoord;

            int pixelX = (int)(textureCoord.x * dirtyMask.width);
            int pixelY = (int)(textureCoord.y * dirtyMask.height);

            Vector2Int paintPixelPosition = new Vector2Int(pixelX, pixelY);
            // Debug.Log("UV : " + textureCoord + " Pixels : " + paintPixelPosition);
            int paintPixelDistance = Mathf.Abs(paintPixelPosition.x - lastPaintPixelPosition.x) + Mathf.Abs(paintPixelPosition.y - lastPaintPixelPosition.y);
            int maxPaintDistance = 7;
            if (paintPixelDistance < maxPaintDistance) {
                // Painting too close to last position
                return;
            }
                
            lastPaintPixelPosition = paintPixelPosition;

            int scaledBrushWidth = Mathf.RoundToInt(brushTexture.width * brushScale);
            int scaledBrushHeight = Mathf.RoundToInt(brushTexture.height * brushScale);
            int pixelXOffset = Mathf.Clamp(pixelX - scaledBrushWidth / 2, 0, dirtyMask.width);
            int pixelYOffset = Mathf.Clamp(pixelY - scaledBrushHeight / 2, 0, dirtyMask.height);

            int clampedWidth = Mathf.Min(scaledBrushWidth, dirtyMask.width - pixelXOffset);
            int clampedHeight = Mathf.Min(scaledBrushHeight, dirtyMask.height - pixelYOffset);

            if (clampedWidth <= 0 || clampedHeight <= 0)
                return;

            Color[] maskPixels = dirtyMask.GetPixels(pixelXOffset, pixelYOffset, clampedWidth, clampedHeight);

            for (int x = 0; x < clampedWidth; x++) {
                for (int y = 0; y < clampedHeight; y++) {
                    float u = x / (float)clampedWidth;
                    float v = y / (float)clampedHeight;

                    Color pixelDirt = brushTexture.GetPixelBilinear(u, v);
                    int index = y * clampedWidth + x;
                    Color pixelDirtMask = maskPixels[index];

                    float removedAmount = pixelDirtMask.g - (pixelDirtMask.g * pixelDirt.g);
                    dirtAmount -= removedAmount;

                    maskPixels[index] = new Color(0, pixelDirtMask.g * pixelDirt.g, 0);
                }
            }

            dirtyMask.SetPixels(pixelXOffset, pixelYOffset, clampedWidth, clampedHeight, maskPixels);
            dirtyMask.Apply();
            Debug.Log("Dirt amount : " + dirtAmount);
        }
    }
}
