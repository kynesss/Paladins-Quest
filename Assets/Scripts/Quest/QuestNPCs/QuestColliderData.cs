using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestColliderData : MonoBehaviour
{
    public QuestNpcController questNpcController;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            questNpcController.isPlayerInRange = true;
        }
        if (other.CompareTag("Enemy"))
        {
            questNpcController.enemies.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            questNpcController.isPlayerInRange = false;
        }
        if (other.CompareTag("Enemy"))
        {
            questNpcController.enemies.Remove(other.gameObject);
        }
    }
}
