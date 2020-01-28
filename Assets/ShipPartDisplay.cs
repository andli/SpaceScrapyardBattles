using System;
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
        if (this.attachingTarget != null && GameManager.Instance.sourceConnector != null && GameManager.Instance.targetConnector != null)
        {
            // Add the part to the Ship object
            GameManager.Instance.player.ship.addShipPart(this.shipPart, this.attachingTarget.shipPart, this.attachingDirection);

            // Stop all outline effects
            GameManager.Instance.sourceConnector.GetComponent<OutlineEffect>().StopOutlining();
            GameManager.Instance.targetConnector.GetComponent<OutlineEffect>().StopOutlining();

            GameManager.Instance.sourceConnector = null;
            GameManager.Instance.targetConnector = null;

            snapToTarget(this.attachingTarget);
        }
    }

    private void snapToTarget(ShipPartDisplay target)
    {
        Vector3 delta = this.transform.position - target.transform.position;

        float movedX = (float)Math.Round(Math.Abs(delta.x)) * 0.9f;
        float movedY = (float)Math.Round(Math.Abs(delta.y)) * 0.9f;

        if (delta.x < 0)
        {
            movedX = -movedX;
        }
        if (delta.y < 0)
        {
            movedY = -movedY;
        }

        this.transform.position = target.transform.position + new Vector3(movedX, movedY, 0);

    }

    public void setDragging(bool isDragging)
    {
        this.beingDragged = isDragging;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ShipPartDisplay collisionTarget = collision.GetComponent<ShipPartDisplay>();

        if (this.beingDragged)
        {
            if (GameManager.Instance.sourceConnector == null && GameManager.Instance.targetConnector == null)
            {
                GameManager.Instance.sourceConnector = this;
                GameManager.Instance.targetConnector = collisionTarget;

                this.GetComponent<OutlineEffect>().StartOutlining();
                collisionTarget.GetComponent<OutlineEffect>().StartOutlining();

                // Only attach a dragged ship part

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

        GameManager.Instance.sourceConnector = null;
        GameManager.Instance.targetConnector = null;
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
