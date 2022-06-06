using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Language
{
    English,
    Polish
}

public class TranslateController : MonoBehaviour
{
    public UnityEvent onLanguageChanged;

    [HideInInspector]
    public Language currentLanguage = Language.English;
    private int currentLanguageID = 0;

    private void Awake()
    {
        LoadLanguage();
    }

    public void ChangeLanguage()
    {
        currentLanguageID++;
        if(currentLanguageID > 1)
        {
            currentLanguageID = 0;
        }

        SetCurrentLanguage();
        SaveLanguage();
    }

    public void SaveLanguage()
    {
        PlayerPrefs.SetInt("language", currentLanguageID);
    }

    public void LoadLanguage()
    {
        if (PlayerPrefs.HasKey("language"))
        {
            currentLanguageID = PlayerPrefs.GetInt("language");
            SetCurrentLanguage();
        }
    }
   
    public void SetCurrentLanguage()
    {
        switch(currentLanguageID)
        {
            case 0:
                currentLanguage = Language.English;
                break;
            case 1:
                currentLanguage = Language.Polish;
                break;
        }

        onLanguageChanged.Invoke();
    }
}
