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
        // Only attach to already stuck parts
        if (this.attachingTarget != null && this.isValidAttachment(this.attachingTarget))
        {
            if (GameManager.Instance.connectionTargets.Contains(this) && GameManager.Instance.connectionTargets.Contains(this.attachingTarget))
            {
                // Add the part to the Ship object
                GameManager.Instance.player.ship.addShipPart(this.shipPart, this.attachingTarget.shipPart, this.attachingDirection);

                // Stop all outline effects
                List<ShipPartDisplay> partsToRemove = new List<ShipPartDisplay>();
                foreach (ShipPartDisplay item in GameManager.Instance.connectionTargets)
                {
                    item.GetComponent<OutlineEffect>().StopOutlining();
                    partsToRemove.Add(item);
                }

                // Remove involved parts from list of connections
                GameManager.Instance.ClearConnectionTargets(partsToRemove);

                snapToTarget(this.attachingTarget);
            }
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



    private bool isValidAttachment(ShipPartDisplay collisionTarget)
    {
        // Get potential slot in the ship array
        Vector2Int potentialPosition;
        try
        {
            potentialPosition = GameManager.Instance.player.ship.getShipPartCoordinate(collisionTarget.shipPart,
            ShipPart.PositionsToDirection(
                this.gameObject.transform.position,
                collisionTarget.transform.position
                )
            );
        }
        catch (Exception ex)
        {
            Debug.Log("Invalid attachment." + ex.Message);
            return false;
        }

        // Check each side of dragged part that has anchors
        foreach (var item in this.shipPart.anchors.GetAll())
        {
            bool anchorOk = GameManager.Instance.player.ship.getMatchingAnchor(potentialPosition, item);
            if (!anchorOk)
            {
                Debug.Log($"{item.Key} anchor NOT ok at {potentialPosition}.");
                return false;
            }
        }

        Debug.Log("Valid attachment!");
        return true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ShipPartDisplay collisionTarget = collision.GetComponent<ShipPartDisplay>();

        // 1. Maintain global list of collision objects
        GameManager.Instance.AddConnectionTarget(this);

        // 2. See if dragged part has valid or no anchors on all sides
        if (this.beingDragged)
        {
            if (this.isValidAttachment(collisionTarget))
            {
                // 3. Enable outline effect
                this.GetComponent<OutlineEffect>().StartOutlining();
                collisionTarget.GetComponent<OutlineEffect>().StartOutlining();

                // 4. Save dragged part as attaching candidate if user does mouse up
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

        GameManager.Instance.connectionTargets.Remove(this);

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
