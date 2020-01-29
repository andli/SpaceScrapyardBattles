using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public struct Directions
{
    public bool north;
    public bool east;
    public bool south;
    public bool west;

    public bool Empty()
    {
        return north || east || south || west;
    }

    public Dictionary<Direction, bool> GetAll()
    {
        Dictionary<Direction, bool> response = new Dictionary<Direction, bool>
        {
            { Direction.North, this.north },
            { Direction.East, this.east },
            { Direction.South, this.south },
            { Direction.West, this.west }
        };

        return response;
    }

    public void RotateAnchors90CW()
    {
        bool originalNorth = this.north;

        this.north = this.west;
        this.west = this.south;
        this.south = this.east;
        this.east = originalNorth;
    }

    public static Vector2Int directionToVector(Direction dir)
    {
        switch (dir)
        {
            case Direction.North:
                return Vector2Int.up;
            case Direction.East:
                return Vector2Int.right;
            case Direction.South:
                return Vector2Int.down;
            case Direction.West:
                return Vector2Int.left;
            case Direction.None:
            default:
                throw new Exception("Unable to parse vector from direction.");
        }
    }

    public static Direction vectorToDirection(Vector2Int v)
    {
        if (v.Equals(Vector2Int.up))
        {
            return Direction.North;
        }
        if (v.Equals(Vector2Int.right))
        {
            return Direction.East;
        }
        if (v.Equals(Vector2Int.down))
        {
            return Direction.South;
        }
        if (v.Equals(Vector2Int.left))
        {
            return Direction.West;
        }
        else
        {
            throw new Exception("Unable to parse direction from vector.");
        }
    }
}


public enum Direction : ushort
{
    None,
    North,
    East,
    South,
    West,
}


[CreateAssetMenu(fileName = "New ship part", menuName = "ShipPart")]
public class ShipPart : ScriptableObject
{
    new public string name = "New item";

    public Sprite artwork;

    internal bool getAnchorInDirection(Direction dir)
    {
        if (dir == Direction.North)
        {
            return this.anchors.north;
        }
        else if (dir == Direction.East)
        {
            return this.anchors.east;
        }
        else if (dir == Direction.South)
        {
            return this.anchors.south;
        }
        else if (dir == Direction.West)
        {
            return this.anchors.west;
        }
        else
        {
            throw new Exception("Invalid direction.");
        }
    }

    public bool isStartingComponent = false;

    public Vector2Int pos { get; set; }
    public Vector3 rotation { get; private set; }
    public Direction direction = Direction.None;

    [SerializeField]
    public Directions anchors = new Directions();

    public bool isAttached = false;

    internal void setRotation(Vector3 eulerAngles)
    {
        this.rotation = eulerAngles;
        this.direction = RotationToDirection(eulerAngles);
        this.anchors.RotateAnchors90CW();
    }

    public static Direction RotationToDirection(Vector3 rotation)
    {
        switch (rotation.z)
        {
            case 0:
                return Direction.North;
            case 90:
                return Direction.West;
            case 180:
                return Direction.South;
            case 270:
                return Direction.East;
            default:
                throw new Exception("Error calculating direction from rotation angle.");
        }
    }

    public static Direction PositionsToDirection(Vector3 pos1, Vector3 pos2)
    {
        float innerLimit = 0.4f;
        float outerLimit = 0.95f;
        Vector3 sum = pos1 - pos2;
        sum = sum.normalized;

        if (sum.y > outerLimit && sum.x >= -innerLimit && sum.x <= innerLimit)
        {
            return Direction.North;
        }
        else if (sum.x > outerLimit && sum.y >= -innerLimit && sum.y <= innerLimit)
        {
            return Direction.East;
        }
        else if (sum.y < -outerLimit && sum.x >= -innerLimit && sum.x <= innerLimit)
        {
            return Direction.South;
        }
        else if (sum.x < -outerLimit && sum.y >= -innerLimit && sum.y <= innerLimit)
        {
            return Direction.West;
        }
        else
        {
            return Direction.None;
            //throw new System.Exception("Error calculating direction.");
        }
    }

    public override string ToString()
    {
        return $"{this.name}, [{this.pos}], {this.rotation}, {this.direction}";
    }
}
