﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipPartDisplay : MonoBehaviour
{
    public ShipPart shipPart;

    private float rotationCooldown = 0.2f;
    private float keyTimestamp;
    private bool beingDragged = false;
    private bool inAttachRange = false;

    private Direction attachingDirection { get; set; }
    private ShipPartDisplay attachingTarget { get; set; }

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
        SpriteRenderer[] rs = GetComponentsInChildren<SpriteRenderer>(true);
        rs[1].transform.Rotate(new Vector3(0, 0, -90));

        shipPart.setRotation(rs[1].transform.rotation.eulerAngles);
    }

    public void attachToPredefinedTarget()
    {
        if (inAttachRange && this.attachingTarget != null)
        {
            // Add the part to the Ship object
            GameManager.Instance.player.ship.addShipPart(this.shipPart, this.attachingTarget.shipPart, this.attachingDirection);

            // Stop all outline effects
            this.attachingTarget.GetComponent<OutlineEffect>().StopOutlining();
            GetComponent<OutlineEffect>().StopOutlining();
            this.inAttachRange = false;
        }
    }

    public void setDragging(bool isDragging)
    {
        this.beingDragged = isDragging;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ShipPartDisplay collisionTarget = collision.GetComponent<ShipPartDisplay>();

        if (this.beingDragged || collisionTarget.beingDragged)
        {
            if (collisionTarget.inAttachRange)
            {
                GetComponent<OutlineEffect>().StartOutlining();
            }
            // Only attach a dragged ship part
            if (this.beingDragged)
            {
                // 
                this.inAttachRange = true;

                // Save collision details to use when attaching after mouse up
                this.attachingDirection = ShipPart.PositionsToDirection(
                    this.gameObject.transform.position,
                    collision.transform.position
                    );
                this.attachingTarget = collisionTarget;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GetComponent<OutlineEffect>().StopOutlining();
        this.inAttachRange = false;
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
