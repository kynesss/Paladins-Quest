using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public static MenuController instance;
    public List<GameObject> panels;
    public InputField StartName;
    public InputField LoadName;

    public bool isCreatingNewFile;
    private void Awake()
    {
        if(instance != null)
        {
            return;
        }
        instance = this;
    }

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void StartNewGame()
    {
        isCreatingNewFile = true;
        SaveManager.instance.CreateNewSaveFile(StartName.text);
    }

    public void LoadGame()
    {
        isCreatingNewFile = false;
        SaveManager.instance.LoadGame(LoadName.text);
    }

    public void OpenPanel(GameObject panel)
    {
        foreach (var item in panels)
        {
            item.SetActive(false);

            if(item == panel)
            {
                item.SetActive(true);
            }
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
