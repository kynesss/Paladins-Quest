using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCController : Interactable
{
    public NPC npc;
    
    private Transform player;

    private Animator anim;

    private bool isPlayerInRange = false;

    public GameObject text3DName;

    public DialogueManager dialogueManager;

    private void Awake()
    {
        GetStartingRotation();
        npc.SetDialogues();
    }

    void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
        dialogueManager = GetComponent<DialogueManager>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isPlayerInRange)
        {
            LookAtPlayer(player);
        }
        else
        {
            BackToStartingRotation();
        }
        if (dialogueManager.isNpcResponding)
        {
            anim.SetBool("isTalking", true);
            anim.SetFloat("randomValue", DrawAnimation(dialogueManager.randomValue));
        }
        else
        {
            anim.SetBool("isTalking", false);
        }

        if (SceneManager.GetActiveScene().buildIndex == 1 && Camera.main != null)
            NameFollowCamera();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
    public void LookAtPlayer(Transform target)
    {
        Vector3 lookDirection = target.position - transform.position;
        lookDirection.y = 0f;
        Quaternion rotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 2.0f * Time.deltaTime);
    }
    public void GetStartingRotation()
    {
        PlayerPrefs.SetFloat("yRotation", transform.eulerAngles.y);
    }
    public void BackToStartingRotation()
    {
        var startingRotation = Quaternion.Euler(0f, PlayerPrefs.GetFloat("yRotation"), 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, startingRotation, Time.deltaTime);
    }
    
    public float DrawAnimation(float value)
    {
        if(value <= 0.4)
        {
            value = 0f;
        }
        else if(value > 0.4 && value < 0.8)
        {
            value = 0.5f;
        }
        else
        {
            value = 1.0f;
        }

        return value;
    }
    public void NameFollowCamera()
    {
        text3DName.transform.LookAt(Camera.main.transform.position);
        text3DName.transform.Rotate(0f, 180f, 0f);
        text3DName.GetComponent<TextMesh>().text = npc._name;
    }
}
