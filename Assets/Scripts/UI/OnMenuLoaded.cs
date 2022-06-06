using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMenuLoaded : MonoBehaviour
{
    public GameController controller;
    private void Awake()
    {
        if(GameController.instance == null)
        {
            Instantiate(controller);
        }
        else
        {
            controller = FindObjectOfType<GameController>();
        }
        GameController.instance.HideGUI();
    }
}
