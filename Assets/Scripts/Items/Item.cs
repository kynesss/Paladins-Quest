using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum WeaponType
{
    OneHanded,
    TwoHanded,
    NotWeapon
}
public enum ItemType
{
    Weapon,
    Food,
    Potion,
    Shield,
    Gold,
    Other
}
[Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string Name;

    public ItemType itemType;

    public WeaponType weaponType;

    public Sprite icon;

    public Color color = Color.white;

    public SkinnedMeshRenderer mesh;

    public int requiredLvl;

    public int price;

    public float armorModifier;

    public float damageModifier;

    public float healthModifier;

    public float manaModifier;

    public float staminaModifier;

    public float weaponConditionModifier;
    
    public bool Eatable;
   
    public void AddItemToInv()
    {
        Equipment.instance.AddEqToInventory(this);
    }

    public void AddItemToEq()
    {
        Inventory.instance.AddInventoryToEq(this);
    }
    public void AddItemToChest()
    {
        Inventory.instance.AddItemToChest(this);
    }
    public void AddChestItemToInv()
    {
        ChestUI.instance.currentChest.AddItemToInventory(this);
    }
    public void EquipItem()
    {
        Equipment.instance.Equip(this);
    }

    public void DropItem()
    {
        Inventory.instance.DropItem(this);
    }

    public void EquipShield()
    {
        Equipment.instance.EquipShield(this);
    }

    public void GetFoodStats()
    {
        Equipment.instance.GetFoodStats(this);
    }
}
