using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chest", menuName = "Inventory/Chest")]
public class ChestModel : ScriptableObject
{
    public List<Item> items;

    public int id;
    public string Name;

}
