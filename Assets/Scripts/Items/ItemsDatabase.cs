using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsDatabase : MonoBehaviour
{
    public static ItemsDatabase instance;
    private void Awake()
    {
        if(instance != null)
        {
            return;
        }
        instance = this;
    }

    public List<Item> items;

    public int GetItemsIDs(Item item)
    {
        return items.IndexOf(item);
    }

    public Item GetItemFromID(int id)
    {
        return items[id];
    }
}
