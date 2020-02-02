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
    private ShipPartDisplay lastCollisionTarget;

    private Direction attachingDirection { get; set; }
    private ShipPartDisplay attachingTarget { get; set; }
    public bool BeingDragged { get => beingDragged; set => beingDragged = value; }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.lastCollisionTarget = collision.GetComponent<ShipPartDisplay>();

        attachIfPossible(this.lastCollisionTarget);
    }

    private void attachIfPossible(ShipPartDisplay target)
    {
        // 1. Maintain global list of collision objects
        GameManager.Instance.AddConnectionTarget(this);
        GameManager.Instance.AddConnectionTarget(target);

        // 2. See if dragged part has valid or no anchors on all sides
        if (this.beingDragged)
        {
            if (this.isValidAttachment(target))
            {
                // 3. Save dragged part as attaching candidate if user does mouse up
                this.attachingDirection = ShipPart.PositionsToDirection(
                    this.gameObject.transform.position,
                    target.transform.position
                    );
                this.attachingTarget = target;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GetComponent<OutlineEffect>().StopOutlining();
        GameManager.Instance.connectionTargets.Remove(this);
        this.attachingTarget = null;
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
                //GameManager.Instance.ClearAllConnectionTargets();
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

    private bool isValidAttachment(ShipPartDisplay target)
    {
        // Get potential slot in the ship array
        Vector2Int potentialPosition;
        Direction targetDirection = ShipPart.PositionsToDirection(
                this.gameObject.transform.position,
                target.transform.position
                );

        try
        {
            potentialPosition = GameManager.Instance.player.ship.getShipPartCoordinate(target.shipPart, targetDirection);
        }
        catch (Exception ex)
        {
            Debug.Log("Invalid attachment." + ex.Message);
            return false;
        }

        if (GameManager.Instance.player.ship.positionOccupied(potentialPosition))
        {
            return false;
        }


        // Check valid placement
        // Check each side of dragged part that has anchors
        bool allSourceAnchorsMatch = true;
        foreach (var anchor in this.shipPart.anchors.GetAll())
        {
            // If there is a part on the other side of this anchor, it has to have an anchor too.
            (bool, bool) neighborAnchor = GameManager.Instance.player.ship.getNeighbourExistsAndAnchor(potentialPosition, anchor.Key);
            // If there is an anchor
            if (anchor.Value)
            {
                //  AND a neighbor part
                if (neighborAnchor.Item1)
                {
                    // If the neighbor part has no corresponding anchor
                    if (!neighborAnchor.Item2)
                    {
                        allSourceAnchorsMatch = false;
                    }
                }
            }
            // If the neighbor has an anchor
            if (neighborAnchor.Item1 && neighborAnchor.Item2)
            {
                // Then we must also have one
                if (!anchor.Value)
                {
                    allSourceAnchorsMatch = false;
                }
            }
            //Debug.Log($"{anchor} = {neighborAnchor}");
        }

        // Check valid connection to main ship
        if (allSourceAnchorsMatch)
        {
            bool validTargetAnchor = target.shipPart.getAnchorTowardsPosition(potentialPosition);
            bool validSourceAnchor = this.shipPart.getAnchorInDirection(Directions.Reverse(targetDirection));
            if (validTargetAnchor && validSourceAnchor)
            {
                if (this.beingDragged)
                {
                    this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                }
                this.GetComponent<OutlineEffect>().StartOutlining();
                target.GetComponent<OutlineEffect>().StartOutlining();
                Debug.Log("Valid attachment!");
                return true;
            }

        }
        return false;
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

    private void FixedUpdate()
    {
        // Throttle rotation keystrokes
        if (beingDragged && Input.GetKey(KeyCode.R))
        {
            if (keyTimestamp + rotationCooldown < Time.time)
            {
                this.rotate90CW();
                keyTimestamp = Time.time;
            }
        }

        if (beingDragged && Input.GetKey(KeyCode.E))
        {
            if (keyTimestamp + rotationCooldown < Time.time)
            {
                this.rotate90CCW();
                keyTimestamp = Time.time;
            }
        }
    }

    public void rotate90CW()
    {
        // Rotate the artwork sprite
        SpriteRenderer[] rs = GetComponentsInChildren<SpriteRenderer>(true);
        rs[1].transform.Rotate(new Vector3(0, 0, -90));

        // Set the direction angle on the ship part
        shipPart.setRotation(rs[1].transform.rotation.eulerAngles);

        // Simulate a OnTriggerExit2D when rotating
        foreach (ShipPartDisplay item in GameManager.Instance.connectionTargets)
        {
            item.GetComponent<OutlineEffect>().StopOutlining();
        }
        GameManager.Instance.ClearAllConnectionTargets();
        this.attachingTarget = null;

        // Simulate a OnTriggerEnter2D when rotating
        if (this.lastCollisionTarget != null)
        {
            attachIfPossible(this.lastCollisionTarget);
        }
    }

    public void rotate90CCW()
    {
        // Rotate the artwork sprite
        SpriteRenderer[] rs = GetComponentsInChildren<SpriteRenderer>(true);
        rs[1].transform.Rotate(new Vector3(0, 0, 90));

        // Set the direction angle on the ship part
        shipPart.setRotation(rs[1].transform.rotation.eulerAngles);

        // Simulate a OnTriggerExit2D when rotating
        foreach (ShipPartDisplay item in GameManager.Instance.connectionTargets)
        {
            item.GetComponent<OutlineEffect>().StopOutlining();
        }
        GameManager.Instance.ClearAllConnectionTargets();
        this.attachingTarget = null;

        // Simulate a OnTriggerEnter2D when rotating
        if (this.lastCollisionTarget != null)
        {
            attachIfPossible(this.lastCollisionTarget);
        }
    }

}
