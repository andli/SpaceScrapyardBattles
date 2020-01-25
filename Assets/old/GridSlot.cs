using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridSlot : MonoBehaviour, IDropHandler
{
    public GameObject shipPart
    {
        get
        {
            if (transform.childCount > 0)
            {
                return this.GetComponent<GameObject>();
            }
            return null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!shipPart)
        {
            if (eventData.pointerDrag != null)
            {
                Debug.Log("Dropped in: " + gameObject.name);

                RectTransform transformDragged = eventData.pointerDrag.GetComponent<RectTransform>();
                RectTransform transformTargetSlot = GetComponent<RectTransform>();

                transformDragged.SetParent(transformTargetSlot, true);
                Debug.Log("### old: " + transformDragged.anchoredPosition + ", new: " + transformTargetSlot.anchoredPosition);
                Debug.Log("### old: " + transformDragged.localPosition + ", new: " + transformTargetSlot.localPosition);
                transformDragged.anchoredPosition = transformTargetSlot.anchoredPosition;

                ShipGrid sg = this.GetComponentInParent<ShipGrid>();
                //Debug.Log(sg.transform.childCount);

            }
        }
    }

}
