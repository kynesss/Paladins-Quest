using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Trader : MonoBehaviour
{
    private PlayerController player;

    public TraderModel trader;

    [SerializeField] protected bool isPlayerInRange = false;

    public TradeDialogueManager manager;

    public bool isTrading = false;

    public GameObject name3D;

    public List<Item> tempItems;

    private void Awake()
    {
        GetStartingRotation(transform.rotation);
        AddTemporaryItems();
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        manager = GetComponent<TradeDialogueManager>();
    }
    private void Update()
    {
        if (isPlayerInRange)
        {
            LookAtPlayer(player.transform);
        }
        else
        {
            BackToStartingRotation();
        }

        CanInterract();

        if (Input.GetKeyDown(KeyCode.E) && CanInterract())
        {
            Interract();
        }
        if (isTrading)
        {
            manager.i = 0;
            TradeController.instance.SetCurrentTrader(this);
            isTrading = false;
        }

        if(SceneManager.GetActiveScene().buildIndex == 1 && Camera.main != null)
            Text3DFollowCamera();
    }
    public void AddTemporaryItems()
    {
        trader.items.Clear();

        trader.items.AddRange(tempItems);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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

    public void GetStartingRotation(Quaternion startRot)
    {
        PlayerPrefs.SetFloat("yRot", startRot.eulerAngles.y);
    }
    public void BackToStartingRotation()
    {
        Quaternion startRot = Quaternion.Euler(0f, PlayerPrefs.GetFloat("yRot"), 0f);

        if (transform.rotation.y != startRot.y)
            transform.rotation = Quaternion.Slerp(transform.rotation, startRot, Time.deltaTime * 2f);
    }
    public bool CanInterract()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 1.5f)
        {
            return true;
        }
        return false;
    }
    public void Interract()
    {
        isTrading = true;
        TradeMenu.instance.OpenPanel();
    }
    public void LookAtPlayer(Transform playerTransform)
    {
        Vector3 lookDirection = playerTransform.position - transform.position;
        lookDirection.y = 0f; 
        Quaternion rotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2f);
    }
    public void Text3DFollowCamera()
    {
        name3D.transform.LookAt(Camera.main.transform.position);
        name3D.transform.Rotate(0f, 180f, 0f);
        name3D.GetComponent<TextMesh>().text = trader.Name;
    }
}
