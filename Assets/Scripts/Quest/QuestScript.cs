using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestScript : MonoBehaviour
{
    public Quest MyQuest { get; set; }
    public bool markedComplete { get; set; }

    public void Select()
    {
        GetComponent<Text>().color = Color.red;
        QuestLog.MyInstance.ShowDescription(MyQuest);
    }
    public void DeSelect()
    {
        GetComponent<Text>().color = Color.black;
    }
    public void IsComplete()
    {
        if (MyQuest.IsComplete && !markedComplete)
        {
            markedComplete = true;
            GetComponent<Text>().text += "(Complete)";
        }
        else if(!MyQuest.IsComplete && markedComplete)
        {
            markedComplete = false;
            GetComponent<Text>().text = MyQuest.MyTitle;
        }
    }
}
