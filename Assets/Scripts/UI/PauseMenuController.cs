using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public static PauseMenuController instance;
    public GameObject pauseMenu;
    [SerializeField] public List<GameObject> panels;

    [HideInInspector]
    public bool isPaused = false;

    private void Awake()
    {
        if(instance != null)
        {
            return;
        }
        instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ChangePauseMenuStatus();
        }
    }

    public void ChangePauseMenuStatus()
    {
        if(isPaused)
        {
            isPaused = false;
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
        else
        {
            isPaused = true;
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
        }
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

    public void BackToMenu()
    {
        ChangePauseMenuStatus();
        SceneManager.LoadScene(0);
    }
}
