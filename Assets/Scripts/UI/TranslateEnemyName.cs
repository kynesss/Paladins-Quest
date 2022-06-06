using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Lanugage
{
    English,
    Polish
}

public class TranslateEnemyName : MonoBehaviour
{

    private TranslateController translateController;
    public TextMesh enemyName;
    public string english;
    public string polish;

    private void Awake()
    {
        enemyName = GetComponent<TextMesh>();
        translateController = FindObjectOfType<TranslateController>();
    }

    private void OnEnable()
    {
        translateController.onLanguageChanged.AddListener(SetTranslateText);
    }

    private void OnDisable()
    {
        translateController.onLanguageChanged.RemoveListener(SetTranslateText);
    }
    public void SetTranslateText()
    {
        switch(translateController.currentLanguage)
        {
            case Language.English:
                enemyName.text = english;
                break;
            case Language.Polish:
                enemyName.text = polish;
                break;
        }
    }
}


