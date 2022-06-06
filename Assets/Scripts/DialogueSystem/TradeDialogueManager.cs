using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeDialogueManager : MonoBehaviour
{
    public List<TradeAnswer> options;
    private TradeMenu tradeMenu;
    public int i = 0;

    private void Start()
    {
        tradeMenu = TradeMenu.instance;
        tradeMenu.onOptionsAdded.AddListener(SetCurrentOptions);
    }

    public void ChooseOption()
    {
        AddColorToOption();

        if(options[i] != null)
            options[i].isChosen = true;

        if(i == 0 && options[options.Count - 1].isChosen)
        {
            options[options.Count - 1].isChosen = false;
        }
        if(Input.GetKeyDown(KeyCode.DownArrow) && i < options.Count - 1)
        {
            options[i].isChosen = false;
            i++;
            options[i].isChosen = true;
        }
        if(Input.GetKeyDown(KeyCode.UpArrow) && i > 0)
        {
            options[i].isChosen = false;
            i--;
            options[i].isChosen = true;
        }
        if(options[i].isChosen && Input.GetKeyDown(KeyCode.Return) && options[i].type == AnswerType.Trade)
        {
            StartCoroutine(TradeMenu.instance.Trade());
        }
        else if(options[i].isChosen && Input.GetKeyDown(KeyCode.Return) && options[i].type == AnswerType.Repair)
        {
            StartCoroutine(TradeMenu.instance.FixWeaponMenu());
        }
        else if(options[i].isChosen && Input.GetKeyDown(KeyCode.Return) && options[i].type == AnswerType.Exit)
        {
            i = 0;
            TradeController.instance.currentTrader.isTrading = false;
            TradeController.instance.ResetCurrentTrader();
            TradeMenu.instance.ClosePanel();
        }
    }
    private void AddColorToOption()
    {
        foreach (TradeAnswer option in options)
        {
            if (option.isChosen)
            {
                option.content.color = Color.red;
            }
            else
            {
                option.content.color = Color.white;
            }
        }
    }

    private void SetCurrentOptions()
    {
        options = tradeMenu.tradeAnswers;
    }
}
