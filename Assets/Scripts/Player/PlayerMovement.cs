using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator anim;
    public CharacterController controller;
    PlayerController player;

    [SerializeField] private float moveSpeed;
    [SerializeField] public float walkSpeed = 5.0f;
    [SerializeField] private float runSpeed = 7.0f;
    [SerializeField] public float jumpHeight = 0.5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundCheckDistance = 0.4f;
    [SerializeField] private LayerMask groundLayerMask;

    [SerializeField] private bool isGrounded;
    [HideInInspector] public bool isRunning = false;
    [HideInInspector] public bool isWalking = false;
    [HideInInspector] public bool isJumping = false;

    private Vector3 moveDirection;
    private Vector3 velocity;

    public bool canMove = true;

    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        player = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if(canMove)
            Move();
    }
    public void Move()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundLayerMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        moveDirection = new Vector3(moveX, 0, moveZ);
        moveDirection = transform.TransformDirection(moveDirection);

        if (isGrounded)
        {
            if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                if (!player.isEquipped)
                {
                    SetMovementAnimValues(0.5f, moveZ, moveX);
                }
                else if (player.isEquipped)
                {
                    SetMovementAnimValues(0.75f, moveZ, moveX);
                }
                Walk();
            }
            else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
                if (!player.isEquipped)
                {
                    SetMovementAnimValues(0.5f, moveZ, moveX);
                }
                else if (player.isEquipped)
                {
                    SetMovementAnimValues(0.75f, moveZ, moveX);
                }
                Run();
            }
            else if (moveDirection == Vector3.zero)
            {
                moveSpeed = 0f;
                isWalking = false;
                anim.SetFloat("vertical", moveZ);
                anim.SetFloat("horizontal", moveX);
            }
            if (Input.GetKeyUp(KeyCode.Space) && controller.isGrounded && !isJumping)
            {
                Jump();
            }
            moveDirection *= moveSpeed;
        }

        controller.Move(moveDirection * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }


    private void Walk()
    {
        moveSpeed = walkSpeed;
        isRunning = false;
        isWalking = true;
        anim.SetBool("isRunning", false);
    }

    public void Jump()
    {
        if (CheckIsPlayerCanJump())
        {
            anim.SetTrigger("Jump");
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            player.stats.Stamina -= 5.0f;
        }
        //if(player.stats.Stamina >= 5.0f)
        //    StartCoroutine(JumpCheck());
    }

    public IEnumerator JumpCheck()
    {
        while (isGrounded)
        {
            isJumping = true;
            yield return null;
        };

        isJumping = false;
    }
    public void Run()
    {
        if (CheckIsPlayerCanRun())
        {
            moveSpeed = runSpeed;
            isRunning = true;
            isWalking = false;
            anim.SetBool("isRunning", true);
        }
        else
            Walk();
    }

    public void SetPosition(Vector3 pos)
    {
        controller.enabled = false;
        transform.position = pos;
        controller.enabled = true;
    }
    public void SetRotation(float x, float y, float z)
    {
        controller.enabled = false;
        transform.rotation = Quaternion.Euler(x, y, z);
        controller.enabled = true;
    }
    public void SetPlayerPose()
    {
        if ((DialogueMenu.instance.currentManager != null && DialogueMenu.instance.currentManager.isTalking && !DialogueMenu.instance.currentManager.isEndOfTalk) 
            || TradeController.instance.currentTrader != null || GameController.instance.isQuestLogVisible || GameController.instance.isSkillTreeVisible)
        {
            moveSpeed = 0f;
            anim.SetFloat("vertical", 0f);
            anim.SetFloat("horizontal", 0f);
            anim.SetBool("isRunning", false);
            canMove = false;
        }
        else
            canMove = true;
    }
    public bool CheckIsPlayerCanRun()
    {
        if (player.stats.Stamina > 0f)
            return true;
        else
            return false;
    }
    public bool CheckIsPlayerCanJump()
    {
        if (player.stats.Stamina >= 5.0f)
            return true;
        else
            return false;
    }
    public void SetMovementAnimValues(float playerValue, float zValue, float xValue)
    {
        anim.SetFloat("PlayerEquipped", playerValue);
        anim.SetFloat("vertical", zValue);
        anim.SetFloat("horizontal", xValue);
    }
}
