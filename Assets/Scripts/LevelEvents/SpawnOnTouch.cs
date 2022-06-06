using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnTouch : MonoBehaviour
{
    public GameObject gameObjectSpawn;

    private void OnTriggerEnter(Collider other)
    {
        gameObjectSpawn.SetActive(true);
    }
}
