using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    public static ChestController instance;

    public ChestModel chestModel;

    Inventory inventory;

    public Animator anim;

    public bool CanOpen = false;
    public bool isOpen = false;
    public bool isListFull { get { return chestModel.items.Count >= 16; } }

    private float distance;

    public delegate void OnChestChanged();
    public OnChestChanged onChestChanged;

    public delegate bool OnSlotsChanged();
    public OnSlotsChanged onSlotsChanged;

    private void Awake()
    {
        if(instance != null)
        {
            return;
        }
        instance = this;
    }

    private void Start()
    {
        inventory = Inventory.instance;
    }

    private void Update()
    {
        if(CanOpen && Input.GetKeyDown(KeyCode.E))
        {
            GameController.instance.InteractWithChest(this);
        }
        else if (isOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            isOpen = false;
            anim.SetBool("isOpen", false);
        }
    }

    public void Add(Item item)
    {
        if (!isListFull)
        {
            chestModel.items.Add(item);

            if (onChestChanged != null)
                onChestChanged.Invoke();
        }
    }

    public void Remove(Item item)
    {
        chestModel.items.Remove(item);

        if (onChestChanged != null)
            onChestChanged.Invoke();
    }

    public void AddItemToInventory(Item item)
    {
        for (int i = 0; i < chestModel.items.Count; i++)
        {
            if(chestModel.items[i] != null && !inventory.isInventoryFull)
            {
                inventory.Add(item);
                Remove(item);
                break;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            distance = Vector3.Distance(other.transform.position, transform.position);

            if (distance < 1.5f)
            {
                CanOpen = true;
            }
            else
            {
                CanOpen = false;
                isOpen = false;
                anim.SetBool("isOpen", false);

                if (!isOpen)
                {
                    GameController.instance.chestUI.gameObject.SetActive(false);
                    GameController.instance.isChestUiVisible = false;
                }

            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            CanOpen = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanOpen = false;
        }
    }
}
