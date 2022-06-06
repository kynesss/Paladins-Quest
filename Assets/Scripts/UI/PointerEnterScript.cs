using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerEnterScript : MonoBehaviour, IPointerEnterHandler
{
    public FixSlot fixSlot;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && fixSlot.item == null && eventData.pointerDrag.GetComponent<Slots>().item.itemType == ItemType.Weapon)
        {
            eventData.pointerDrag.GetComponent<Slots>().icon.transform.SetParent(transform);
        }
    }
}
