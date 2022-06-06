using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TranslateText : MonoBehaviour
{
    private TranslateController translateController;
    private Text ui_text;
    public string english;
    public string polish;

    private void Awake()
    {
        translateController = FindObjectOfType<TranslateController>();
        ui_text = GetComponent<Text>();
    }

    private void OnEnable()
    {
        SetTranslateText();
        translateController.onLanguageChanged.AddListener(SetTranslateText);
    }
    private void OnDisable()
    {
        translateController.onLanguageChanged.RemoveListener(SetTranslateText);
    }

    public void SetTranslateText()
    {
        switch (translateController.currentLanguage)
        {
            case Language.English:
                ui_text.text = english;
                break;
            case Language.Polish:
                ui_text.text = polish;
                break;
        }
    }
}
