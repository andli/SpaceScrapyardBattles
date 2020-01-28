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
            GetComponent<ShipPartDisplay>().setDragging(true);
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
    }

    void OnMouseDrag()
    {
        if (!GetComponent<ShipPartDisplay>().shipPart.isAttached)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

            GetComponent<Rigidbody2D>().MovePosition(curPosition);
        }
    }

    void OnMouseUp()
    {
        ShipPartDisplay shipPartDisplay = GetComponent<ShipPartDisplay>();
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        if (!shipPartDisplay.shipPart.isAttached)
        {
            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            shipPartDisplay.setDragging(false);

            shipPartDisplay.attachToPredefinedTarget();
        }
    }
}
