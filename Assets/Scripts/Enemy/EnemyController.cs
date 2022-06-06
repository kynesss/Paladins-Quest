using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;


public class EnemyController : MonoBehaviour
{
    private Animator anim;
    private NavMeshAgent agent;
    public EnemyStats stats;
    public EnemyMovement movement;
    private Rigidbody rb;
    private GameObject target;
    private PlayerController player;

    public BoxCollider swordCollider;
    public BoxCollider secondWeaponCollider;
    private Collider[] colliders;
    private CapsuleCollider capsuleCollider;
    private Rigidbody[] rigidbodies;

    public Rigidbody[] otherRigidbodies;
    public Collider[] otherColliders;

    public GameObject textName;
    public GameObject healthCanvas;

    public ItemPickUp loot;

    public bool isAttacking = false;
    public bool isAlive = true;
    private bool isGettingHit = false;
    private bool isInfected = false;
    private float infectionTimer = 10.0f;


    [SerializeField] private float distanceValue;


    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        movement = GetComponent<EnemyMovement>();
        rb = GetComponent<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
        capsuleCollider = GetComponentInChildren<CapsuleCollider>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        player = FindObjectOfType<PlayerController>();

        disableSword();

        stats.Health = 100;

        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }

        foreach (Rigidbody rig in rigidbodies)
        {
            rig.isKinematic = true;
        }
        foreach (Rigidbody rb in otherRigidbodies)
        {
            rb.isKinematic = false;
        }
        foreach (Collider collider in otherColliders)
        {
            collider.enabled = true;
        }

        rb.isKinematic = false;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1 && Camera.main != null)
            NameFollowCamera();

        if (movement.isApproachingPlayer && !movement.isAppoachingNPC && isAlive)
        {
            target = movement.playerTarget;

            if (CanEnemyAtack() && target.GetComponent<PlayerController>().isAlive)
            {
                Attack();
            }
            else if (!CanEnemyAtack() || !target.GetComponent<PlayerController>().isAlive)
            {
                StopAttack();
            }
        }
        else if(movement.isAppoachingNPC && !movement.isApproachingPlayer && isAlive)
        {
            target = movement.npcTarget;
            
            if (CanEnemyAtack() && target.GetComponent<QuestNpcController>().isAlive)
            {
                Attack();
            }
            else if (!CanEnemyAtack() || !target.GetComponent<QuestNpcController>().isAlive)
            {
                StopAttack();
            }
        }
        if(isInfected)
        {
            Infection();
        }
    }
    private bool CanEnemyAtack()
    {
        return Vector3.Distance(transform.position, target.transform.position) < distanceValue;
    }
    private void Attack()
    {
        isAttacking = true;
        anim.SetBool("isAttacking", isAttacking);
    }
    private void StopAttack()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", isAttacking);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Weapon") && isAlive)
        {
            if (target == movement.playerTarget)
            {
                GetHit(target.GetComponent<PlayerController>().stats.Attack);
            }
            if(collision.gameObject.GetComponent<CurrentWeapon>().isWeaponInfected)
            {
                isInfected = true;
            }
        }
        else if(collision.gameObject.CompareTag("NPCWeapon") && isAlive)
        {
            if (target == movement.npcTarget)
                GetHit(target.GetComponent<QuestNpcController>().stats.Attack);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
            isGettingHit = false;
    }
    public void GetHit(float dmg)
    {
        stats.Health -= (dmg - stats.Defence);
        isGettingHit = true;

        if (stats.Health <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        player.stats.Experience += stats.Experience;
        movement.Die();
        enabled = false;
        agent.enabled = false;
        isAlive = false;
        healthCanvas.SetActive(false);
        StopAttack();
        RagdollDeath();
        rb.isKinematic = true;
        textName.SetActive(false);
        swordCollider.enabled = false;
        DropItem();
        QuestLog.MyInstance.OnEnemyKilled?.Invoke(this);
        Destroy(gameObject, 10f);
    }

    public void RagdollDeath()
    {
        if (stats.Name != "Crab Monster")
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
        else
        {
            anim.SetBool("isAlive", false);
        }
    }
    public void enableSword()
    {
        swordCollider.enabled = true;

        if(secondWeaponCollider != null)
        {
            secondWeaponCollider.enabled = true;
        }
    }

    public void disableSword()
    {
        swordCollider.enabled = false;

        if (secondWeaponCollider != null)
        {
            secondWeaponCollider.enabled = false;
        }
    }

    public void NameFollowCamera()
    {
        if (textName != null)
        {
            textName.transform.LookAt(Camera.main.transform.position);
            textName.transform.Rotate(0f, 180, 0f);

            if (movement.isApproachingPlayer)
            {
                textName.GetComponent<TextMesh>().color = Color.red;
            }
            else
            {
                textName.GetComponent<TextMesh>().color = Color.black;
            }
        }
    }

    public EnemyStats GetEnemyStats()
    {
        return stats;
    }

    public void SetPosition(Vector3 pos)
    {
        movement.SetPosition(pos);
    }
    public void SetRotation(float x, float y, float z)
    {
        movement.SetRotation(x, y, z);
    }
    public void DropItem()
    {
        Item newItem = DrawItem();
        if (newItem != null)
        {
            loot.item = newItem;
            ItemPickUp tempLoot = Instantiate(loot, transform.position, Quaternion.identity);

            if (tempLoot.GetComponent<MeshCollider>().sharedMesh != null)
            {
                tempLoot.GetComponent<MeshCollider>().sharedMesh = newItem.mesh.sharedMesh;
            }
        }
    }
    public ItemType DrawItemType()
    {
        int value = UnityEngine.Random.Range(1, 100);
        ItemType type;

        if (value >= 1 && value < 40)
        {
            type = ItemType.Food;
        }
        else if (value >= 40 && value < 70)
        {
            type = ItemType.Potion;
        }
        else if (value >= 70 && value < 85)
        {
            type = ItemType.Shield;
        }
        else
        {
            type = ItemType.Weapon;
        }

        return type;
    }
    public Item DrawItem()
    {
        Item dropItem = null;
        int drawIndex = 0;
        int drawValue = 0;
        int tabLen = 0;
        int y = 0;
        List<Item> listOfItems = ItemsDatabase.instance.items;
        var type = DrawItemType();

        for (int i = 0; i < listOfItems.Count; i++)
        {
            if (listOfItems[i].itemType == type)
            {
                tabLen++;
            }
        }
        int[] tempTab = new int[tabLen];

        for (int i = 0; i < listOfItems.Count; i++)
        {
            if (listOfItems[i].itemType == type)
            {
                tempTab[y] = i;
                y++;
            }
        }
        drawIndex = new System.Random().Next(tempTab.Length);

        for (int i = 0; i < tempTab.Length; i++)
        {
            if (drawIndex == i)
            {
                drawValue = tempTab[i];
            }
        }

        for (int i = 0; i < listOfItems.Count; i++)
        {
            if (drawValue == i)
            {
                dropItem = listOfItems[i];
            }
        }

        return dropItem;
    }
    private void Infection()
    {
        stats.Health -= 0.05f;
        infectionTimer -= Time.deltaTime;

        healthCanvas.GetComponent<EnemyHealthBar>().fillingHealthBar.color = Color.green;
        if(infectionTimer <= 0)
        {
            isInfected = false;
            infectionTimer = 10.0f;
            healthCanvas.GetComponent<EnemyHealthBar>().fillingHealthBar.color = Color.red;
        }

        if (stats.Health <= 0 && !isGettingHit)
        {
            Die();
        }
    }
}
