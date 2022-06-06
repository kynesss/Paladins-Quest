using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Stats : MonoBehaviour
{
    public string Name;

    public int Level = 1;

    public float Experience = 0f;

    public float Health = 600.0f;
    public float MaxHealth = 600.0f;

    public float Mana = 100.0f;
    public float MaxMana = 100.0f;

    public float Stamina = 100.0f;
    public float MaxStamina = 100.0f;

    //atak bazowy
    public float baseAttack = 50.0f;
    public float baseMagicAttack = 50.0f;
    public float baseDefence = 10.0f;

    public float Attack = 50.0f;
    public float MagicAttack = 50.0f;
    public float Defence = 50.0f;

    public int currentGold;
    [HideInInspector] public UnityEvent onGoldChanged;
    

    public bool AddExp(float value)
    {
        Experience += value;
        if(Experience >= Level * 200)
        {
            Experience -= Level * 200;
            Level++;
            Health = MaxHealth;
            return true;
        }

        return false;
    }


}
