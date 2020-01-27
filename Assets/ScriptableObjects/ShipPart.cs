using UnityEngine;
using System;

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
}

public enum Direction : ushort
{
    None = 0,
    North = 1,
    East = 2,
    South = 3,
    West = 4,
}


[CreateAssetMenu(fileName = "New ship part", menuName = "ShipPart")]
public class ShipPart : ScriptableObject
{
    new public string name = "New item";

    public Sprite artwork;

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
        Vector3 sum = pos1 - pos2;
        sum = sum.normalized;

        if (sum.y > 0.9 && sum.x >= -0.5 && sum.x <= 0.5)
        {
            return Direction.North;
        }
        else if (sum.x > 0.9 && sum.y >= -0.5 && sum.y <= 0.5)
        {
            return Direction.East;
        }
        else if (sum.y < -0.9 && sum.x >= -0.5 && sum.x <= 0.5)
        {
            return Direction.South;
        }
        else if (sum.x < -0.9 && sum.y >= -0.5 && sum.y <= 0.5)
        {
            return Direction.West;
        }
        else
        {
            throw new System.Exception("Error calculating direction.");
        }
    }

    public override string ToString()
    {
        return $"{this.name}, [{this.pos}], {this.rotation}, {this.direction}";
    }
}
