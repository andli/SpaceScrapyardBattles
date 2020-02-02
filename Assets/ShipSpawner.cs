using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Ship ship = GameManager.Instance.player.ship;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
