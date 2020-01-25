using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropBehavior : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    [SerializeField] private Canvas canvas = null;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private float rotationCooldown = 0.2f;
    private float timestamp;
    private GameObject draggedObject = null;

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.R))
        {
            if (timestamp + rotationCooldown < Time.time)
            {
                try
                {
                    ShipPartDisplay shipPartDisplay = draggedObject.GetComponent<ShipPartDisplay>();
                    if (shipPartDisplay != null)
                    {
                        shipPartDisplay.rotate90CW();

                    }
                    timestamp = Time.time;
                }
                catch (System.Exception)
                {

                }

            }
        }

    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBeginDrag");
        //canvasGroup.alpha = .6f;
        this.draggedObject = eventData.pointerDrag;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("OnEndDrag");
        //canvasGroup.alpha = 1f;
        this.draggedObject = null;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("OnPointerDown");
    }


}
