using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Click");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Drag");
        rectTransform.anchoredPosition += eventData.delta;
    }
}
