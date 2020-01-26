using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipPartDisplay : MonoBehaviour
{
    public ShipPart shipPart;

    private float rotationCooldown = 0.2f;
    private float keyTimestamp;
    private bool beingDragged = false;

    private void FixedUpdate()
    {
        if (beingDragged && Input.GetKey(KeyCode.R))
        {
            if (keyTimestamp + rotationCooldown < Time.time)
            {
                this.rotate90CW();
                keyTimestamp = Time.time;
            }
        }
    }

    public void rotate90CW()
    {
        Debug.Log("rotate!");
        SpriteRenderer[] rs = GetComponentsInChildren<SpriteRenderer>(true);
        rs[1].transform.Rotate(new Vector3(0, 0, 90));

        shipPart.rotation = rs[1].transform.rotation.eulerAngles;
    }

    public void setDragging(bool isDragging)
    {
        this.beingDragged = isDragging;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OutlineEffect outlineEffect = GetComponent<OutlineEffect>();
        outlineEffect.StartOutlining();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OutlineEffect outlineEffect = GetComponent<OutlineEffect>();
        outlineEffect.StopOutlining();
    }

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer[] rs = GetComponentsInChildren<SpriteRenderer>(true);
     
        foreach (SpriteRenderer spriteRenderer in rs)
        {
            if (spriteRenderer.name == "Artwork")
            {
                spriteRenderer.sprite = shipPart.artwork;
            }
        }
    }
    void Update()
    {

    }



}
