using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    PlayerController player;
    PlayerData currentData;

    public List<EnemyController> enemyList;
    private void Awake()
    {
        if(instance != null)
        {
            return;
        }
        instance = this;
    }
    public void SaveGame()
    {
        player = FindObjectOfType<PlayerController>();
        SaveSystem.SavePlayer(new PlayerData(player));
    }

    public void CreateNewSaveFile(string name)
    {
        if(name.Length > 2)
        {
            SaveSystem.SavePlayer(new PlayerData(name));
            StartCoroutine(NewGameAsync(name));
        }
    }

    private IEnumerator NewGameAsync(string name)
    {
        SceneManager.LoadSceneAsync(1);

        while(SceneManager.GetActiveScene().buildIndex == 0)
        {
            yield return null;
        }
        LoadGame(name);
    }

    public void LoadGame(string name)
    {
        StartCoroutine(LoadGameAsync(name));
    }

    private IEnumerator LoadGameAsync(string name)
    {
        PlayerData data = SaveSystem.LoadPlayer(name);
        SceneManager.LoadSceneAsync(data.map);
        currentData = data;

        while(SceneManager.GetActiveScene().buildIndex == 0 || GameController.instance == null)
        {
            yield return null;
        }
        PlayerController player = FindObjectOfType<PlayerController>();
        player.stats.Name = data.Name;
        player.stats.Level = data.Level;
        player.stats.Experience = data.Experience;
        player.stats.Health = data.Health;
        player.stats.Mana = data.Mana;
        Inventory.instance.LoadItems(data.inventory);
        Equipment.instance.LoadItems(data.equipment);
        player.SetPosition(new Vector3(data.position[0], data.position[1], data.position[2]));
        player.SetRotation(data.rotation[0], data.rotation[1], data.rotation[2]);
  
        yield return null;
    }
    public IEnumerator SetEnemyPositions()
    {
        enemyList = GameController.instance.enemies;

        while(enemyList.Count < 0)
        {
            yield return null;
        }

        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].SetPosition(new Vector3(currentData.enemyPositionX[i], currentData.enemyPositionY[i], currentData.enemyPositionZ[i]));
            enemyList[i].SetRotation(currentData.enemyRotationX[i], currentData.enemyRotationY[i], currentData.enemyRotationZ[i]);
        }
    }
    public void LoadQuests()
    {

    }
}
