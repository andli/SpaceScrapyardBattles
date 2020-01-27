using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Ship ship;

    // Start is called before the first frame update
    void Start()
    {
        ship = new Ship(new Vector2Int(25, 25));

        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/ShipPart.prefab");

        GameObject cockpit = Instantiate<GameObject>(prefab, new Vector3(0,0,0), Quaternion.identity);
        cockpit.name = "Cockpit";
        //TODO: Freeze the cockpit in place
        //cockpit.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        ShipPartDisplay shipPartDisplay = cockpit.GetComponent<ShipPartDisplay>();
        ShipPart soCockpit = AssetDatabase.LoadAssetAtPath<ShipPart>("Assets/ShipParts/Cockpit.asset");
        shipPartDisplay.shipPart = Instantiate(soCockpit);

        ship.addStartingShipPart(shipPartDisplay.shipPart);

        // Add a couple more objects

        GameObject go2 = Instantiate<GameObject>(prefab, new Vector3(1, 2, 0), Quaternion.identity);
        //go2.name = "Autocannon";
        go2.GetComponent<ShipPartDisplay>().shipPart = Instantiate(AssetDatabase.LoadAssetAtPath<ShipPart>("Assets/ShipParts/Autocannon.asset"));

        GameObject go1 = Instantiate<GameObject>(prefab, new Vector3(-1, 2, 0), Quaternion.identity);
        //go1.name = "Engine";
        go1.GetComponent<ShipPartDisplay>().shipPart = Instantiate(AssetDatabase.LoadAssetAtPath<ShipPart>("Assets/ShipParts/Engine.asset"));

        GameObject go3 = Instantiate<GameObject>(prefab, new Vector3(-3, 2, 0), Quaternion.identity);
        //go3.name = "Hull-4";
        go3.GetComponent<ShipPartDisplay>().shipPart = Instantiate(AssetDatabase.LoadAssetAtPath<ShipPart>("Assets/ShipParts/Hull-4.asset"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
