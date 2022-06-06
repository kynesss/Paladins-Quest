using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeAnswer : MonoBehaviour
{
    public Text content;
    
    public AnswerType type;

    public bool isChosen = false;
}
public enum AnswerType
{
    Trade, Repair, Crafting, Exit
}