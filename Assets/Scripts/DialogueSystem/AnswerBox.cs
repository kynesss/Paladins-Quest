using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerBox : MonoBehaviour
{
    public static AnswerBox instance;

    public bool isQuestDecisionBox = false;

    private void Awake()
    {
        if(instance != null)
        {
            return;
        }
        instance = this;
    }

    public bool isFirst = false;

    public bool isOpen = false;

    public OptionType type;

    public bool IsEmpty()
    {
        if(DialogueMenu.instance.playerAnswers.Count <= 0)
        {
            return true;
        }

        return false;
    }

    private void OnDestroy()
    {
        DialogueMenu.instance.answerBoxesList.Remove(this.gameObject);
    }
}
