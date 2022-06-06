using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    //Transform player;
    Animator anim;
    
    public GameObject playerTarget;
    public GameObject npcTarget;
    public GameObject target;

    public float distanceFromPlayer;
    public float distanceFromNPC;
    Vector3 startingPosition;


    public bool isApproachingPlayer = false;
    public bool isPlayerTargetInReach = false;

    public bool isAppoachingNPC = false;
    public bool isNPCTargetInReach = false;

    public bool isAlive = true;

    public float roamingDistanceX = 3.0f;
    public float roamingDistanceZ = 3.0f;
    public float roamingDelay = 20.0f;
    public float roamingTimer;
    private float distanceValue;

    void Start()
    {
        anim = GetComponent<Animator>();
        startingPosition = transform.position;
        roamingTimer = Random.Range(0f, roamingDelay);
    }

    void Update()
    {
        if(isAlive && (isPlayerTargetInReach || isNPCTargetInReach))
        {
            FollowTarget();
        }
        if(isAlive)
        {
            anim.SetBool("isWalking", agent.hasPath);
        }
        Roam();
    }

    public void Roam()
    {
        if(isAlive && !isApproachingPlayer && !isAppoachingNPC)
        {
            roamingTimer -= Time.deltaTime;

            if(roamingTimer <= 0)
            {
                agent.SetDestination(startingPosition + new Vector3(Random.Range(-roamingDistanceX, roamingDistanceX), 0f, Random.Range(-roamingDistanceZ, roamingDistanceZ)));
                roamingTimer = roamingDelay;
            }
        }
    }

    public void Die()
    {
        isAlive = false;
        enabled = false;
        agent.enabled = false;
    }

    public void GoBackToStartingPosition()
    {
        if(isAlive)
        {
            isApproachingPlayer = false;
            isAppoachingNPC = false;
            agent.SetDestination(startingPosition);
        }
    }

    public void FollowTarget()
    {
        float playerDistance = CheckTargetDistance(playerTarget, distanceFromPlayer);
        float npcDistance = CheckTargetDistance(npcTarget, distanceFromNPC);

        if (playerDistance < npcDistance)
        {
            if(isAppoachingNPC)
            {
                isAppoachingNPC = false;
            }
            
            if (!isApproachingPlayer && !isAppoachingNPC && playerTarget.GetComponent<PlayerController>().isAlive)
            {
                isApproachingPlayer = true;
            }

            if (!playerTarget.GetComponent<PlayerController>().isAlive)
            {
                isPlayerTargetInReach = false;
                GoBackToStartingPosition();
            }

            if (playerTarget != null && playerDistance > 1.0f)
            {
                agent.isStopped = false;
                agent.SetDestination(playerTarget.transform.position);
            }
            else
            {
                agent.isStopped = true;
                agent.ResetPath();
            }
        }
        else if(npcDistance < playerDistance)
        {
            if(isApproachingPlayer)
            {
                isApproachingPlayer = false;
            }
            
            if (!isApproachingPlayer && !isAppoachingNPC && npcTarget.GetComponent<QuestNpcController>().isAlive)
            {
                isAppoachingNPC = true;
            }

            if (!npcTarget.GetComponent<QuestNpcController>().isAlive)
            {
                isNPCTargetInReach = false;
                GoBackToStartingPosition();
            }

            if (npcTarget != null && npcDistance > 1.0f)
            {
                agent.isStopped = false;
                agent.SetDestination(npcTarget.gameObject.transform.position);
            }
            else
            {
                agent.isStopped = true;
                agent.ResetPath();
            }
        }
    }
    private float CheckTargetDistance(GameObject target, float distance)
    {
        if(target != null)
        {
            distance = Vector3.Distance(transform.position, target.transform.position);
        }
        else
        {
            distance = 100f;
        }

        return distance;
    }
    public void SetPosition(Vector3 pos)
    {
        agent.Warp(pos);
    }
    public void SetRotation(float x, float y, float z)
    {
        transform.rotation = Quaternion.Euler(x, y, z);
    }
}
