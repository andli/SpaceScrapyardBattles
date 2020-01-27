using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineEffect : MonoBehaviour
{
    [SerializeField] private Material outlineMaterial;
    private Material originalMaterial;

    private float outlineThickness;
    private float fadeSpeed = 8f;
    private bool isOutlined;

    private void Update()
    {
        if (isOutlined)
        {
            outlineThickness = Mathf.Clamp01(outlineThickness + fadeSpeed * Time.deltaTime);
            outlineMaterial.SetFloat("_OutlineThickness", outlineThickness);

        }
        else
        {
            outlineThickness = Mathf.Clamp01(outlineThickness - fadeSpeed * Time.deltaTime);
            outlineMaterial.SetFloat("_OutlineThickness", outlineThickness);

        }

    }

    public void StartOutlining()
    {
        if (!isOutlined)
        {
            isOutlined = true;
            this.setMaterialOnNamedRenderer("Background", outlineMaterial);
        }
    }
    public void StopOutlining()
    {
        if (isOutlined)
        {
            isOutlined = false;
            this.setMaterialOnNamedRenderer("Background", originalMaterial);
        }
    }

    private void setMaterialOnNamedRenderer(string name, Material material)
    {
        SpriteRenderer[] rs = GetComponentsInChildren<SpriteRenderer>(true);

        foreach (SpriteRenderer spriteRenderer in rs)
        {
            if (spriteRenderer.name == name)
            {
                originalMaterial = spriteRenderer.material;
                spriteRenderer.material = material;
            }
        }
    }
}
