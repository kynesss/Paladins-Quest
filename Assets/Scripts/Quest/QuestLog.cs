using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class QuestLog : MonoBehaviour
{
    private static QuestLog instance;

    [SerializeField]
    private GameObject questPrefab;

    [SerializeField]
    private Transform questParent;

    [SerializeField]
    private Text questDescription;

    [SerializeField]
    private List<QuestScript> questScripts;

    private Quest selected;

    public static QuestLog MyInstance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<QuestLog>();
            }
            return instance;
        }
    }

    public UnityAction<Item> OnItemAdded;
    public UnityAction<EnemyController> OnEnemyKilled;
   
    public void AcceptQuest(Quest quest)
    {
        foreach (CollectObjective o in quest.MyCollectObjectives)
        { 
            OnItemAdded += o.UpdateItemCount;
            o.CheckInventory();
        }

        foreach (KillObjective kill in quest.MyKillObjectives)
        {
            OnEnemyKilled += kill.UpdateKillCount;
        }

        GameObject go = Instantiate(questPrefab, questParent);

        QuestScript qs = go.GetComponent<QuestScript>();
        questScripts.Add(qs);
        quest.MyQuestScript = qs;
        qs.MyQuest = quest;

        go.GetComponent<Text>().text = quest.MyTitle;
    }
    public void ShowDescription(Quest quest)
    {
        if (quest != null)
        {
            if (selected != null && selected != quest)
            {
                selected.MyQuestScript.DeSelect();
            }

            selected = quest;

            string title = quest.MyTitle;

            string currentObjectives = string.Empty;

            SetQuestDescription(currentObjectives, title, quest);
        }
    }
    public void UpdateSelected()
    {
        ShowDescription(selected);
    }
    public void CheckCompletion()
    {
        foreach (QuestScript qs in questScripts)
        {
            qs.IsComplete();
        }
    }
    public void SetQuestDescription(string objectiveInfo, string title, Quest quest)
    {
        foreach (Objective obj in quest.MyCollectObjectives)
        {
            objectiveInfo += "Typ: " + obj.MyType + " " + obj.MyCurrentAmount + "/" + obj.MyAmount;
        }
        foreach (Objective obj in quest.MyKillObjectives)
        {
            objectiveInfo += "Typ: " + obj.MyType + " " + obj.MyCurrentAmount + "/" + obj.MyAmount;
        }

        questDescription.text = string.Format("<size=15>{0}</size>\n\n<size=13>{1}\n{2}</size>", title, quest.MyDescription, objectiveInfo);
    }
}
