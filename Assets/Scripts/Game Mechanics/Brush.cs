using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush : MonoBehaviour
{
    [SerializeField] private Texture2D dirtyMaskBase;
    [SerializeField] private Texture2D brushTexture;
    [SerializeField] private Material objectMaterial;
    [SerializeField] private float brushScale = 1f;
    
    private Texture2D dirtyMask;
    private bool isBrushActive;
    private float dirtAmountTotal;
    private float dirtAmount;
    private Vector2Int lastPaintPixelPosition;

    // Start is called before the first frame update
    void Start()
    {
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0)) {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                Debug.Log(hit.transform.name + " " + hit.textureCoord);
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

    void OnMouseDown()
    {

    }

    public void ActivateBrush()
    {
        isBrushActive = true;
    }

    public void DeactivateBrush()
    {
        isBrushActive = false;
    }

}
