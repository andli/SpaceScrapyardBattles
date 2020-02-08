using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
    public GameObject shipAssembly;

    // Start is called before the first frame update
    void Start()
    {
        /* Populate the ship assembly with components
         * Connect all components with Fixed Joint 2Ds
         */

        Ship ship = GameManager.Instance.player.ship;
        GameObject shipComponentPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/ShipComponent.prefab");

        // Spiral out around the center
        this.populateShip(shipAssembly, ship, shipComponentPrefab);

    }

    private void populateShip(GameObject shipAssembly, Ship ship, GameObject shipComponentPrefab)
    {
        List<ShipComponent> components = new List<ShipComponent>();

        int x = 0; // current position; x
        int y = 0; // current position; y
        int d = 0; // current direction; 0=RIGHT, 1=DOWN, 2=LEFT, 3=UP
        int c = 0; // counter
        int s = 1; // chain size

        // starting point
        x = ship.getCenterPoint().x;
        y = ship.getCenterPoint().y;
        int size = ship.getSize().x;

        for (int k = 1; k <= (size - 1); k++)
        {
            for (int j = 0; j < (k < (size - 1) ? 2 : 3); j++)
            {
                for (int i = 0; i < s; i++)
                {
                    // Create the component
                    Vector2Int arrayPos = new Vector2Int(x, y);
                    if (ship.positionOccupied(arrayPos))
                    {
                        GameObject tmpGo = this.tmpSpawnPart(ship, shipComponentPrefab, arrayPos);
                        tmpGo.transform.SetParent(shipAssembly.transform);
                        components.Add(tmpGo.GetComponent<ShipComponent>());
                        Debug.Log(arrayPos);
                    }

                    c++;

                    switch (d)
                    {
                        case 0: y = y + 1; break;
                        case 1: x = x + 1; break;
                        case 2: y = y - 1; break;
                        case 3: x = x - 1; break;
                    }
                }
                d = (d + 1) % 4;
            }
            s = s + 1;
        }

        // Look at all placed components
        foreach (ShipComponent sc in components)
        {
            // Loop possible neighbours
            foreach (ShipPart neighbour in sc.GetComponent<ShipComponent>().shipPart.connectedTo)
            {
                if (!neighbour.Equals(sc)) //TODO: check for duplicate joints
                {
                    // Add a joint between tmpGo and the already created ShipComponent sc
                    FixedJoint2D joint = sc.gameObject.AddComponent<FixedJoint2D>();
                    ShipComponent target = components.Find(comp => comp.shipPart.Equals(neighbour));
                    joint.connectedBody = target.GetComponent<Rigidbody2D>();

                }
                
            }
        }

        // Set the steering rigidbody
        shipAssembly.GetComponent<ShipMovement>().rb = components.Find(comp => comp.name == "Cockpit").GetComponent<Rigidbody2D>();
    }

    private GameObject tmpSpawnPart(Ship ship, GameObject shipComponentPrefab, Vector2Int arrayPos)
    {
        ShipPart part = ship.getPartAtPosition(arrayPos);
        // TODO: Add a proper coordinate so the parts spawn sensibly
        float positionScale = 0.75f;
        GameObject go = Instantiate<GameObject>(shipComponentPrefab,
            new Vector3(
                (arrayPos.x - ship.getCenterPoint().x) * positionScale,
                (arrayPos.y - ship.getCenterPoint().y) * positionScale,
                0
                ), Quaternion.identity);
        go.GetComponent<ShipComponent>().shipPart = part;
        go.GetComponent<SpriteRenderer>().sprite = part.artwork;
        go.name = go.GetComponent<ShipComponent>().shipPart.name;

        return go;
    }

}
