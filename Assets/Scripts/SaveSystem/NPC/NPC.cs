using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NPC file", menuName = "NPC files archive")]
public class NPC : ScriptableObject
{
    public string _name;

    //NPC dialogues
    [TextArea(3, 15)]
    public List<string> npcTalkDialogue;

    [TextArea(3, 15)]
    public List<string> npcQuestDialogue;

    [TextArea(3, 15)]
    public List<string> npcTradeDialogue;

    [TextArea(3, 15)]
    public List<string> npcExitDialogue;

    //Player answers
    [TextArea(3, 15)]
    public List<string> playerTalkDialogue;

    [TextArea(3, 15)]
    public List<string> playerQuestDialogue;

    [TextArea(3, 15)]
    public List<string> playerTradeDialogue;

    [TextArea(3, 15)]
    public List<string> playerExitDialogue;

    public void SetDialogues()
    {
        DialogueCreator.MyInstance.ClearListsAndSetDialogues(this);
    }
    public void SetQuestRewardDialogues()
    {
        DialogueCreator.MyInstance.AddQuestRewardDialogues(this);
        DialogueCreator.MyInstance.dialogueAdded = false;
    }
}
