using UnityEngine;

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
            GetComponent<ShipPartDisplay>().BeingDragged = true;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }
    }

    void OnMouseDrag()
    {
        if (GetComponent<ShipPartDisplay>().BeingDragged)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

            GetComponent<Rigidbody2D>().MovePosition(curPosition);
        }
    }

    void OnMouseUp()
    {
        if (GetComponent<ShipPartDisplay>().BeingDragged)
        {
            ShipPartDisplay shipPartDisplay = GetComponent<ShipPartDisplay>();
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            shipPartDisplay.BeingDragged = false;

            // Try to attach
            shipPartDisplay.attachToPredefinedTarget();
        }
    }
}
