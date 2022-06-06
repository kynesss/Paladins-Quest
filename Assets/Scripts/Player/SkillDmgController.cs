using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDmgController : MonoBehaviour
{
    public EnemyController enemy;
    public QuestNpcController npc;
    PlayerController player;

    private void Start()
    {
        enemy = FindObjectOfType<EnemyController>();
        player = FindObjectOfType<PlayerController>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if(other.CompareTag("Enemy"))
        {
            enemy = other.GetComponent<EnemyController>();
            if(enemy != null && enemy.isAlive)
            enemy.GetHit(player.stats.MagicAttack);
        }
        if(other.CompareTag("NPC") && other.GetComponent<QuestGiver>().canAttackPlayer)
        {
            npc = other.GetComponent<QuestNpcController>();

            if(npc != null)
            {
                npc.GetHit(player.stats.MagicAttack);
            }
        }
    }


}
