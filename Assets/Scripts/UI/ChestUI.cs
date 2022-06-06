using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChestUI : MonoBehaviour
{
    public static ChestUI instance;
    
    public GameObject panel;

    public Transform parent;

    public Slots[] chestSlots;

    public GameObject playerInfo;

    public GameController gameController;

    public ChestController currentChest;

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }

    private void Start()
    {
        panel.SetActive(false);
        chestSlots = parent.GetComponentsInChildren<Slots>();
        gameController = GameController.instance;
    }
    private void Update()
    {
        if(gameController.chest.Length > 0)
        FindCurrentChest();
    }
    private void OnEnable()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1)
            ChestController.instance.onChestChanged += UpdateUI;
    }
    private void OnDisable()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
            ChestController.instance.onChestChanged -= UpdateUI;
    }
    public void Show()
    {
        panel.SetActive(true);
        playerInfo.SetActive(false);
    }

    public void Hide()
    {
        panel.SetActive(false);
        playerInfo.SetActive(true);
    }

    public void UpdateUI()
    {
        for (int i = 0; i < chestSlots.Length; i++)
        {
            if(i < currentChest.chestModel.items.Count)
            {
                chestSlots[i].SetSlot(currentChest.chestModel.items[i]);
            }
            else
            {
                chestSlots[i].ClearSlot();
            }
        }
    }

    public void FindCurrentChest()
    { 
        for (int i = 0; i < gameController.chest.Length; i++)
        {
            if (gameController.chest[i].GetComponent<ChestController>().isOpen)
            {
                currentChest = gameController.chest[i].GetComponent<ChestController>();
            }
        }
    }
}
