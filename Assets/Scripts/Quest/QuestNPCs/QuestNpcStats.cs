using System;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestNpc", menuName = "QuestNPCs/questNPC")]
public class QuestNpcStats : ScriptableObject
{
    public int Level = 1;

    public float Experience = 0f;

    public float Health = 100.0f;

    public float MaxHealth = 100.0f;

    public float Attack = 10.0f;

    public float MagicAttack = 10.0f;

    public float Defence = 10.0f;
}
