using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Trader", menuName = "Traders/trader")]
public class TraderModel : ScriptableObject
{
    public string Name;

    public TraderType type;

    public float gold;

    public List<Item> items;
}
public enum TraderType
{
    Weapon, Food
}
