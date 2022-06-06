using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DropItemClick : MonoBehaviour, IPointerClickHandler
{
    
    public delegate void ClickAction();
    public ClickAction dropClick;

    public delegate void ShieldAction();
    public ShieldAction shieldAction;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if (dropClick != null)
                dropClick.Invoke();

            if(shieldAction != null)
                shieldAction.Invoke();
        }
    }
}
