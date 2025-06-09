using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush : MonoBehaviour
{
    [SerializeField] private Texture2D dirtyMaskBase;
    [SerializeField] private Texture2D brushTexture;
    [SerializeField] private Material objectMaterial;

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
                lastPaintPixelPosition = paintPixelPosition;

                int pixelXOffset = pixelX - (brushTexture.width / 2);
                int pixelYOffset = pixelY - (brushTexture.height / 2);

                for (int x = 0; x < brushTexture.width; x++)
                {
                    for (int y = 0; y < brushTexture.height; y++)
                    {
                        Color pixelDirt = brushTexture.GetPixel(x, y);
                        Color pixelDirtMask = dirtyMask.GetPixel(pixelXOffset + x, pixelYOffset + y);

                        float removedAmount = pixelDirtMask.g - (pixelDirtMask.g * pixelDirt.g);
                        dirtAmount -= removedAmount;

                        dirtyMask.SetPixel(
                            pixelXOffset + x,
                            pixelYOffset + y,
                            new Color(0, pixelDirtMask.g * pixelDirt.g, 0)
                        );
                        Debug.Log("Dirt amount : " + dirtAmount);
                    }
                }

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
