using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    public static EquipmentUI instance;
    
    Equipment equipment;

    public Transform parent;

    Slots[] eqSlots;

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }
    private void Start()
    {
        equipment = Equipment.instance;
        eqSlots = parent.GetComponentsInChildren<Slots>();
        equipment.onEquipmentChanged += UpdateUI;
    }

    public void UpdateUI()
    {
        for (int i = 0; i < eqSlots.Length; i++)
        {
            if (i < equipment.equipmentItems.Count)
            {
                eqSlots[i].SetSlot(equipment.equipmentItems[i]);
            }
            else
            {
                eqSlots[i].ClearSlot();
            }
        }
    }
}
