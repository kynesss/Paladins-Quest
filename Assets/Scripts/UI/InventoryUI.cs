using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;
    
    Inventory inventory;
    PlayerController player;
    public GameObject panel;
    public Transform BagParent;
    public Text goldInfo;

    Slots[] slots;

    private void Awake()
    {
        if(instance != null)
        {
            return;
        }
        instance = this;
    }
    private void Start()
    {
        slots = BagParent.GetComponentsInChildren<Slots>();
        player = FindObjectOfType<PlayerController>();
        inventory = Inventory.instance;
        inventory.onInventoryChanged += UpdateUI;
        player.stats.onGoldChanged.AddListener(UpdateGoldUI);
        panel.SetActive(false);
    }

    public void Show()
    {
        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
        TradeController.instance.ResetItemInfo();
    }

    private void Update()
    {
        if(goldInfo.text != player.stats.currentGold.ToString())
        {
            player.stats.onGoldChanged?.Invoke();
        }
        if(player.isTrading)
        {
            TurnOffButtons();
        }
        else
        {
            TurnOnButtons();
        }
    }

    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if(i < inventory.items.Count)
            {
                slots[i].SetSlot(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
    public void UpdateGoldUI()
    {
        goldInfo.text = player.stats.currentGold.ToString();
    }
    public void TurnOffButtons()
    {
        foreach (Slots slot in slots)
        {
            if(slot.button.gameObject.activeInHierarchy)
            slot.button.gameObject.SetActive(false);
        }
    }
    public void TurnOnButtons()
    {
        foreach (Slots slot in slots)
        {
            if (!slot.button.gameObject.activeInHierarchy)
                slot.button.gameObject.SetActive(true);
        }
    }

}
