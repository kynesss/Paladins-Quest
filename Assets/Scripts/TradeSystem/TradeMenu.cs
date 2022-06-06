using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TradeMenu : MonoBehaviour
{
    public static TradeMenu instance;

    public List<TradeAnswer> tradeAnswers;

    public TradeAnswer repairAnswer;

    public TradeAnswer craftAnswer;

    public GameObject playerTalkPanel;

    public GameObject traderTalkPanel;

    public GameObject tradePanel;

    public GameObject goldPanel;

    public GameObject exitConfirmPanel;

    public GameObject weaponFix;

    public Button exitBtn;

    private PlayerController player;

    public bool startTrading = false;
    public bool endTrading = false;

    public UnityEvent onPlayerTrade;
    public UnityEvent onOptionsAdded;

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
    }
    public void OpenPanel()
    {
        playerTalkPanel.SetActive(true);
    }
    public void ClosePanel()
    {
        playerTalkPanel.SetActive(false);
        tradeAnswers.Clear();
    }
    public void OpenTradePanels()
    {
        tradePanel.SetActive(true);
        goldPanel.SetActive(true);
        exitBtn.gameObject.SetActive(true);
        startTrading = true;
    }
    public void CloseTradePanels()
    {
        tradePanel.SetActive(false);
        goldPanel.SetActive(false);
        exitBtn.gameObject.SetActive(false);
        endTrading = true;
    }
    public void OpenFixPanels()
    {
        InventoryUI.instance.TurnOffButtons();
        weaponFix.SetActive(true);
        startTrading = true;
        player.isFixingWeapon = true;
    }
    public void CloseFixPanels()
    {
        InventoryUI.instance.TurnOnButtons();
        weaponFix.SetActive(false);
        endTrading = true;
        player.isFixingWeapon = false;
    }
    public IEnumerator Trade()
    {
        ClosePanel();
        StartCoroutine(SetDialogues(AnswerType.Trade));
        yield return new WaitForSeconds(1.5f);
        onPlayerTrade.Invoke();
        OpenTradePanels();
        player.SetPlayerTradeMode();
    }
    public IEnumerator FixWeaponMenu()
    {
        ClosePanel();
        StartCoroutine(SetDialogues(AnswerType.Repair));
        yield return new WaitForSeconds(1.5f);
        OpenFixPanels();
    }
    public IEnumerator SetDialogues(AnswerType answerType)
    {
        string dialogue = "Wybierz sobie!";

        if (answerType == AnswerType.Repair || answerType == AnswerType.Crafting)
        {
            dialogue = "Proszê bardzo!";
        }

        traderTalkPanel.GetComponentInChildren<TradeAnswer>().content.text = dialogue;
        traderTalkPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        traderTalkPanel.SetActive(false);
    }
    public void CloseTrade()
    {
        CloseTradePanels();
        player.ExitPlayerTradeMode();
        CloseConfirmPanel();
        TradeController.instance.ClearSlots();
        TradeController.instance.currentTrader.isTrading = false;
        TradeController.instance.currentTrader = null;
    }
    public void CloseWeaponFix()
    {
        CloseFixPanels();
        player.ExitPlayerTradeMode();
        TradeController.instance.currentTrader.isTrading = false;
        TradeController.instance.currentTrader = null;
    }
    public void OpenConfirmPanel()
    {
        exitConfirmPanel.SetActive(true);
    }
    public void CloseConfirmPanel()
    {
        exitConfirmPanel.SetActive(false);
    }
    public void SetOptions(Trader traderModel)
    {
        tradeAnswers.Clear();

        if(traderModel.trader.type == TraderType.Food)
        {
            repairAnswer.gameObject.SetActive(false);
            craftAnswer.gameObject.SetActive(false);
            tradeAnswers = new List<TradeAnswer>(playerTalkPanel.GetComponentsInChildren<TradeAnswer>());
            SetAnswerPosition(traderModel.trader.type);
            onOptionsAdded?.Invoke();
        }
        else if(traderModel.trader.type == TraderType.Weapon)
        {
            repairAnswer.gameObject.SetActive(true);
            craftAnswer.gameObject.SetActive(true);
            tradeAnswers = new List<TradeAnswer>(playerTalkPanel.GetComponentsInChildren<TradeAnswer>());
            SetAnswerPosition(traderModel.trader.type);
            onOptionsAdded?.Invoke();
        }
    }

    private void SetAnswerPosition(TraderType type)
    {
        foreach (TradeAnswer a in tradeAnswers)
        {
            if (a.type == AnswerType.Exit && type == TraderType.Food)
            {
                a.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 50f);
            }
            else if(a.type == AnswerType.Exit && type == TraderType.Weapon)
            {
                a.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -50f);
            }
        }
    }
}
