using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerData 
{
    //Player variables
    public string Name;
    public int Level;
    public float Experience;
    public float Health;
    public float Mana;

    public List<int> inventory;
    public List<int> equipment;

    public int map;
    public float[] position;
    public float[] rotation;

    public List<float> enemyPositionX;
    public List<float> enemyPositionY;
    public List<float> enemyPositionZ;

    public List<float> enemyRotationX;
    public List<float> enemyRotationY;
    public List<float> enemyRotationZ;

    public PlayerData(PlayerController player)
    {
        Name = player.stats.Name;
        Level = player.stats.Level;
        Experience = player.stats.Experience;
        Health = player.stats.Health;
        Mana = player.stats.Mana;
        inventory = Inventory.instance.GetItemID();
        equipment = Equipment.instance.GetItemID();

        map = SceneManager.GetActiveScene().buildIndex;
        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

        enemyPositionX = GameController.instance.GetEnemyXpositions();
        enemyPositionY = GameController.instance.GetEnemyYpositions();
        enemyPositionZ = GameController.instance.GetEnemyZpositions();

        enemyRotationX = GameController.instance.GetEnemyXrotations();
        enemyRotationY = GameController.instance.GetEnemyYrotations();
        enemyRotationZ = GameController.instance.GetEnemyZrotations();

        rotation = new float[3];
        rotation[0] = player.transform.eulerAngles.x;
        rotation[1] = player.transform.eulerAngles.y;
        rotation[2] = player.transform.eulerAngles.z;
    }

    public PlayerData(string _name)
    {
        Name = _name;
        Level = 10;
        Experience = 0f;
        Health = 100f;
        Mana = 100f;
        inventory = new List<int>();
        equipment = new List<int>();
        map = 1;
        position = new float[3];
        rotation = new float[3];
        enemyPositionX = new List<float>();
        enemyPositionY = new List<float>();
        enemyPositionZ = new List<float>();
        enemyRotationX = new List<float>();
        enemyRotationY = new List<float>();
        enemyRotationZ = new List<float>();
    }
}
