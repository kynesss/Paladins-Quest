using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderData : MonoBehaviour
{
    public EnemyMovement enemy;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            enemy.isPlayerTargetInReach = true;
            enemy.playerTarget = other.gameObject;
        }
        if (other.CompareTag("NPC"))
        {
            enemy.isNPCTargetInReach = true;
            enemy.npcTarget = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.isPlayerTargetInReach = false;
            enemy.playerTarget = null;
            enemy.isApproachingPlayer = false;
        }
        if (other.CompareTag("NPC"))
        {
            enemy.isNPCTargetInReach = false;
            enemy.npcTarget = null;
            enemy.isAppoachingNPC = false;
        }
        if(!enemy.isPlayerTargetInReach && !enemy.isNPCTargetInReach)
        {
            enemy.GoBackToStartingPosition();
        }
    }
}
