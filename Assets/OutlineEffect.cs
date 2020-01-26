using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineEffect : MonoBehaviour
{
    [SerializeField] private Material material;

    private float outlineThickness;
    private float fadeSpeed = 1f;
    private bool isOutlined;

    private void Update()
    {
        if (isOutlined)
        {
            outlineThickness = Mathf.Clamp01(outlineThickness + fadeSpeed * Time.deltaTime);
            material.SetFloat("_OutlineThickness", outlineThickness);

        }
        else
        {
            outlineThickness = Mathf.Clamp01(outlineThickness - fadeSpeed * Time.deltaTime);
            material.SetFloat("_OutlineThickness", outlineThickness);

        }

    }

    public void StartOutlining()
    {
        Debug.Log("Outline start");
        if (!isOutlined)
        {
            isOutlined = true;

        }
    }
    public void StopOutlining()
    {
        Debug.Log("Outline STOP");
        if (isOutlined)
        {
            isOutlined = false;

        }
    }
}
