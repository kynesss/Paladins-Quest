using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TradeController : MonoBehaviour
{
    public static TradeController instance;

    private PlayerController player;

    public Trader currentTrader;

    public TradeDialogueManager currentManager;

    public GameObject tradePanel;

    public GameObject goldPanel;

    public GameObject itemInfoPanel;

    public GameObject tradeConfirmPanel;

    public GameObject noMoneyPanel;

    public GameObject sellConfirmPanel;

    public Image itemInfoImage;

    public Text itemInfoTitle;

    public Text itemInfoText;

    public Text itemDescription;

    [HideInInspector] public Item currentItem;

    public Text goldText;

    public ShopSlots[] shopSlots;
    [HideInInspector] public ShopSlots chosenShopSlot;
    [HideInInspector] public Slots chosenSlot;

    private void Start()
    {
        shopSlots = tradePanel.GetComponentsInChildren<ShopSlots>();
        player = FindObjectOfType<PlayerController>();
        TradeMenu.instance.onPlayerTrade.AddListener(UpdateUI);
    }

    private void Awake()
    {
        if(instance != null)
        {
            return;
        }
        instance = this;
    }

    private void Update()
    {
        if(currentManager != null && TradeMenu.instance.playerTalkPanel.activeInHierarchy)
        {
            currentManager.ChooseOption();
        }
    }
    public void SetCurrentTrader(Trader trader)
    {
        if (currentTrader == null)
        {
            currentTrader = trader;
        }
        currentManager = currentTrader.manager;
        TradeMenu.instance.SetOptions(trader);
        player.movement.SetPlayerPose();
        player.SetPlayerTradeMode();
    }
    public void UpdateUI()
    {
        goldText.text = currentTrader.trader.gold.ToString();

        for (int i = 0; i < currentTrader.trader.items.Count; i++)
        {
            if(i < currentTrader.trader.items.Count)
            {
                shopSlots[i].SetSlot(currentTrader.trader.items[i]);
            }
            else
            {
                shopSlots[i].ClearSlot();
            }
        }
    }
    public void DisplayItemInfo(Item item)
    {
        if (item != null)
        {
            itemInfoTitle.text = item.Name;
            itemInfoImage.sprite = item.icon;

            if(item.itemType == ItemType.Shield)
            {
                itemDescription.text = "Typ: \n" + "Wymagany poziom: \n" + "Obrona: \n" + "Cena: ";
                itemInfoText.text = $"{item.itemType}\n{item.requiredLvl}\n{item.armorModifier}\n{item.price}";
            }
            else if(item.itemType == ItemType.Weapon)
            {
                itemDescription.text = "Typ: \n" + "Wymagany poziom: \n" + "Broñ: \n" + "Si³a ataku: \n" + "Wytrzyma³oœæ: \n" + "Cena: ";
                itemInfoText.text = $"{item.itemType}\n{item.requiredLvl}\n{item.weaponType}\n{item.damageModifier}\n{item.weaponConditionModifier}\n{item.price}";
            }
            else
            {
                itemDescription.text = "Typ: \n" + "Regeneracja zdrowia: \n" + "Regeneracja many: \n" + "Regeneracja kondycji: \n" + "Cena: ";
                itemInfoText.text = $"{item.itemType}\n{item.healthModifier}\n{item.manaModifier}\n{item.staminaModifier}\n{item.price}";
            }
            
            itemInfoPanel.GetComponent<RectTransform>().position = new Vector3(Input.mousePosition.x, Input.mousePosition.y - 200f);
            itemInfoPanel.SetActive(true);
        }
    }
    public void ResetItemInfo()
    {
        itemInfoPanel.SetActive(false);
        itemInfoTitle.text = null;
        itemInfoImage.sprite = null;
        itemInfoText.text = "";
    }
    public void OpenTradeConfirmPanel(Item item)
    {
        if(player.stats.currentGold >= item.price)
        {
            tradeConfirmPanel.SetActive(true);
        }
        else
        {
            noMoneyPanel.SetActive(true);
        }
    }
    public void CloseTradeConfirmPanel()
    {
        if(tradeConfirmPanel.activeInHierarchy)
        {
            tradeConfirmPanel.SetActive(false);
        }
        else if(noMoneyPanel.activeInHierarchy)
        {
            noMoneyPanel.SetActive(false);
        }
        else if(sellConfirmPanel.activeInHierarchy)
        {
            sellConfirmPanel.SetActive(false);
        }
    }
    public void GetItemFromSlot(Item item)
    {
        currentItem = item;
    }
    public void ChooseShopSlot(ShopSlots shopSlot)
    {
        chosenShopSlot = shopSlot;
    }
    public void ChooseSlot(Slots slot)
    {
        chosenSlot = slot;
    }
    public void BuyItem()
    {
        Inventory.instance.Add(currentItem);
        player.stats.currentGold -= currentItem.price;
        currentTrader.trader.gold += currentItem.price;
        UpdateCurrentGold(currentTrader);
        currentTrader.trader.items.Remove(currentItem);
        chosenShopSlot.ClearSlot();
        CloseTradeConfirmPanel();
        currentItem = null;
        chosenShopSlot = null;
    }
    public void SellItem()
    {
        Inventory.instance.Remove(currentItem);
        player.stats.currentGold += currentItem.price;
        currentTrader.trader.gold -= currentItem.price;
        UpdateCurrentGold(currentTrader);
        currentTrader.trader.items.Add(currentItem);
        UpdateUI();
        sellConfirmPanel.SetActive(false);
        currentItem = null;
        chosenSlot = null;
    }
    public void UpdateCurrentGold(Trader _trader)
    {
        if (goldText.text != _trader.trader.gold.ToString())
        {
            goldText.text = _trader.trader.gold.ToString();
        }
    }
    public void ClearSlots()
    {
        foreach (ShopSlots slot in shopSlots)
        {
            slot.ClearSlot();
        }
    }
    public void ResetCurrentTrader()
    {
        currentManager = null;
        currentTrader = null;
        player.movement.SetPlayerPose();
        player.ExitPlayerTradeMode();
    }
}
