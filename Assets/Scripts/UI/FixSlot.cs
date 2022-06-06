using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FixSlot : MonoBehaviour, IDropHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Item item;
    public Image icon;
    public Transform parentPanel;
    private PlayerController player;
    private Vector3 startPos;
    public bool droppedOnSlot;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null && item == null && eventData.pointerDrag.GetComponent<Slots>().item.itemType == ItemType.Weapon)
        {
            eventData.pointerDrag.GetComponent<Slots>().icon.transform.SetParent(transform);
            eventData.pointerDrag.GetComponent<Slots>().droppedOnSlot = true;
            item = eventData.pointerDrag.GetComponent<Slots>().item;
            icon = eventData.pointerDrag.GetComponent<Slots>().icon;
            eventData.pointerDrag.GetComponent<Slots>().icon.rectTransform.anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        }
    }

    public void FixItem()
    {
        if(item != null && item.weaponConditionModifier < 100f)
        {
            if(item.weaponConditionModifier >= 80)
            {
                SetFixValues(100);
            }
            else if(item.weaponConditionModifier >= 60)
            {
                SetFixValues(300);
            }
            else if (item.weaponConditionModifier >= 40)
            {
                SetFixValues(600);
            }
            else if (item.weaponConditionModifier >= 20)
            {
                SetFixValues(900);
            }
        }
        else if(item != null && item.weaponConditionModifier == 100f)
        {

        }
        else
        {
            return;
        }
    }
    private void SetFixValues(int gold)
    {
        player.stats.currentGold -= gold;
        item.weaponConditionModifier = 100;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        droppedOnSlot = false;
        icon.transform.SetParent(transform);
        startPos = icon.transform.position;
        icon.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        icon.GetComponent<CanvasGroup>().blocksRaycasts = true;

        if(!droppedOnSlot)
        {
            icon.transform.position = startPos;
        }
        ResetItemAndIcon();
    }

    public void OnDrag(PointerEventData eventData)
    {
        icon.rectTransform.anchoredPosition += eventData.delta;
    }
    public void ResetItemAndIcon()
    {
        item = null;
        icon = null;
    }
}
