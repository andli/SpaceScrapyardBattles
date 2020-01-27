using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship
{
    private ShipPart[,] parts;
    private int centerX;
    private int centerY;

    public Ship(Vector2Int size)
    {
        if (size.x % 2 != 0 && size.y % 2 != 0)
        {
            parts = new ShipPart[size.x, size.y];

            centerX = (int)Math.Ceiling((double)size.x / 2);
            centerY = (int)Math.Ceiling((double)size.y / 2);

            // The ship is created without a starting part.
        }
        else
        {
            throw new System.Exception("Ship dimensions must be odd.");
        }
    }

    public void addShipPart(ShipPart newPart, ShipPart existingPart, Direction attachingDirection)
    {
        int xOffset = 0;
        int yOffset = 0;
        switch (attachingDirection)
        {
            case Direction.North:
                yOffset = 1;
                break;
            case Direction.East:
                xOffset = 1;
                break;
            case Direction.South:
                yOffset = -1;
                break;
            case Direction.West:
                xOffset = -1;
                break;
            default:
                break;
        }

        newPart.isAttached = true;

        newPart.pos = new Vector2Int(existingPart.pos.x + xOffset, existingPart.pos.y + yOffset);
        parts[newPart.pos.x, newPart.pos.y] = newPart;

        // Print all items in the ship
        foreach (ShipPart item in parts)
        {
            if (item != null)
            {
                Debug.Log(item);

            }

        }
    }

    public void addStartingShipPart(ShipPart newPart)
    {
        newPart.pos = new Vector2Int(this.centerX, this.centerY);
        newPart.isAttached = true;
        parts[this.centerX, this.centerY] = newPart;

        //TODO: error handling etc
    }

}
