using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public DialogueMenu dialogueMenu;
    public NPC npc;
    public QuestGiver questGiver;

    [HideInInspector]
    public bool isTalking = false;
    public bool isEndOfTalk = false;

    [HideInInspector]
    public bool isNpcResponding = false;

    private bool canNpcTalk = false;
    private bool isNewBoxLoaded = false;
    private bool isFirstConversation = true;
    private bool isDialogueFinished = false;
    private bool isQuestDecisionBox = false;
    private bool questRejected = false;
    private bool questDialoguesUpdated = false;
    public bool isNpcInterraction = false;

    [HideInInspector]
    public float randomValue;
    private float distance;

    private int id = 0;

    public List<DialogueOptions> currentOptions;
    public DialogueOptions activeOption;

    public GameObject dialogueUI;
    public GameObject npcDialoguePanel;
    public GameObject playerDialoguePanel;
    public GameObject playerAnswerPanel;
    public GameObject currentAnswerBox;

    public Text playerDialogueBox;
    public Text playerName;
    
    [HideInInspector]
    public Text npcName;
    public Text npcDialogueBox;

    private PlayerController player;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;

        isFirstConversation = true;
    }

    void Start()
    {
        GetDialogueObjects();
        player = FindObjectOfType<PlayerController>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        dialogueUI = GameController.instance.dialogueUI;
        dialogueUI.SetActive(false);
        questGiver = GetComponent<QuestGiver>();
        dialogueMenu = DialogueMenu.instance;
    }
    private void Update()
    {
        distance = Vector3.Distance(player.transform.position, transform.position);

        if (Input.GetKeyDown(KeyCode.E) && distance <= 2.5f && canNpcTalk)
        {
            if(!isTalking)
            {
                StartConversation();
            }
            else if(isTalking && isEndOfTalk)
            {
                EndDialogue();
            }
        }
        if(isTalking && !isEndOfTalk)
        {
            SetCurrentAnswerBox();

            if (currentOptions.Count > 0)
            {
                ChooseOption();
            }
        }
        if(questGiver.isAnyQuestFinished && !DialogueCreator.MyInstance.dialogueAdded && !questDialoguesUpdated)
        {
            npc.SetQuestRewardDialogues();
            questDialoguesUpdated = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            canNpcTalk = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            canNpcTalk = false;
    }

    private void StartConversation()
    {
        DialogueMenu.instance.SetDialogueManager(this);
        SetOptions();
        isEndOfTalk = false;
        isTalking = true;
        isNewBoxLoaded = true;
        dialogueUI.SetActive(true);
        playerMovement.SetPlayerPose();
    }
    private void EndDialogue()
    {
        isTalking = false;
        isEndOfTalk = true;
        dialogueUI.SetActive(false);
        currentOptions.Clear();
        DialogueMenu.instance.playerAnswers.Clear();
        isFirstConversation = false;
        DialogueMenu.instance.ResetAllBoxes();
        currentAnswerBox = null;
        DialogueMenu.instance.ResetDialogueManager();
        playerMovement.SetPlayerPose();
    }
    public void GetDialogueObjects()
    {
        npcDialoguePanel = DialogueMenu.instance.npcDialogueUI;
        npcDialogueBox = DialogueMenu.instance.npcDialogueText;
        playerDialoguePanel = DialogueMenu.instance.playerDialogueUI;
        playerAnswerPanel = DialogueMenu.instance.playerAnswersUI;
        playerDialogueBox = DialogueMenu.instance.playerDialogueText;
        playerName = DialogueMenu.instance.playerDialogueName;
    }
    public void ChooseOption()
    {
        if(currentOptions[id].isChosen)
        {
            currentOptions[id].description.color = Color.white;
            currentOptions[id].isChosen = false;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (id < currentOptions.Count - 1)
            {
                id++;

                currentOptions[id].description.color = Color.red;
                currentOptions[id - 1].description.color = Color.white;
            }
            else if (id >= currentOptions.Count - 1)
            {
                id = currentOptions.Count - 1;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (id <= 0)
            {
                id = 0;
            }
            else
            {
                id--;

                currentOptions[id].description.color = Color.red;
                currentOptions[id + 1].description.color = Color.white;
            }
        }
        if (id == 0)
        {
            currentOptions[id].description.color = Color.red;
        }
        if(id == currentOptions.Count - 1)
        {
            currentOptions[id].description.color = Color.red;
        }
        if(id > currentOptions.Count - 1)
        {
            id = 0;
        }
        if(currentOptions[id] != null)
        {
            activeOption = currentOptions[id];
        }
        if(currentOptions[id].description.color == Color.red)
        {
            foreach (DialogueOptions option in currentOptions)
            {
                if(currentOptions.IndexOf(option) != id)
                {
                    option.description.color = Color.white;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            activeOption.isChosen = true;

            if (activeOption.isChosen && activeOption.type == OptionType.exit && activeOption.description.text != "Back")
            {
                EndDialogue();
                GameController.instance.playerInfo.gameObject.SetActive(true);
            }
            else if (activeOption.isChosen && activeOption.type == OptionType.exit && activeOption.description.text == "Back")
            {
                BackToPreviousAnswerBox();
            }
            else if (activeOption.isChosen && activeOption.type == OptionType.talk)
            {
                StartCoroutine(TalkOption());
            }
            else if (activeOption.isChosen && activeOption.type == OptionType.quest && !currentAnswerBox.GetComponent<AnswerBox>().isQuestDecisionBox)
            {
                StartCoroutine(QuestOption());
            }
            else if(activeOption.isChosen && activeOption.type == OptionType.quest && currentAnswerBox.GetComponent<AnswerBox>().isQuestDecisionBox)
            {
                AcceptOrRejectQuest(activeOption);
            }
        }

        if(isNewBoxLoaded)
        {
            id = 0;
            isNewBoxLoaded = false;
        }
    }

    public void SetOptions()
    {
        DialogueMenu.instance.CreatePlayerAnswers(); 
        DialogueMenu.instance.SetCurrentOptions(currentOptions);
        SetCurrentAnswerBox();
    }
    public void SetCurrentAnswerBox()
    {
        var answerBoxList = DialogueMenu.instance.answerBoxesList;

        for (int i = 0; i < answerBoxList.Count; i++)
        {
            if (answerBoxList[i].activeInHierarchy)
            {
                 currentAnswerBox = answerBoxList[i];
            }
        }
    }
    public void BackToPreviousAnswerBox()
    {
        var answerBoxList = DialogueMenu.instance.answerBoxesList;
        
        IsAnswerBoxEmpty(currentOptions);

        for (int i = 0; i < answerBoxList.Count; i++)
        {
            if(answerBoxList[i].activeInHierarchy && i == 1)
            {
                answerBoxList[i].SetActive(false);
                answerBoxList[i - 1].SetActive(true);
            }
            else if(answerBoxList[i].activeInHierarchy && i > 1)
            {
                answerBoxList[i].SetActive(false);
                answerBoxList[i - 2].SetActive(true);
            }
        }

        if(IsAnswerBoxEmpty(currentOptions) && !isEndOfTalk)
        {
            DialogueMenu.instance.DeleteEmptyOption(currentAnswerBox.GetComponent<AnswerBox>().type, currentAnswerBox);
        }

        SetCurrentAnswerBox();

        DialogueMenu.instance.SetCurrentOptions(currentOptions);
        LoadNewOptions();
    }
    public void GoToNextAnswerBox(OptionType boxType)
    {
        var answerBoxList = DialogueMenu.instance.answerBoxesList;


        for (int i = 0; i < answerBoxList.Count; i++)
        {
            if (answerBoxList[i].activeInHierarchy)
            {
                answerBoxList[i].SetActive(false);
                break;
            }
        }
        for (int i = 0; i < answerBoxList.Count; i++)
        {
            if(boxType == OptionType.quest && boxType == answerBoxList[i].GetComponent<AnswerBox>().type)
            {
                answerBoxList[i].SetActive(true);
                break;
            }
            else if(boxType == OptionType.talk && boxType == answerBoxList[i].GetComponent<AnswerBox>().type && i > 0)
            {
                answerBoxList[i].SetActive(true);
                break;
            }
        }
        SetCurrentAnswerBox();
        

        DialogueMenu.instance.SetCurrentOptions(currentOptions);
        LoadNewOptions();
    }

    private void AcceptOrRejectQuest(DialogueOptions activeOption)
    {
        foreach (DialogueOptions option in currentAnswerBox.GetComponentsInChildren<DialogueOptions>())
        {
            if(activeOption == option && DialogueMenu.instance.playerAnswers.IndexOf(option) == 0)
            {
                QuestGiver questGiver = GetComponent<QuestGiver>();
                if(questGiver.quests.Length > 0) { questGiver.AcceptQuest(questGiver.quests[0]); }
                
                currentAnswerBox.GetComponent<AnswerBox>().isQuestDecisionBox = false;
                questRejected = false;
                StartCoroutine(QuestOption());
                AcceptKillQuest();
            }
            else if(activeOption == option && DialogueMenu.instance.playerAnswers.IndexOf(option) == 1)
            {
                QuestGiver questGiver = GetComponent<QuestGiver>();
                if (questGiver.quests.Length > 0) { questGiver.RejectQuest(questGiver.quests[0]); }

                currentAnswerBox.GetComponent<AnswerBox>().isQuestDecisionBox = false;
                questRejected = true;
                npc.npcQuestDialogue.RemoveAt(0);
                DialogueMenu.instance.DeleteChosenAnswer(currentOptions[0]);
                id = 0;
                StartCoroutine(QuestOption());
                questGiver.isKillQuestAccepted = false;
                RejectKillQuest();
            }
        }
    }
    private void AcceptKillQuest()
    {
        if(npc._name == "Martin")
        {
            questGiver.isKillQuestAccepted = true;
            return;
        }

        questGiver.isKillQuestAccepted = false;
    }
    private void RejectKillQuest()
    {
        if (npc._name == "Martin")
        {
            questGiver.canAttackPlayer = true;
        }
    }
    public void LoadNewOptions()
    {
        var playerAnswers = DialogueMenu.instance.playerAnswers;
        
        currentOptions.Clear();
        playerAnswers.Clear();

        foreach (DialogueOptions option in currentAnswerBox.GetComponentsInChildren<DialogueOptions>())
        {
            if(option != null)
            {
                playerAnswers.Add(option);
                currentOptions.Add(option);
            }
        }
        isNewBoxLoaded = true;
    }
    public IEnumerator TalkConversation(string playerDialogue, string npcDialogue)
    {
        isDialogueFinished = false;
        playerAnswerPanel.SetActive(false);
        playerDialogueBox.text = playerDialogue;
        playerDialoguePanel.SetActive(true);

        currentAnswerBox.SetActive(false);

        yield return new WaitForSeconds(3f);
        playerDialoguePanel.SetActive(false);

        npcDialogueBox.text = npcDialogue;
        npcDialoguePanel.SetActive(true);
        randomValue = Random.Range(0f, 1.0f);
        isNpcResponding = true;

        yield return new WaitForSeconds(3f);
        npcDialoguePanel.SetActive(false);
        isNpcResponding = false;
        isTalking = true;
        isDialogueFinished = true;
    }

    private IEnumerator TalkOption()
    {
        var dialogueMenu = DialogueMenu.instance;
        
        currentAnswerBox.GetComponent<AnswerBox>().IsEmpty();

        if ((dialogueMenu.answerBoxesList.Count <= 3 || !dialogueMenu.IsFirstAnswerBox(currentAnswerBox)) && !dialogueMenu.IsAnswerBoxExist(OptionType.talk))
        {
            StartCoroutine(TalkConversation(activeOption.description.text, npc.npcTalkDialogue[0]));

            while(!isDialogueFinished)
            {
                yield return null;
            }

            currentOptions.Remove(activeOption);
            ClearUsedDialogues(npc.playerTalkDialogue, npc.npcTalkDialogue);
  

            if (dialogueMenu.answerBoxesList.Count < 4 && !dialogueMenu.IsAnswerBoxExist(OptionType.talk)) 
            {
                dialogueMenu.playerAnswers.Clear();
                dialogueMenu.CreateNewAnswerBox(activeOption, npc.playerTalkDialogue);
                SetCurrentAnswerBox();
                dialogueMenu.CreateBackOption(currentAnswerBox.transform);
                currentOptions.Clear();
                dialogueMenu.SetCurrentOptions(currentOptions);
            }
        }
        else if (dialogueMenu.IsFirstAnswerBox(currentAnswerBox) && dialogueMenu.answerBoxesList.Count > 1 && dialogueMenu.IsAnswerBoxExist(OptionType.talk))
        {
            GoToNextAnswerBox(OptionType.talk);
        }
        else if(!dialogueMenu.IsFirstAnswerBox(currentAnswerBox) && dialogueMenu.answerBoxesList.Count > 1 && dialogueMenu.IsAnswerBoxExist(OptionType.talk))
        {
            StartCoroutine(TalkConversation(activeOption.description.text, npc.npcTalkDialogue[0]));

            while (!isDialogueFinished)
            {
                yield return null;
            }

            if (questGiver.isAnyQuestFinished && npc.npcTalkDialogue.Count == 1)
            {
                currentAnswerBox.SetActive(true);
                currentOptions.Remove(activeOption);
                ClearUsedDialogues(npc.playerTalkDialogue, npc.npcTalkDialogue);
                dialogueMenu.DeleteChosenAnswer(activeOption);
                questGiver.GiveRewardAndRemoveQuest();
                questGiver.isAnyQuestFinished = false;
            }
            else
            {
                currentAnswerBox.SetActive(true);
                currentOptions.Remove(activeOption);
                ClearUsedDialogues(npc.playerTalkDialogue, npc.npcTalkDialogue);
                dialogueMenu.DeleteChosenAnswer(activeOption);
            }
        }

        yield return null;
    }
    private IEnumerator QuestOption()
    {
        var dialogueMenu = DialogueMenu.instance;

        currentAnswerBox.GetComponent<AnswerBox>().IsEmpty();

        if ((dialogueMenu.answerBoxesList.Count <= 3 || !dialogueMenu.IsFirstAnswerBox(currentAnswerBox)) && !dialogueMenu.IsAnswerBoxExist(OptionType.quest) && !isNpcInterraction)
        {
            StartCoroutine(TalkConversation(activeOption.description.text, npc.npcQuestDialogue[0]));

            while (!isDialogueFinished)
            {
                yield return null;
            }
            currentOptions.Remove(activeOption);
            ClearUsedDialogues(npc.playerQuestDialogue, npc.npcQuestDialogue);

            if (dialogueMenu.answerBoxesList.Count < 4 && !dialogueMenu.IsAnswerBoxExist(OptionType.quest))
            {
                dialogueMenu.playerAnswers.Clear();
                dialogueMenu.CreateNewAnswerBox(activeOption, npc.playerQuestDialogue);
                SetCurrentAnswerBox();
                dialogueMenu.CreateBackOption(currentAnswerBox.transform);
                currentOptions.Clear();
                dialogueMenu.SetCurrentOptions(currentOptions);
                currentAnswerBox.GetComponent<AnswerBox>().isQuestDecisionBox = true;
                id = 0;
            }
        }
        else if (dialogueMenu.IsFirstAnswerBox(currentAnswerBox) && dialogueMenu.answerBoxesList.Count > 1 && dialogueMenu.IsAnswerBoxExist(OptionType.quest) && !isNpcInterraction)
        {
            GoToNextAnswerBox(OptionType.quest);
        }
        else if ((!dialogueMenu.IsFirstAnswerBox(currentAnswerBox) && dialogueMenu.answerBoxesList.Count > 1 && dialogueMenu.IsAnswerBoxExist(OptionType.quest)) || isNpcInterraction)
        {
            if(npc._name == "Bandyta" && isNpcInterraction && player.stats.currentGold <= 0)
            {
                npc.npcQuestDialogue[0] = "Nie masz tyle z³ota k³amco! Zabijê ciê!";
                npcDialogueBox.text = npc.npcQuestDialogue[0];
            }
            
            StartCoroutine(TalkConversation(activeOption.description.text, npc.npcQuestDialogue[0]));

            while (!isDialogueFinished)
            {
                yield return null;
            }
            
            currentAnswerBox.SetActive(true);

            if (!questRejected)
            {
                QuestAcceptedOption();
            }
            else
            {
                QuestRejectedOption();
            }
        }

        yield return null;
    }
    private void QuestAcceptedOption()
    {
        var dialogueMenu = DialogueMenu.instance;


        if (npc._name != "Bandyta")
        {
            dialogueMenu.DeleteChosenAnswer(activeOption);
            currentOptions.Remove(activeOption);

            dialogueMenu.DeleteChosenAnswer(currentOptions[0]);
            currentOptions.RemoveAt(0);

            activeOption = currentOptions[0];

            for (int i = 0; i < 2; i++)
            {
                ClearUsedDialogues(npc.playerQuestDialogue, npc.npcQuestDialogue);
            }
        }
        else
        {
            if (player.stats.currentGold >= 3000)
            {
                player.stats.currentGold -= 3000;

                dialogueMenu.currentManager = null;
                dialogueMenu.ResetAllBoxes();
                npc.npcQuestDialogue.Clear();
                npc.playerQuestDialogue.Clear();
                isTalking = false;
                dialogueUI.SetActive(false);
                isNpcInterraction = false;
                currentOptions.Clear();
            }
            else
            {
                dialogueMenu.currentManager = null;
                dialogueMenu.ResetAllBoxes();
                npc.npcQuestDialogue.Clear();
                npc.playerQuestDialogue.Clear();
                isTalking = false;
                dialogueUI.SetActive(false);
                questGiver.canAttackPlayer = true;
            }
        }
    }
    private void QuestRejectedOption()
    {
        var dialogueMenu = DialogueMenu.instance;

        if (npc._name != "Bandyta")
        {
            dialogueMenu.DeleteChosenAnswer(currentOptions[0]);
            currentOptions.RemoveAt(0);

            dialogueMenu.DeleteChosenAnswer(activeOption);
            currentOptions.Remove(activeOption);

            dialogueMenu.UpdateFirstAnswerPos();

            for (int i = 0; i < 2; i++)
            {
                npc.playerQuestDialogue.RemoveAt(0);
            }
            npc.npcQuestDialogue.RemoveAt(0);
        }
        else
        {
            dialogueMenu.currentManager = null;
            dialogueMenu.ResetAllBoxes();
            npc.npcQuestDialogue.Clear();
            npc.playerQuestDialogue.Clear();
            isTalking = false;
            dialogueUI.SetActive(false);
            questGiver.canAttackPlayer = true;
        }
    }
    private void ClearUsedDialogues(List<string> playerList, List<string> npcList)
    {
        playerList.RemoveAt(0);
        npcList.RemoveAt(0);
    }
    public bool IsAnswerBoxEmpty(List<DialogueOptions> boxAnswers)
    {
        foreach (DialogueOptions an in boxAnswers)
        {
            if(boxAnswers.Contains(an) && an.type != OptionType.exit)
            {
                return false;
            }
        }
        return true;
    }
    public IEnumerator NpcDialogueInterract()
    {
        var dialogueMenu = DialogueMenu.instance;

        dialogueMenu.currentManager = this;
        isTalking = true;
        playerMovement.SetPlayerPose();
        dialogueMenu.npcDialogueName.text = npc._name;
        dialogueMenu.enabled = false;
        dialogueUI.SetActive(true);
        playerAnswerPanel.SetActive(false);
        

        for (int i = 0; i < 2; i++)
        {
            npcDialogueBox.text = npc.npcQuestDialogue[0];
            playerDialoguePanel.SetActive(false);
            npcDialoguePanel.SetActive(true);
            randomValue = Random.Range(0f, 1.0f);
            isNpcResponding = true;
            npc.npcQuestDialogue.RemoveAt(0);
            yield return new WaitForSeconds(3f);

            if(i == 1) { break; }

            playerDialogueBox.text = npc.playerQuestDialogue[0];
            npcDialoguePanel.SetActive(false);
            playerDialoguePanel.SetActive(true);
            isNpcResponding = false;
            npc.playerQuestDialogue.RemoveAt(0);
            yield return new WaitForSeconds(3f);

        }

        isNpcResponding = false;
        npcDialoguePanel.SetActive(false);
        dialogueMenu.enabled = true;
        playerAnswerPanel.SetActive(true);
        yield return new WaitForEndOfFrame();

        SetOptions();
        dialogueMenu.ReplaceExitOptionOnQuestDecision(npc.playerQuestDialogue[1]);
        currentAnswerBox.GetComponent<AnswerBox>().isQuestDecisionBox = true;
        isNpcInterraction = true;

        yield return null;
    }
}
