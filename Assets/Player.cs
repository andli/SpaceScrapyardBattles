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

        GameObject shipPartPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/ShipPart.prefab");

        GameObject cockpit = Instantiate<GameObject>(shipPartPrefab, new Vector3(0, -2, 0), Quaternion.identity);
        cockpit.name = "Cockpit";
        //TODO: Freeze the cockpit in place
        //cockpit.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        ShipPartDisplay shipPartDisplay = cockpit.GetComponent<ShipPartDisplay>();
        ShipPart soCockpit = AssetDatabase.LoadAssetAtPath<ShipPart>("Assets/ShipParts/Cockpit.asset");
        shipPartDisplay.shipPart = Instantiate(soCockpit);

        ship.addStartingShipPart(shipPartDisplay.shipPart);

        // Add a couple more objects
        GameObject go1 = tmpSpawnPart(shipPartPrefab, new Vector3(-3.5f, 2, 0), "Hull-4");
        GameObject go2 = tmpSpawnPart(shipPartPrefab, new Vector3(-2f, 2, 0), "Engine");
        GameObject go3 = tmpSpawnPart(shipPartPrefab, new Vector3(-0.5f, 2, 0), "Autocannon");
        GameObject go4 = tmpSpawnPart(shipPartPrefab, new Vector3(1, 2, 0), "Hull-2");
        GameObject go5 = tmpSpawnPart(shipPartPrefab, new Vector3(2.5f, 2, 0), "Lasercannon");
        GameObject go6 = tmpSpawnPart(shipPartPrefab, new Vector3(4, 2, 0), "Hull-3");
    }

    private GameObject tmpSpawnPart(GameObject shipPartPrefab, Vector3 pos, string name) { 
        GameObject go = Instantiate<GameObject>(shipPartPrefab, pos, Quaternion.identity);
        go.GetComponent<ShipPartDisplay>().shipPart = Instantiate(AssetDatabase.LoadAssetAtPath<ShipPart>($"Assets/ShipParts/{name}.asset"));
        go.name = go.GetComponent<ShipPartDisplay>().shipPart.name;

        return go;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
