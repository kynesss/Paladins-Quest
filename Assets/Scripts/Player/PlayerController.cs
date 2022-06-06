using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
public class PlayerController : MonoBehaviour
{
    public Animator anim;

    public PlayerMovement movement;

    public CameraController cameraController;

    public Rigidbody rb;

    public Stats stats;
    
    public ItemPickUp itemTarget;

    [HideInInspector] public bool isPickingUp = false;

    [HideInInspector] public bool isEquipped = false;

    [HideInInspector] public bool isEating = false;

    [HideInInspector] public bool isTrading = false;

    [HideInInspector] public bool isDrinking = false;

    [HideInInspector] public bool isAlive = true;

    [HideInInspector] public bool isUsingSkill = false;
    
     public bool isFixingWeapon = false;

    private float timer = 1.8f;

    [SerializeField] float rotateSpeed = 5.0f;

    public GameObject[] skills;

    public GameObject target;

    public GameObject leftHandSlot;

    private EnemyController enemy;

    public delegate void OnSkillsChanged(float magicDmg);
    public event OnSkillsChanged onSkillsChanged;

    public UnityEvent onPlayerAttacked;

    void Start()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        enemy = FindObjectOfType<EnemyController>();
        rb = GetComponent<Rigidbody>();

        for (int i = 0; i < skills.Length; i++)
        {
            skills[i].SetActive(false);
        }

        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (isEating)
            {
                Eat();
            }
            else if (isDrinking)
            {
                Drink();
            }
            else
            {
                Attack();
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (Equipment.instance.leftHandSlot.gameObject.activeInHierarchy) 
                StartCoroutine(Block());
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(PickUpItem());
        }
        if(isPickingUp)
        {
            FaceTarget();
        }
        if (!movement.isJumping && !PauseMenuController.instance.isPaused && !GameController.instance.isSkillTreeVisible && !GameController.instance.isInventoryVisible &&
            !GameController.instance.isQuestLogVisible && SceneManager.GetActiveScene().buildIndex == 1 && !GameController.instance.isSvCheatsOn)
        {
            CheckIfPlayerHasFallen();
        }
        if(isAlive)
            UseSkill();
    }
    private IEnumerator PickUpItem()
    {
       if(itemTarget != null)
       {
           if (itemTarget.CanPickUp)
           {
                movement.canMove = false;
                if(itemTarget.transform.position.y <= 0.8f)
                {
                    isPickingUp = true;
                    anim.SetTrigger("PickUpObject");
                    yield return new WaitForSeconds(0.8f);
                }
                else
                {
                    isPickingUp = true;
                    anim.SetTrigger("PickUpObject2");
                    yield return new WaitForSeconds(0.8f);
                }
                itemTarget.Interact();
                itemTarget = null;
                yield return new WaitForSeconds(0.5f);
                movement.canMove = true;
                isPickingUp = false;
            }
       }
       else
       {
            yield return null;
       }
    }

    private void FaceTarget()
    {
        if (itemTarget != null)
        {
            Vector3 direction = itemTarget.transform.position - transform.position;
            direction.y = 0f;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
        }
    }
    private void Attack()
    {
        if (!GameController.instance.isInventoryVisible && !GameController.instance.isQuestLogVisible && isAlive && !isTrading 
            && DialogueMenu.instance.currentManager == null && !PauseMenuController.instance.isPaused && !GameController.instance.isSkillTreeVisible)
        {
            if (!isEquipped && stats.Stamina > 1.0f) 
            {
                anim.SetBool("isEquipped", false);
                anim.SetTrigger("Attack");
                stats.Stamina -= 1.0f;
            }
            else if(isEquipped && stats.Stamina > 3.0f && CurrentWeapon.instance.item.weaponType == WeaponType.OneHanded)
            {
                anim.SetBool("isEquipped", true);
                anim.SetBool("isOneHandedWeapon", true);
                anim.SetBool("isTwoHandedWeapon", false);
                anim.SetTrigger("Attack");
                stats.Stamina -= 3.0f;
                onPlayerAttacked?.Invoke();
            }
            else if(isEquipped && stats.Stamina > 3.0f && CurrentWeapon.instance.item.weaponType == WeaponType.TwoHanded)
            {
                anim.SetBool("isEquipped", true);
                anim.SetBool("isOneHandedWeapon", false);
                anim.SetBool("isTwoHandedWeapon", true);
                anim.SetTrigger("Attack");
                stats.Stamina -= 3.0f;
                onPlayerAttacked?.Invoke();
            }

        }
    }

    private IEnumerator Block()
    {
        if(!GameController.instance.isInventoryVisible && Equipment.instance.rightHandSlot.gameObject.activeInHierarchy)
        {
            if(isEquipped)
            {
                CurrentShield.instance.boxCollider.enabled = true;
                anim.SetTrigger("ShieldBlock");
                yield return new WaitForSeconds(1.2f);
                CurrentShield.instance.boxCollider.enabled = false;
            }
        }
    }

    private void Eat()
    {
        if (!GameController.instance.isInventoryVisible)
        {
            anim.SetBool("isEating", isEating);
            anim.SetTrigger("Consume");
        }
    }

    private void Drink()
    {
        if (!GameController.instance.isInventoryVisible)
        {
            anim.SetBool("isDrinking", isDrinking);
            anim.SetTrigger("Consume");
        }
    }

    private void UseSkill()
    {
        int number = 0;
        if (Input.GetKeyDown(KeyCode.Alpha1) && SkillManager.instance.skills[0].isActivated) number = 1;
        if (Input.GetKeyDown(KeyCode.Alpha2) && SkillManager.instance.skills[4].isActivated) number = 2;
        if (Input.GetKeyDown(KeyCode.Alpha3) && SkillManager.instance.skills[1].isActivated) number = 3;
        if (Input.GetKeyDown(KeyCode.Alpha4) && SkillManager.instance.skills[3].isActivated) number = 4;

        switch (number)
        {
            case 1:
                if (stats.Mana >= 30f)
                {
                    stats.Mana -= 30f;
                    anim.SetFloat("SkillType", 0f);
                    anim.SetTrigger("CastSkill");
                    skills[number - 1].SetActive(true);
                    StartCoroutine(WaitForSkill(skills[number - 1], 2.5f));
                }
                break;
            case 2: 
                if (stats.Mana >= 50f)
                {
                    stats.Mana -= 50f;
                    anim.SetFloat("SkillType", 0.5f);
                    anim.SetTrigger("CastSkill");
                    skills[number - 1].SetActive(true);
                    StartCoroutine(WaitForSkill(skills[number - 1], 2f));
                }
                break;
            case 3:
                if (stats.Mana >= 50f)
                {
                    stats.Mana -= 50f;
                    anim.SetFloat("SkillType", 0.5f);
                    anim.SetTrigger("CastSkill");
                    skills[number - 1].SetActive(true);
                    StartCoroutine(WaitForSkill(skills[number - 1], 2f));
                }
                break;
            case 4: 
                if (stats.Mana >= 100f)
                {
                    stats.Mana -= 100f;
                    anim.SetFloat("SkillType", 1f);
                    anim.SetTrigger("CastSkill");
                    skills[number - 1].SetActive(true);
                    StartCoroutine(WaitForSkill(skills[number - 1], 2f));
                }
                break;
        }
        if(isUsingSkill || !isAlive || isTrading || (DialogueMenu.instance.currentManager != null && DialogueMenu.instance.currentManager.isTalking))
        {
            movement.isRunning = false;
            movement.canMove = false;
        }
        else
        {
            movement.canMove = true;
        }
        if(isUsingSkill || (DialogueMenu.instance.currentManager != null && DialogueMenu.instance.currentManager.isTalking)
            || GameController.instance.isSkillTreeVisible || GameController.instance.isQuestLogVisible)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        if(!isAlive)
        {
            rb.isKinematic = true;
        }
    }

    private IEnumerator WaitForSkill(GameObject obj, float time)
    {
        isUsingSkill = true;
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
        isUsingSkill = false;
    }

    public void GetHit(float dmg)
    {
        stats.Health -= (dmg - (stats.Defence));
        if(stats.Health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        isAlive = false;
        anim.SetBool("isAlive", isAlive);
    }

    private void CheckIfPlayerHasFallen()
    {
        if(timer > 0f && !movement.controller.isGrounded)
        {
            timer -= Time.deltaTime;
        }
        else if(movement.controller.isGrounded)
        {
            if (timer <= 0)
            {
                timer = 0f;
                stats.Health = 0f;
                Die();
            }
            else if(timer > 0 && timer <= 0.1)
            {
                stats.Health -= 80.0f;
            }
            else if(timer > 0.1 && timer <= 0.3)
            {
                stats.Health -= 60.0f;
            }
            else if (timer > 0.3 && timer <= 0.5)
            {
                stats.Health -= 40.0f;
            }
            timer = 1.8f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyWeapon") && enemy.isAlive)
        {
            if (Equipment.instance.leftHandSlot.gameObject.activeInHierarchy && CurrentShield.instance.hitBlocked)
            {
                return;
            }
            else if (Equipment.instance.leftHandSlot.gameObject.activeInHierarchy && !CurrentShield.instance.hitBlocked)
            {
                GetHit(collision.gameObject.GetComponentInParent<EnemyController>().stats.Attack);
            }
            else
            {
                GetHit(collision.gameObject.GetComponentInParent<EnemyController>().stats.Attack);
            }
        }
        else if(collision.gameObject.CompareTag("NPCWeapon") && collision.gameObject.GetComponentInParent<QuestGiver>().canAttackPlayer)
        {
            if (Equipment.instance.leftHandSlot.gameObject.activeInHierarchy && CurrentShield.instance.hitBlocked)
            {
                return;
            }
            else if (Equipment.instance.leftHandSlot.gameObject.activeInHierarchy && !CurrentShield.instance.hitBlocked)
            {
                GetHit(collision.gameObject.GetComponentInParent<QuestNpcController>().stats.Attack);
            }
            else
            {
                GetHit(collision.gameObject.GetComponentInParent<QuestNpcController>().stats.Attack);
            }
        }
        else
        {
            return;
        }
    }
    public void SetPlayerTradeMode()
    {
        isTrading = true;
        rb.isKinematic = true;
    }
    public void ExitPlayerTradeMode()
    {
        isTrading = false;
        rb.isKinematic = false;
    }
    public void enableWeapon()
    {
        if(Equipment.instance.rightHandSlot.gameObject.activeInHierarchy)
        {
            CurrentWeapon.instance.boxCollider.enabled = true;
        } 
    }

    public void disableWeapon()
    {
        if (Equipment.instance.rightHandSlot.gameObject.activeInHierarchy)
        {
            CurrentWeapon.instance.boxCollider.enabled = false;
        }
    }

    public void SetPosition(Vector3 pos)
    {
        movement.SetPosition(pos);
    }

    public void SetRotation(float x, float y, float z)
    {
        movement.SetRotation(x, y, z);
    }
}
