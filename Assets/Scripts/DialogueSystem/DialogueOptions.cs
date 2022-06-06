using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueOptions : MonoBehaviour
{
    public Text description;

    public bool isChosen = false;

    public AudioClip voice;

    public float waitTime;

    public OptionType type;

    public void SetType(int newType)
    {
        type = (OptionType)newType;
    }

    private void OnDestroy()
    {
        isChosen = false;
        DialogueMenu.instance.playerAnswers.Remove(this);
        
        if(DialogueMenu.instance.currentManager != null)
        DialogueMenu.instance.currentManager.currentOptions.Remove(this);
    }
}
public enum OptionType
{
    talk, quest, trade, exit
}