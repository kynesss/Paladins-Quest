using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSceneLoaded : MonoBehaviour
{
    public GameController controller;
    private void Awake()
    {
        if (GameController.instance == null)
        {
            Instantiate(controller);
        }

        GameController.instance.RespawnPlayer();
        GameController.instance.ShowGUI();
        GameController.instance.chest = GameObject.FindGameObjectsWithTag("Chest");
        StartCoroutine(GameController.instance.WaitForEnemies());
    }
}
