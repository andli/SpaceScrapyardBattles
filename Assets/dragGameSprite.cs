﻿using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class dragGameSprite : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;

    void OnMouseDown()
    {
        if (!GetComponent<ShipPartDisplay>().shipPart.isAttached)
        {
            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            GetComponent<ShipPartDisplay>().setDragging(true);
        }
    }

    void OnMouseDrag()
    {
        if (!GetComponent<ShipPartDisplay>().shipPart.isAttached)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            transform.position = curPosition;
        }
    }

    void OnMouseUp()
    {
        if (!GetComponent<ShipPartDisplay>().shipPart.isAttached)
        {
            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            GetComponent<ShipPartDisplay>().setDragging(false);

            ShipPartDisplay shipPartDisplay = GetComponent<ShipPartDisplay>();
            //TODO: Only attach to closest part
            GetComponent<ShipPartDisplay>().attach(shipPartDisplay.shipPart);
        }
    }
}
