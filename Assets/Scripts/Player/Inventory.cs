using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    Equipment equipment;
    public ItemPickUp dropItem;

    PlayerController player;

    public GameObject[] chest;
    ChestController currentChest;

    public bool isInventoryFull;

    //temporary added Items
    public Item swordItem;
    public Item axeItem;
    public Item shieldItem;
    public Item manaPotion;
    public Item healthPotion;

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
        equipment = Equipment.instance;
        player = FindObjectOfType<PlayerController>();
        chest = GameObject.FindGameObjectsWithTag("Chest");
        
        StartCoroutine(AddItemsCoroutine());
    }

    public List<Item> items = new List<Item>();

    public delegate void OnInventoryChanged();
    public OnInventoryChanged onInventoryChanged;

    public IEnumerator AddItemsCoroutine()
    {
        items.Clear();
        yield return new WaitForSeconds(1f);
        Add(swordItem);
        Add(axeItem);
        Add(shieldItem);
        Add(manaPotion);
        Add(healthPotion);
    }
    public List<int> GetItemID()
    {
        List<int> temp = new List<int>();

        foreach (Item item in items)
        {
            temp.Add(ItemsDatabase.instance.GetItemsIDs(item));
        }
        return temp;
    }

    public void LoadItems(List<int> ids)
    {
        items.Clear();
        foreach (int id in ids)
        {
            items.Add(ItemsDatabase.instance.GetItemFromID(id));
        }

        if (onInventoryChanged != null)
        {
            onInventoryChanged.Invoke();
        }
    }

    public bool Add(Item item)
    {
        if(items.Count >= 24)
        {
            isInventoryFull = true;
            return false;
        }
        items.Add(item);
        

        if(onInventoryChanged != null)
        {
            onInventoryChanged.Invoke();
        }

        QuestLog.MyInstance.OnItemAdded?.Invoke(item);

        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);

        if (onInventoryChanged != null)
        {
            onInventoryChanged.Invoke();
        }
    }

    public void AddInventoryToEq(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null)
            {
                equipment.Add(item);
                Remove(item);
                break;
            }
        }
    }

    public void AddItemToChest(Item item)
    {
        currentChest = ChestUI.instance.currentChest;

        for (int i = 0; i < items.Count; i++)
        {
            if(items[i] != null && !currentChest.isListFull)
            {
                currentChest.Add(item);
                Remove(item);
                break;
            }
        }
    }

    public void DropItem(Item item)
    {
        if(item != null)
        {
            dropItem.item = item;
            Vector3 pos = new Vector3(player.transform.position.x, 10f, player.transform.position.z + 1.0f);
            ItemPickUp tempItem = Instantiate(dropItem, pos, Quaternion.Euler(0f, 0f, 0f));
            tempItem.GetComponent<MeshCollider>().sharedMesh = item.mesh.sharedMesh;

            Remove(item);
        }
    }

    public int GetItemCount(string itemName)
    {
        int itemCount = 0;

        foreach (Item item in items)
        {
            if (item.Name == itemName)
            {
                itemCount++;
            }
        }

        return itemCount;
    }

}
