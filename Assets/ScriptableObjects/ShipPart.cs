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


[CreateAssetMenu(fileName = "New ship part", menuName = "ShipPart")]
public class ShipPart : ScriptableObject
{
    new public string name = "New item";

    public Sprite artwork;

    public bool isStartingComponent = false;

    public Vector3 rotation = new Vector3(0, 0);

    [SerializeField]
    public Directions anchors = new Directions();
    [SerializeField]
    public Directions attachments = new Directions();

    public bool isAttached()
    {
        return !attachments.Empty();
    }
}
