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

    public Vector2Int getCenterPoint()
    {
        return new Vector2Int(this.centerX, this.centerY);
    }

    public Vector2Int getSize()
    {
        return new Vector2Int(parts.GetLength(0), parts.GetLength(1));
    }

    public Vector2Int getShipPartCoordinate(ShipPart existingPart, Direction targetSide)
    {
        if (existingPart.pos.Equals(Vector2Int.zero))
        {
            throw new Exception("not a valid target shippart.");
        }

        Vector2Int offset = new Vector2Int(0, 0);
        switch (targetSide)
        {
            case Direction.North:
                offset = Vector2Int.up;
                break;
            case Direction.East:
                offset = Vector2Int.right;
                break;
            case Direction.South:
                offset = Vector2Int.down;
                break;
            case Direction.West:
                offset = Vector2Int.left;
                break;
            default:
                throw new Exception("not a valid direction");
        }

        return existingPart.pos + offset;
    }

    public ShipPart getPartAtPosition(Vector2Int pos)
    {
        return parts[pos.x, pos.y];

    }

    public List<Vector2Int> getNeighbourPositions(Vector2Int pos)
    {
        List<Vector2Int> neighbours = new List<Vector2Int>();

        for (int i = pos.x - 1; i <= pos.x + 1; i++)
        {
            for (int j = pos.y - 1; j <= pos.y + 1; j++)
            {
                if (!new Vector2Int(i,j).Equals(pos))
                {
                    if (this.parts[i,j] != null)
                    {
                        neighbours.Add(this.parts[i, j].pos);
                    }
                }
            }
        }
        return neighbours;
    }

    public (bool,bool) getNeighbourExistsAndAnchor(Vector2Int potentialPosition, Direction sourceDirection)
    {
        Vector2Int checkVector = Directions.directionToVector(sourceDirection);

        ShipPart part = this.getPartAtPosition(potentialPosition + checkVector);
        // No part means we don't have to match in that direction
        if (part == null) return (false, false);

        Direction reverseDirection = Directions.vectorToDirection(-checkVector);
        bool targetAnchorValue = part.getAnchorInDirection(reverseDirection);

        return (true, targetAnchorValue);
    }

    public void addShipPart(ShipPart newPart, ShipPart existingPart, Direction targetSide)
    {
        newPart.pos = this.getShipPartCoordinate(existingPart, targetSide);
        newPart.isAttached = true;

        parts[newPart.pos.x, newPart.pos.y] = newPart;
        existingPart.AddConnection(newPart);
    }

    public void addStartingShipPart(ShipPart newPart)
    {
        newPart.pos = new Vector2Int(this.centerX, this.centerY);
        newPart.isAttached = true;
        parts[this.centerX, this.centerY] = newPart;

    }

    public bool positionOccupied(Vector2Int pos)
    {
        if (pos.x >= 0 && pos.x < this.parts.GetLength(0))
        {
            if (pos.y >= 0 && pos.y < this.parts.GetLength(1))
            {
                return parts[pos.x, pos.y] != null;
            }
        }
        return false;
    }

}
