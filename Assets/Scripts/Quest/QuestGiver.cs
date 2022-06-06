using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestGiver : MonoBehaviour
{
    //[HideInInspector]
    private NPCController npcController;

    public NPC npc;
    
    [SerializeField]
    private QuestLog tmpLog;

    public Quest[] quests;

    private PlayerController player;

    [HideInInspector]
    public bool isAnyQuestFinished = false;
    [HideInInspector]
    public bool isKillQuestAccepted = false;
    [HideInInspector]
    public bool isKillQuestFinished = false;
    [HideInInspector]
    public bool canAttackPlayer = false;

    private void Start()
    {
        npcController = GetComponent<NPCController>();
        npc = npcController.npc;
        tmpLog = FindObjectOfType<QuestLog>();
        player = FindObjectOfType<PlayerController>();
    }
    private void Update()
    {
        if(!isAnyQuestFinished)
        CheckIsAnyQuestComplete();

        if(!player.isAlive && canAttackPlayer)
        {
            canAttackPlayer = false;
        }
    }
    public void AcceptQuest(Quest quest)
    {
        tmpLog.AcceptQuest(quest);
    }
    public void RejectQuest(Quest quest)
    {
        List<Quest> tempList = new List<Quest>(quests);
        tempList.RemoveAt(0);
        quests = tempList.ToArray();
    }
    public void CheckIsAnyQuestComplete()
    {
        foreach (Quest q in quests)
        {
            if (q.IsComplete)
            {
                isAnyQuestFinished = true;
            }
        }
    }
    public void GiveRewardAndRemoveQuest()
    {
        foreach (Quest q in quests)
        {
            if (q.IsComplete)
            {
                q.MyQuestReward[0].GiveReward(player);
                RejectQuest(q);
                break;
            }
        }

        if(npc._name == "Martin")
        {
            isKillQuestFinished = true;
        }
    }
}
