using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointController : MonoBehaviour
{
    public Transform player;
    public List<SpawnPoint> spawnPoints;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
        spawnPoints = new List<SpawnPoint>(GetComponentsInChildren<SpawnPoint>());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            player.position = spawnPoints[0].GetSpawnPoint();
        }
        else if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            player.position = spawnPoints[1].GetSpawnPoint();
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            player.position = spawnPoints[2].GetSpawnPoint();
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            player.position = spawnPoints[3].GetSpawnPoint();
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            player.position = spawnPoints[4].GetSpawnPoint();
        }
    }
}
