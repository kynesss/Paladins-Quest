using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopSlots : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Item item;
    public Image image;

    public void SetSlot(Item newItem)
    {
        item = newItem;
        image.sprite = item.icon;
    }
    public void ClearSlot()
    {
        item = null;
        image.sprite = null;
        TradeController.instance.chosenShopSlot = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TradeController.instance.DisplayItemInfo(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TradeController.instance.ResetItemInfo();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.clickCount == 2 && item != null)
        {
            TradeController.instance.OpenTradeConfirmPanel(item);
            TradeController.instance.GetItemFromSlot(item);
            TradeController.instance.ChooseShopSlot(this);
        }
    }
}
