using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public static Equipment instance;
    
    private int space = 10;
    private int scroll;
    Inventory inventory;
    
    PlayerController player;
    Animator playerAnim;

    public Slots[] eqSlots;
    public Transform eqParent;

    public Item[] currentEquipment;
   
    public Transform rightHandSlot;
    public MeshFilter r_meshfilter;
    public MeshRenderer r_renderer;
    
    public Transform leftHandSlot;
    public MeshFilter l_meshfilter;
    public MeshRenderer l_renderer;

    public bool isShieldEquipped = false;
    public bool isTwoHandedWeaponEquipped = false;

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
        eqParent = EquipmentUI.instance.parent;
        inventory = Inventory.instance;
        eqSlots = eqParent.GetComponentsInChildren<Slots>();
        scroll = 0;
        currentEquipment = new Item[10];
        
        rightHandSlot.gameObject.SetActive(false);
        leftHandSlot.gameObject.SetActive(false);
        
        player = FindObjectOfType<PlayerController>();
        playerAnim = player.GetComponent<Animator>();

        Slots.onFirstSlotChanged += EquipFirstSlot;
    }

    public List<Item> equipmentItems = new List<Item>();
    
    public delegate void OnEquipmentChanged();
    public OnEquipmentChanged onEquipmentChanged;

    public delegate void OnStatsChanged(Item item);
    public event OnStatsChanged onStatsChanged;

    public List<int> GetItemID()
    {
        List<int> temp = new List<int>();
        foreach (Item item in equipmentItems)
        {
            temp.Add(ItemsDatabase.instance.GetItemsIDs(item));
        }
        return temp;
    }

    public void LoadItems(List<int> ids)
    {
        equipmentItems.Clear();

        foreach (int id in ids)
        {
            equipmentItems.Add(ItemsDatabase.instance.GetItemFromID(id));
        }

        if (onEquipmentChanged != null)
            onEquipmentChanged.Invoke();
    }

    private void Update()
    {
        ScrollBetweenFields();
    }

    public bool Add(Item item)
    {
        if(equipmentItems.Count >= space)
        {
            return false;
        }

        equipmentItems.Add(item);

        if (onEquipmentChanged != null)
            onEquipmentChanged.Invoke();

        return true;
    }

    public void Remove(Item item)
    {
        equipmentItems.Remove(item);

        if(onEquipmentChanged != null)
            onEquipmentChanged.Invoke();
    }

    public void AddEqToInventory(Item item)
    {
        for (int i = 0; i < equipmentItems.Count; i++)
        {
            if(equipmentItems[i] != null)
            {
                inventory.Add(item);
                Remove(item);
                break;
            }
        }
    }

    public void EquipFirstSlot()
    {
        if (!eqSlots[0].isItemEquipped)
        {
            eqSlots[0].Equip();

            if(eqSlots[0].isItemEquipped)
                player.stats.Attack += eqSlots[0].item.damageModifier;
        }
    }

    private void ScrollBetweenFields()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (scroll <= 8)
            {
                scroll++;
                if(eqSlots[scroll - 1].isItemEquipped)
                {
                    eqSlots[scroll - 1].UnEquip();
                    
                        onStatsChanged?.Invoke(eqSlots[scroll].item);
                }

                if (!eqSlots[scroll].isItemEquipped)
                {
                    eqSlots[scroll].Equip();

                        onStatsChanged?.Invoke(eqSlots[scroll].item);
                }
            }
        }

       else if(Input.GetAxis("Mouse ScrollWheel") < 0f)
       {
            if (scroll > 0)
            {
                scroll--;

                if (eqSlots[scroll + 1].isItemEquipped)
                {
                    eqSlots[scroll + 1].UnEquip();

                    if (onStatsChanged != null)
                        onStatsChanged.Invoke(eqSlots[scroll].item);
                }

                if (!eqSlots[scroll].isItemEquipped)
                {
                    eqSlots[scroll].Equip();

                    if (onStatsChanged != null)
                        onStatsChanged.Invoke(eqSlots[scroll].item);
                }
            }
        }

       if(Input.GetKeyDown(KeyCode.Mouse0) && !GameController.instance.isInventoryVisible)
       {
            StartCoroutine(eqSlots[scroll].Consume());
            eqSlots[scroll].OpenBagOfGold();
       }
    }

    public void Equip(Item item)
    {
        if (rightHandSlot == null)
            rightHandSlot = CurrentWeapon.instance.gameObject.transform;

        if (item != null && item.itemType != ItemType.Shield)
        {
            rightHandSlot.gameObject.SetActive(true);
            rightHandSlot.transform.localScale = new Vector3(1, 1, 1);
            int slotIndex = (int)item.itemType;
            
            currentEquipment[slotIndex] = item;
            CurrentWeapon.instance.item = item;

            if (item.itemType == ItemType.Weapon && item.requiredLvl <= player.stats.Level && item.weaponType == WeaponType.OneHanded)
            {
                SetWeaponMesh(item);

                isTwoHandedWeaponEquipped = false;
                player.isEquipped = true;
                player.isEating = false;
                player.isDrinking = false;
                CurrentWeapon.instance.SetWeaponPoisonEffect();
            }
            else if (item.itemType == ItemType.Weapon && item.requiredLvl <= player.stats.Level && item.weaponType == WeaponType.TwoHanded && !isShieldEquipped)
            {
                SetWeaponMesh(item);

                isTwoHandedWeaponEquipped = true;
                player.isEquipped = true;
                player.isEating = false;
                player.isDrinking = false;
                CurrentWeapon.instance.SetWeaponPoisonEffect();
            }
            else if (item.itemType != ItemType.Weapon && item.itemType != ItemType.Shield)
            {
                r_meshfilter.sharedMesh = item.mesh.sharedMesh;
                r_renderer.sharedMaterials = item.mesh.sharedMaterials;
                player.isEquipped = false;
                player.isEating = false;
                player.isDrinking = false;
                isTwoHandedWeaponEquipped = false;

                if (item.itemType == ItemType.Food)
                {
                    player.isEating = true;
                }
                if (item.itemType == ItemType.Potion)
                {
                    rightHandSlot.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    player.isDrinking = true;
                }
                CurrentWeapon.instance.SetWeaponPoisonEffect();
            }
            else
                UnEquip();
        }
    }
    public void UnEquip()
    {
        if (rightHandSlot == null)
            rightHandSlot = CurrentWeapon.instance.gameObject.transform;
        else
            rightHandSlot.gameObject.SetActive(false);
        
        player.isEquipped = false;
        player.isEating = false;
        player.isDrinking = false;
        isTwoHandedWeaponEquipped = false;

        if(CurrentWeapon.instance.item != null)
            CurrentWeapon.instance.item = null;
    }

    public void SetWeaponMesh(Item item)
    {
        r_meshfilter.sharedMesh = item.mesh.sharedMesh;
        r_renderer.sharedMaterials = item.mesh.sharedMaterials;
        CurrentWeapon.instance.item = item;
        CurrentWeapon.instance.CheckWeaponType(item);
    }

    public void EquipShield(Item item)
    {
        if(item != null && item.itemType == ItemType.Shield && item.requiredLvl <= player.stats.Level && !isTwoHandedWeaponEquipped && !isShieldEquipped)
        {
            leftHandSlot.gameObject.SetActive(true);
            int slotIndex = (int)item.itemType;

            currentEquipment[slotIndex] = item;

            l_meshfilter.sharedMesh = item.mesh.sharedMesh;
            l_renderer.sharedMaterials = item.mesh.sharedMaterials;

            playerAnim.SetFloat("PlayerEquipped", 1f);
            playerAnim.SetBool("isShieldEquipped", true);
            player.isEquipped = true;

            CurrentShield.instance.currentShield = item;
            isShieldEquipped = true;
        }
        else if(isTwoHandedWeaponEquipped)
        {
            UnEquipShield();
        }
    }

    public void UnEquipShield()
    {
        leftHandSlot.gameObject.SetActive(false);
        player.isEquipped = false;
        playerAnim.SetFloat("PlayerEquipped", 0.5f);
        playerAnim.SetBool("isShieldEquipped", false);
        CurrentShield.instance.currentShield = null;
        isShieldEquipped = false;
    }

    public void ConsumeItem(Item item)
    {
        if(item.Eatable)
        {
            Remove(item);
        }
    }
    public void OpenBagOfGold(Item item)
    {
        player.stats.currentGold += item.price;
        Remove(item);
    }
    public void GetFoodStats(Item item)
    {
        if (item.itemType == ItemType.Food || item.Name == "Health Potion")
        {
            if (player.stats.Health < player.stats.MaxHealth)
            {
                if(player.stats.MaxHealth - player.stats.Health < item.healthModifier)
                {
                    player.stats.Health += (player.stats.MaxHealth - player.stats.Health);
                }
                else
                {
                    player.stats.Health += item.healthModifier;
                }
            }
            else
            {
                return;
            }
        }
        else if (item.itemType == ItemType.Potion && item.Name == "Mana Potion")
        {
            if (player.stats.Mana < player.stats.MaxMana)
            {
                if (player.stats.MaxMana - player.stats.Mana < item.manaModifier)
                {
                    player.stats.Mana += (player.stats.MaxMana - player.stats.Mana);
                }
                else
                {
                    player.stats.Mana += item.manaModifier;
                }
            }
            else
            {
                return;
            }
        }
        else if (item.itemType == ItemType.Potion && item.Name == "Stamina Potion")
        {
            if (player.stats.Mana < player.stats.MaxMana)
            {
                if (player.stats.MaxMana - player.stats.Mana < item.manaModifier)
                {
                    player.stats.Mana += (player.stats.MaxMana - player.stats.Mana);
                }
                else
                {
                    player.stats.Mana += item.manaModifier;
                }
            }
            else
            {
                return;
            }
        }
    }
}
