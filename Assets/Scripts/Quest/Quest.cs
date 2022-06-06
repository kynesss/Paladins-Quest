using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum QuestType
{
    kill, talk, collect
}

[System.Serializable]
public class Quest
{
    [SerializeField]
    private string title;

    [SerializeField]
    private string description;

    [SerializeField]
    private QuestType questType;

    [SerializeField]
    private QuestReward[] reward;

    [SerializeField]
    private CollectObjective[] collectObjectives;

    [SerializeField]
    private KillObjective[] killObjectives;

    public QuestScript MyQuestScript { get; set; }

    public string MyTitle
    {
        get
        {
            return title;
        }
        set
        {
            title = value;
        }
    }
    public string MyDescription
    {
        get
        {
            return description;
        }
        set
        {
            description = value;
        }
    }

    public QuestType MyQuestType { get => questType; }

    public QuestReward[] MyQuestReward { get => reward; }

    public CollectObjective[] MyCollectObjectives { get => collectObjectives; }

    public KillObjective[] MyKillObjectives { get => killObjectives; }

    public bool IsComplete
    {
        get
        {
            if (collectObjectives != null)
            {
                foreach (Objective c in collectObjectives)
                {
                    if (!c.IsComplete)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            if (killObjectives != null)
            {
                foreach (Objective k in killObjectives)
                {
                    if (!k.IsComplete)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        set
        {
            IsComplete = value;
        }
    }
}

[Serializable]
public abstract class Objective
{
    [SerializeField]
    private int amount;

    private int currentAmount;

    [SerializeField]
    private string type;

    public int MyAmount { get => amount; set => amount = value; }
    public int MyCurrentAmount { get => currentAmount; set => currentAmount = value; }
    public string MyType { get => type; set => type = value; }
    
    public bool IsComplete
    {
        get
        {
            return MyCurrentAmount >= MyAmount;
        }
    }
}

[Serializable]
public class CollectObjective : Objective
{
    public void UpdateItemCount(Item item)
    {
        if(MyType.ToLower() == item.Name.ToLower())
        {
            MyCurrentAmount = Inventory.instance.GetItemCount(item.Name);
            QuestLog.MyInstance.UpdateSelected();
            QuestLog.MyInstance.CheckCompletion();
        }
    }
    public void CheckInventory()
    {
        foreach (Item item in Inventory.instance.items)
        {
            if(item.Name.ToLower() == MyType.ToLower())
            {
                MyCurrentAmount++;
                QuestLog.MyInstance.UpdateSelected();
                QuestLog.MyInstance.CheckCompletion();
            }
        }
    }
}

[Serializable]
public class KillObjective : Objective
{
    public void UpdateKillCount(EnemyController enemy)
    {
        if(enemy.stats.Name.ToLower() == MyType.ToLower() && !enemy.isAlive)
        {
            MyCurrentAmount++;
            QuestLog.MyInstance.UpdateSelected();
            QuestLog.MyInstance.CheckCompletion();
        }
    }
}

[Serializable]
public class QuestReward
{
    [SerializeField]
    private float experience;

    [SerializeField]
    private int gold;

    [SerializeField]
    private Item item;

    public float MyExperience { get => experience; set => experience = value; }
    public int MyGold { get => gold; set => gold = value; }
    public Item MyItem { get => item; set => item = value; }

    public void GiveReward(PlayerController player)
    {
        player.stats.Experience += MyExperience;
        player.stats.currentGold += MyGold;

        if(MyItem != null)
            Inventory.instance.Add(MyItem);
    }
}