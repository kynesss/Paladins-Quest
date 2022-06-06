using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Slots : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    public Item item;

    public Image icon;

    public Image iconCopy;

    public bool isEquipmentSlot = false;

    public bool isInventorySlot = false;

    public bool isChestSlot = false;

    public bool isItemEquipped = false;

    public bool isFirstSlot = false;

    public bool isShieldEquipped = false;

    public Button button;

    public DropItemClick dropItemClick;

    public GameObject leftHandSlot;

    Equipment equipment;

    PlayerController player;

    Animator playerAnim;

    private Vector3 startPos;
    public bool droppedOnSlot;

    public delegate void OnFirstSlotChanged();
    public static event OnFirstSlotChanged onFirstSlotChanged;

    private void Start()
    {
        equipment = Equipment.instance;

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            player = FindObjectOfType<PlayerController>();
            playerAnim = player.GetComponent<Animator>();
        }
        if (player == null)
            Debug.Log(null);

        isItemEquipped = false;
        leftHandSlot = player.leftHandSlot;

        if (!isEquipmentSlot)
        {
            dropItemClick.dropClick += DropItem;
        }
        else if(isEquipmentSlot)
        {
            dropItemClick.shieldAction += EquipShield;
        }
    }
    public void SetSlot(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.color = item.color;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    public void ClickOnButton()
    {
        if(isInventorySlot && !GameController.instance.isChestUiVisible)
        {
            if (item != null && !player.isTrading && item.requiredLvl <= player.stats.Level)
            {
                item.AddItemToEq();
            }
            else if(item != null && item.requiredLvl > player.stats.Level)
            {
                GameController.instance.LevelTooLowInfo(); 
            }

                onFirstSlotChanged?.Invoke();
        }
        else if(isInventorySlot && GameController.instance.isChestUiVisible)
        {
            if (item != null)
                item.AddItemToChest();
            ChestController.instance.onChestChanged?.Invoke();
        }
        else if(isChestSlot && GameController.instance.isChestUiVisible)
        {
            if (item != null)
                item.AddChestItemToInv();
            ChestController.instance.onChestChanged?.Invoke();
        }
        else if(isEquipmentSlot)
        {
            if (item != null)
            {
                if (item.itemType != ItemType.Shield)
                {
                    UnEquip();
                    item.AddItemToInv();
                }
                else if (item.itemType == ItemType.Shield && !isShieldEquipped)
                {
                    item.AddItemToInv();
                }
                else if (item.itemType == ItemType.Shield && isShieldEquipped)
                {
                    return;
                }
            }
        }
    }
    public void Equip()
    {
        if(isEquipmentSlot && item != null)
        {
            item.EquipItem();
            isItemEquipped = true;
        }
    }

    public void UnEquip()
    {
        if (item != null && isEquipmentSlot && isItemEquipped && item.itemType != ItemType.Shield)
        {
            equipment.UnEquip();
            isItemEquipped = false;
        }
    }

    public IEnumerator Consume()
    {
        if(isEquipmentSlot && item != null && item.Eatable)
        {
            yield return new WaitForSeconds(4f);
            GetFoodStats(item);
            equipment.Remove(item);
            
            equipment.rightHandSlot.gameObject.SetActive(false);
        }
        
        player.isEating = false;
        player.isDrinking = false;

        if (playerAnim != null)
        {
            playerAnim.SetBool("isEating", player.isEating);
            playerAnim.SetBool("isDrinking", player.isDrinking);
        }
        yield return null;
    }
    public void OpenBagOfGold()
    {
        if(isEquipmentSlot && item != null && item.itemType == ItemType.Gold)
        {
            equipment.OpenBagOfGold(item);

            equipment.rightHandSlot.gameObject.SetActive(false);
        }
    }

    public void GetFoodStats(Item item)
    {
        item.GetFoodStats();
    }

    public void DropItem()
    {
        if (item != null && isInventorySlot)
        {
            item.DropItem();
        }
    }

    public void EquipShield()
    {
        if (isEquipmentSlot && item != null && !isShieldEquipped)
        {
            leftHandSlot.gameObject.SetActive(true);
            item.EquipShield();
            player.stats.Defence += item.armorModifier;
            dropItemClick.shieldAction -= EquipShield;
            dropItemClick.shieldAction += UnEquipShield;
            isShieldEquipped = true;
        }
    }

    public void UnEquipShield()
    {
        if(isEquipmentSlot && item != null && isShieldEquipped)
        {
            leftHandSlot.gameObject.SetActive(false);
            equipment.UnEquipShield();
            player.stats.Defence -= item.armorModifier;
            isShieldEquipped = false;
            dropItemClick.shieldAction -= UnEquipShield;
            dropItemClick.shieldAction += EquipShield;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (player.isTrading && eventData.clickCount == 2 && !player.isFixingWeapon)
        {
            TradeController.instance.ChooseSlot(this);
            TradeController.instance.GetItemFromSlot(item);
            TradeController.instance.sellConfirmPanel.SetActive(true);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TradeController.instance.DisplayItemInfo(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TradeController.instance.ResetItemInfo();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null && item.itemType == ItemType.Weapon && player.isFixingWeapon)
        {
            droppedOnSlot = false;
            startPos = icon.transform.position;
            icon.transform.SetParent(transform);
            icon.GetComponent<CanvasGroup>().blocksRaycasts = false;

            iconCopy = null;
            iconCopy = Instantiate(icon, transform);
            iconCopy.enabled = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (item != null && player.isFixingWeapon)
        {
            icon.GetComponent<CanvasGroup>().blocksRaycasts = true;

            if (!droppedOnSlot)
            {
                icon.transform.position = startPos;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null && item.itemType == ItemType.Weapon && player.isFixingWeapon)
            icon.rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && item.itemType == ItemType.Weapon)
        {
            FixSlot fixSlot = eventData.pointerDrag.GetComponent<FixSlot>();
            
            fixSlot.icon.transform.SetParent(transform);
            fixSlot.droppedOnSlot = true;
            fixSlot.icon.rectTransform.anchoredPosition = startPos;
            Destroy(eventData.pointerDrag.GetComponent<FixSlot>().icon);
            SetNewIcon();
        }
    }

    public void SetNewIcon()
    {
        Destroy(icon.gameObject);
        icon = iconCopy;
        icon.enabled = true;
    }
}
