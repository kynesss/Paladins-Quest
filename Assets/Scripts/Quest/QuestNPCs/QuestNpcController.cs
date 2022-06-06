using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class QuestNpcController : MonoBehaviour
{
    public QuestNpcStats stats;

    [SerializeField]
    private GameObject healthCanvas;

    [SerializeField]
    private GameObject weaponSlot;

    private DialogueManager dialogueManager;
    private QuestGiver questGiver;
    private NPCController npcController;
    private PlayerController player;
    private NavMeshAgent agent;
    private Animator anim;
    public List<GameObject> enemies = null;

    public Dictionary<float, GameObject> distDictionary = new Dictionary<float, GameObject>();
    
    private GameObject enemyTarget = null;

    private Rigidbody rb;
    public Rigidbody[] rigidbodies;
    private Collider[] colliders;
    public Collider[] otherColliders;
    private CapsuleCollider capsuleCollider;

    private bool isAproachingPlayer = false;
    public bool isPlayerInRange = false;
    public bool isAlive = true;

    private bool isApproachingEnemy = false;
    public bool isEnemyInRange = false;

    private bool isAttacking = false;
    private float startPosX;
    private float startPosY;
    private float startPosZ;
    private Vector3 startPosition;
    [SerializeField]
    private GameObject dungeonDoor;

    public bool isInterractingWithPlayer = false;
    private bool isInfected = false;
    private float infectionTimer = 10.0f;

    private void Awake()
    {
        SetStartPosition();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        questGiver = GetComponent<QuestGiver>();
        dialogueManager = GetComponent<DialogueManager>();
        npcController = GetComponent<NPCController>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
        player = FindObjectOfType<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        

        foreach (Rigidbody rig in rigidbodies)
        {
            rig.isKinematic = true;
        }
        rb.isKinematic = false;

        foreach (Collider c in colliders)
        {
            c.enabled = false;
        }
        foreach (Collider o in otherColliders)
        {
            o.enabled = true;
        }

        stats.Health = stats.MaxHealth;
    }

    private void Update()
    {
        if (isAlive)
        {
            SetHealthCanvasRotation();
        }
        else
        {
            healthCanvas.gameObject.SetActive(false);
            npcController.text3DName.SetActive(false);
        }

        if (isPlayerInRange && !isEnemyInRange && !questGiver.isKillQuestFinished && questGiver.isKillQuestAccepted)
        {
            FollowPlayer();
        }
        if(isEnemyInRange)
        {
            FollowEnemy();
        }
        if(enemies.Count > 0)
        {
            isEnemyInRange = true;
        }
        else
        {
            isEnemyInRange = false;
            enemyTarget = null;
        }
        if(isApproachingEnemy && enemyTarget != null && enemyTarget.GetComponent<EnemyController>().isAlive && isAlive)
        {
            StartCoroutine(AttackTarget(enemyTarget));
        }
        else
        {
            StopAttack();
        }
        if(isAproachingPlayer)
        {
            SetFollowPlayerAnimation();
        }
        if(isAttacking)
        {
            FaceTarget();
        }
        if(questGiver.isKillQuestFinished && !dialogueManager.isTalking && transform.position != startPosition)
        {
            GoBackToStartPosition();
        }
        else if(transform.position == startPosition)
        {
            anim.SetBool("isWalking", false);
        }
        if(questGiver.isKillQuestAccepted)
        {
            dungeonDoor.GetComponent<BoxCollider>().enabled = false;
        }
        if(questGiver.canAttackPlayer && player.isAlive && !dialogueManager.isTalking && isAlive)
        {
            StartCoroutine(AttackTarget(player.gameObject));
        }
        if(isPlayerInRange && !isInterractingWithPlayer && npcController.npc._name == "Bandyta")
        {
            InterractWithPlayer();
        }
        if(isInfected)
        {
            Infection();
        }
    }
    private void FaceTarget()
    {
        Vector3 lookDir = enemyTarget.transform.position - transform.position;
        lookDir.y = 0f;
        Quaternion rot = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 0.1f);
    }
    private void SetFollowPlayerAnimation()
    {
        if (Vector3.Distance(transform.position, player.transform.position) >= 3.0f)
        {
            if (!player.movement.isRunning && !player.movement.isWalking)
            {
                agent.speed = 5.0f;
                anim.SetBool("isWalking", true);
                anim.SetBool("isRunning", false);
            }
            else if (player.movement.isWalking)
            {
                agent.speed = 5.0f;
                anim.SetBool("isWalking", player.movement.isWalking);
            }
            else if (player.movement.isRunning)
            {
                agent.speed = 7.0f;
                anim.SetBool("isRunning", player.movement.isRunning);
            }
        }
        else
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", false);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("EnemyWeapon"))
        {
            GetHit(collision.gameObject.GetComponentInParent<EnemyController>().stats.Attack);
        }
        if(collision.gameObject.CompareTag("Weapon") && questGiver.canAttackPlayer)
        {
            GetHit(player.stats.Attack);

            if (collision.gameObject.GetComponent<CurrentWeapon>().isWeaponInfected)
            {
                isInfected = true;
            }
        }
    }
    private void FollowPlayer()
    {
        if(player.isAlive && !isAproachingPlayer)
        {
            isAproachingPlayer = true;
        }
        if(isAproachingPlayer && Vector3.Distance(transform.position, player.transform.position) >= 3.0f)
        {
            agent.SetDestination(player.transform.position);
            agent.isStopped = false;
            LookAtPlayer(player.transform);
        }
        if(isAproachingPlayer && Vector3.Distance(transform.position, player.transform.position) < 3.0f)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }
    }

    private void FollowEnemy()
    {
        enemyTarget = CountEnemyDistance(enemies);

        if(enemyTarget != null && enemyTarget.GetComponent<EnemyController>().isAlive)
        {
            isApproachingEnemy = true;
            agent.SetDestination(enemyTarget.transform.position);
        }
        else
        {
            enemies.Remove(enemyTarget);
            isApproachingEnemy = false;
            agent.ResetPath();
        }
    }
    private IEnumerator AttackTarget(GameObject target)
    {
        if (!weaponSlot.activeInHierarchy)
            weaponSlot.SetActive(true);

        if(target == player.gameObject)
        {
            yield return new WaitForSeconds(1.5f);
            agent.SetDestination(target.transform.position);
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        if (Vector3.Distance(transform.position, target.transform.position) < 2.0f) 
        {
            anim.SetBool("isRunning", false);
            anim.SetBool("isWalking", true);
            anim.SetBool("isAttacking", true);
            isAttacking = true;
        }
        else
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isAttacking", false);
            anim.SetBool("isRunning", true);
            isAttacking = false;
        }
    }
    private void StopAttack()
    {
        weaponSlot.SetActive(false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isWalking", false);
        isAttacking = false;
    }
    private GameObject CountEnemyDistance(List<GameObject> enemies)
    {
        GameObject target = null;
        distDictionary.Clear();

        foreach (GameObject e in enemies)
        {
            float dist = Vector3.Distance(transform.position, e.transform.position);

            if(distDictionary.ContainsKey(dist)) 
            {
                distDictionary.Remove(dist);
            }
            else 
            {
                distDictionary.Add(dist, e);
            }
        }
        List<float> distance = distDictionary.Keys.ToList();

        distance.Sort();

        foreach (var d in distDictionary) 
        {
            if(distance[0] == d.Key)
            {
                target = d.Value;
            }
        }

        return target;
    }
    public void GetHit(float dmg)
    {
        stats.Health -= (dmg - stats.Defence);

        if(stats.Health <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        isAlive = false;
        isAttacking = false;
        rb.isKinematic = true;
        //agent.enabled = false;
        npcController.enabled = false;
        RagdollDeath();

        if(npcController.npc._name == "Martin")
        {
            dungeonDoor.GetComponent<BoxCollider>().enabled = false;
        }
        Invoke("DestroyBody", 10f);
    }
    private void DestroyBody()
    {
        Destroy(gameObject);
    }
    public void RagdollDeath()
    {
        anim.enabled = false;
        if (rb != null)
            rb.constraints = RigidbodyConstraints.FreezeAll;

        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }

        foreach (Rigidbody rig in rigidbodies)
        {
            rig.isKinematic = false;
            rig.useGravity = true;
        }

        capsuleCollider.enabled = false;
    }
    public void LookAtPlayer(Transform target)
    {
        Vector3 lookDirection = target.position - transform.position;
        lookDirection.y = 0f;
        Quaternion rotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1.0f * Time.deltaTime);
    }
    private void SetHealthCanvasRotation()
    {
        if((healthCanvas.transform.eulerAngles.y != npcController.text3DName.transform.eulerAngles.y))
        {
            healthCanvas.transform.rotation = Quaternion.Euler(0f, npcController.text3DName.transform.eulerAngles.y, 0f);
        }
    }
    public void enableWeapon()
    {
        weaponSlot.GetComponent<BoxCollider>().enabled = true;
    }
    public void disableWeapon()
    {
        weaponSlot.GetComponent<BoxCollider>().enabled = false;
    }
    private void SetStartPosition()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        startPosZ = transform.position.z;

        PlayerPrefs.SetFloat("posX", startPosX);
        PlayerPrefs.SetFloat("posY", startPosY);
        PlayerPrefs.SetFloat("posZ", startPosZ);

        startPosition = new Vector3(PlayerPrefs.GetFloat("posX"), PlayerPrefs.GetFloat("posY"), PlayerPrefs.GetFloat("posZ"));
    }
    private void GoBackToStartPosition()
    {
        agent.SetDestination(startPosition);
        anim.SetBool("isWalking", true);
    }
    public void InterractWithPlayer()
    {
        if(Vector3.Distance(transform.position, player.transform.position) < 3.0f)
        {
            StartCoroutine(dialogueManager.NpcDialogueInterract());
            isInterractingWithPlayer = true;
        }
    }
    private void Infection()
    {
        stats.Health -= 0.05f;
        infectionTimer -= Time.deltaTime;

        healthCanvas.GetComponentInChildren<QuestNpcHealthBar>().healthImage.color = Color.green;
        if (infectionTimer <= 0)
        {
            isInfected = false;
            infectionTimer = 10.0f;
            healthCanvas.GetComponentInChildren<QuestNpcHealthBar>().healthImage.color = Color.red;
        }

        if (stats.Health <= 0)
        {
            Die();
        }
    }
}
