using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineEffect : MonoBehaviour
{
    [SerializeField] public Material outlineMaterial;

    private Material _outlineMaterial = null;
    private Material originalMaterial = null;

    private float outlineThickness;
    private float fadeSpeed = 8f;
    private bool isOutlined;

    private void Update()
    {
        if (isOutlined)
        {
            outlineThickness = Mathf.Clamp01(outlineThickness + fadeSpeed * Time.deltaTime);
            _outlineMaterial.SetFloat("_OutlineThickness", outlineThickness);

        }
        else
        {
            outlineThickness = Mathf.Clamp01(outlineThickness - fadeSpeed * Time.deltaTime);
            _outlineMaterial.SetFloat("_OutlineThickness", outlineThickness);

        }

    }

    private void Awake()
    {
        if (this._outlineMaterial == null)
        {
            this._outlineMaterial = Instantiate(outlineMaterial);
        }
    }

    public void StartOutlining()
    {
        if (!isOutlined)
        {
            this.originalMaterial = this.getNamedRenderer("Background").material;
            this.getNamedRenderer("Background").material = _outlineMaterial;
            isOutlined = true;
        }
    }
    public void StopOutlining()
    {
        if (isOutlined && this.originalMaterial != null)
        {
            this.getNamedRenderer("Background").material = originalMaterial;
            isOutlined = false;
        }
    }

    private SpriteRenderer getNamedRenderer(string name)
    {
        SpriteRenderer[] rs = GetComponentsInChildren<SpriteRenderer>(true);

        foreach (SpriteRenderer spriteRenderer in rs)
        {
            if (spriteRenderer.name == name)
            {
                return spriteRenderer;
            }
        }
        return null;
    }
}
