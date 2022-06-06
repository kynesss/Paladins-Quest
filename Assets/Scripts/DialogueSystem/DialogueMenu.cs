using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueMenu : MonoBehaviour
{
    public static DialogueMenu instance;
    private PlayerController player;

    public GameObject npcDialogueUI;
    public Text npcDialogueText;
    public Text npcDialogueName;

    public GameObject playerDialogueUI;
    public GameObject playerAnswersUI;
    public Text playerDialogueText;
    public Text playerDialogueName;

    public List<DialogueOptions> playerAnswers;
    public List<GameObject> answerBoxesList;

    public GameObject answerPrefab;
    public Transform answerParent;

    public GameObject answerPanelPrefab;

    public DialogueManager currentManager;

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
        player = FindObjectOfType<PlayerController>();
        playerDialogueName.text = player.stats.Name;

        var boxes = GameObject.FindGameObjectsWithTag("AnswerBox");
        if (boxes[0] != null && boxes[0].activeInHierarchy)
            answerBoxesList.Add(boxes[0]);
    }

    private void Update()
    {
        if(currentManager != null)
        {
            npcDialogueName.text = currentManager.npc._name;
        }

        CheckIsEmptyBox();
        ClearOptionsAndAnswers();
    }

    public void SetDialogueManager(DialogueManager dialogueManager)
    {
        currentManager = dialogueManager;
    }
    public void ResetDialogueManager()
    {
        currentManager = null;
    }
    public void SetCurrentOptions(List<DialogueOptions> options)
    {
        foreach (DialogueOptions answer in playerAnswers)
        {
            if(answer != null)
            options.Add(answer);
        }
    }
    public void AddPlayerAnswers(List<string> dialogue, OptionType answerType)
    {
        if(dialogue[0] != null)
        {
            if (playerAnswers.Count == 0)
            {
                var option = Instantiate(answerPrefab, answerParent);
                option.GetComponent<DialogueOptions>().description.text = dialogue[0];
                option.GetComponent<DialogueOptions>().type = answerType;
                playerAnswers.Add(option.GetComponent<DialogueOptions>());
            }
            else
            {
                foreach (DialogueOptions answer in answerParent.GetComponentsInChildren<DialogueOptions>())
                {
                    if(playerAnswers.IndexOf(answer) == playerAnswers.Count - 1)
                    {

                        var option = Instantiate(answerPrefab, answerParent);
                        if(option.GetComponent<RectTransform>().transform.position.y == playerAnswers[0].transform.position.y)
                        {
                            var posY = answer.GetComponent<RectTransform>().position.y - 51;
                            var posX = answerParent.GetComponent<RectTransform>().position.x;
                            option.transform.position = new Vector2(posX, posY);
                        }

                        
                        option.GetComponent<DialogueOptions>().description.text = dialogue[0];
                        option.GetComponent<DialogueOptions>().type = answerType;
                        playerAnswers.Add(option.GetComponent<DialogueOptions>());
                    }
                }
            }
        }
    }
    public void CreatePlayerAnswers()
    {
        if (currentManager.npc.playerTalkDialogue.Count > 0)
        {
            AddPlayerAnswers(currentManager.npc.playerTalkDialogue, OptionType.talk);
        }
        if (currentManager.npc.playerQuestDialogue.Count > 0)
        {
            AddPlayerAnswers(currentManager.npc.playerQuestDialogue, OptionType.quest);
        }
        if (currentManager.npc.playerTradeDialogue.Count > 0)
        {
            AddPlayerAnswers(currentManager.npc.playerTradeDialogue, OptionType.trade);
        }
        if (currentManager.npc.playerExitDialogue.Count > 0)
        {
            AddPlayerAnswers(currentManager.npc.playerExitDialogue, OptionType.exit);
        }
    }

    public void ClearOptionsAndAnswers()
    {
        if(playerAnswers.Count >= 4)
        {
            foreach (DialogueOptions answer in playerAnswers)
            {
                if(playerAnswers.IndexOf(answer) >= 4)
                {
                    Destroy(answer.gameObject);
                    playerAnswers.Remove(answer);
                }
            }
        }
        if (currentManager.currentOptions.Count >= 4)
        {
            foreach (DialogueOptions option in currentManager.currentOptions)
            {
                if (currentManager.currentOptions.IndexOf(option) >= 4)
                {
                    currentManager.currentOptions.Remove(option);
                }
            }
        }
    }

    public void DeleteAllOptions()
    {
        foreach (DialogueOptions answer in playerAnswers)
        {
            Destroy(answer.gameObject);
        }
    }
    public void DeleteChosenAnswer(DialogueOptions answer)
    {
        UpdateAnswersPositions(answer, playerAnswers);
        Destroy(answer.gameObject);
    }
    public bool IsEmptyOption(DialogueOptions option, OptionType type)
    {
        foreach (DialogueOptions answer in playerAnswers)
        {
            if(answer.type == type)
            {
                return false;
            }
        }

        return true;
    }
    
    public int CheckOptionIndex(DialogueOptions option, List<DialogueOptions> chosenBox)
    {
        return chosenBox.IndexOf(option);
    }

    public void UpdateAnswersPositions(DialogueOptions option, List<DialogueOptions> chosenBox)
    {
        int j = 0;

        if (CheckOptionIndex(option, chosenBox) == 0) { j = 1; }
        else if (CheckOptionIndex(option, chosenBox) == 1) { j = 2; }
        else if (CheckOptionIndex(option, chosenBox) == 2) { j = 3; }

        for (int i = j; i < chosenBox.Count; i++)
        {
            chosenBox[i].transform.position = new Vector2(chosenBox[i].transform.position.x, chosenBox[i].transform.position.y + 51);
        }
    }
    public void UpdateFirstAnswerPos()
    {
        foreach (var an in playerAnswers)
        {
            if(playerAnswers.IndexOf(an) == 1)
            {
                an.transform.position = new Vector2(an.transform.position.x, an.transform.position.y - 51);
            }
        }
    }
    public void CreateNewAnswerBox(DialogueOptions activeOption, List<string> list)
    {
        var answerBox = Instantiate(answerPanelPrefab, transform);

        if (activeOption.type == OptionType.talk || activeOption.type == OptionType.quest)
        {
            foreach (var element in list)
            {
                DisplayAnswersFromList(element, activeOption.type, answerBox.transform);
            }
        }
        answerBox.SetActive(true);
        answerBox.GetComponent<AnswerBox>().type = activeOption.type;
        answerBoxesList.Add(answerBox);
    }
    public void DisplayAnswersFromList(string dialogue, OptionType type, Transform answerBoxTransform)
    {
        if (dialogue != "")
        {
            if (playerAnswers.Count == 0)
            {
                var option = Instantiate(answerPrefab, answerBoxTransform);
                option.GetComponent<DialogueOptions>().description.text = dialogue;
                option.GetComponent<DialogueOptions>().type = type;
                playerAnswers.Add(option.GetComponent<DialogueOptions>());
            }
            else
            {
                foreach (DialogueOptions answer in answerBoxTransform.GetComponentsInChildren<DialogueOptions>())
                {
                    if (playerAnswers.IndexOf(answer) == playerAnswers.Count - 1 && playerAnswers.Count < 4)
                    {
                        var option = Instantiate(answerPrefab, answerBoxTransform);
                        if (option.GetComponent<RectTransform>().transform.position.y == playerAnswers[0].transform.position.y)
                        {
                            var posY = answer.GetComponent<RectTransform>().position.y - 51;
                            var posX = answerParent.GetComponent<RectTransform>().position.x;
                            option.transform.position = new Vector2(posX, posY);
                        }
                        option.GetComponent<DialogueOptions>().description.text = dialogue;
                        option.GetComponent<DialogueOptions>().type = type;
                        playerAnswers.Add(option.GetComponent<DialogueOptions>());
                    }
                }
            }
        }
    }
    public void CreateBackOption(Transform answerBoxTransform)
    {
        foreach (DialogueOptions answer in answerBoxTransform.GetComponentsInChildren<DialogueOptions>())
        {
            if (playerAnswers.IndexOf(answer) == playerAnswers.Count - 1)
            {
                var option = Instantiate(answerPrefab, answerBoxTransform);
                if (option.GetComponent<RectTransform>().transform.position.y == playerAnswers[0].transform.position.y)
                {
                    var posY = answer.GetComponent<RectTransform>().position.y - 51;
                    var posX = answerParent.GetComponent<RectTransform>().position.x;
                    option.transform.position = new Vector2(posX, posY);
                }
                option.GetComponent<DialogueOptions>().description.text = "Back";
                option.GetComponent<DialogueOptions>().type = OptionType.exit;
                playerAnswers.Add(option.GetComponent<DialogueOptions>());
            }
        }
    }
    public bool IsFirstAnswerBox(GameObject answerBox)
    {
        if(answerBoxesList.IndexOf(answerBox) == 0)
        {
            return true;
        }
        return false;
    }

    public void DeleteEmptyOption(OptionType type, GameObject emptyBox)
    {
        foreach (DialogueOptions answer in answerBoxesList[0].GetComponentsInChildren<DialogueOptions>())
        {
            if (answer.type == type)
            {
                UpdateAnswersPositions(answer, new List<DialogueOptions>(answerBoxesList[0].GetComponentsInChildren<DialogueOptions>()));
                Destroy(answer.gameObject);
            }
        }
        Destroy(emptyBox);
        answerBoxesList.Remove(emptyBox);
    }
    public void OpenAnswerBox(OptionType type)
    {
        foreach (GameObject box in answerBoxesList)
        {
            if(answerBoxesList.IndexOf(box) != 0 && box.GetComponent<AnswerBox>().type == type)
            {
                box.SetActive(false);
            }
        }
    }
    public void CloseAnswerBox(OptionType type)
    {
        foreach (GameObject box in answerBoxesList)
        {
            if (answerBoxesList.IndexOf(box) != 0 && box.GetComponent<AnswerBox>().type == type)
            {
                box.SetActive(true);
            }
        }
    }
    public void ResetAllBoxes()
    {
        foreach (DialogueOptions option in answerBoxesList[0].GetComponentsInChildren<DialogueOptions>())
        {
            Destroy(option.gameObject);
        }

        foreach (GameObject box in answerBoxesList)
        {
            if (answerBoxesList.IndexOf(box) != 0)
            {
                Destroy(box);
            }
        }
    }
    public void CheckIsEmptyBox()
    {
        if(currentManager.currentAnswerBox.GetComponent<AnswerBox>().IsEmpty() && currentManager != null)
        {
            currentManager.BackToPreviousAnswerBox();
        }
    }
    public bool IsAnswerBoxExist(OptionType boxType)
    {
        int value = 0;

        if (boxType == OptionType.quest)
        {
            foreach (GameObject box in answerBoxesList)
            {
                if (box.GetComponent<AnswerBox>().type == boxType)
                {
                    return true;
                }
            }
        }
        else if(boxType == OptionType.talk)
        {
            foreach (GameObject box in answerBoxesList)
            {
                if (box.GetComponent<AnswerBox>().type == boxType)
                {
                    value++;
                }
            }
            if(value > 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        return false;
    }
    public void ReplaceExitOptionOnQuestDecision(string dialogue)
    {
        foreach (DialogueOptions option in playerAnswers)
        {
            if(option.type == OptionType.exit)
            {
                option.type = OptionType.quest;
                option.description.text = dialogue;
            }
        }
    }
}
