using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public GameObject playerPrefab;

    public PlayerController player;

    public PlayerInfoUI playerInfo;

    private SaveManager saveManager;

    [Header("Character stats")]

    public CharacterStats charStats;
    public GameObject charStatsUI;
    private bool isCharStatsVisible = false;

    [Header("Inventory")]

    public InventoryUI inventoryUI;
    [HideInInspector] public bool isInventoryVisible = false;
    public GameObject inventoryPanel;
    private bool isAnimating = false;
    
    [HideInInspector] public bool canPressKey = false;
    private float panelAnimTimer = 1f;

    [Header("Chest")]
    [HideInInspector]  public bool isChestUiVisible = false;
    public GameObject[] chest;
    public ChestUI chestUI;

    [Header("Skill tree")]
    public bool isSkillTreeVisible = false;
    public GameObject skillTreePanel;

    [Header("Game Over")]
    public GameOverUI gameOverPanel;
    public bool isGameOverPanelVisible = false;

    public GameObject GUI;
    
    public GameObject questLog;
    [HideInInspector] public bool isQuestLogVisible = false;

    public GameObject dialogueUI;

    public StaminaBar staminaBar;

    public Text levelTooLowText;

    public List<EnemyController> enemies;

    public bool isSvCheatsOn = false;

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        saveManager = SaveManager.instance;
        charStatsUI.SetActive(false);
        chestUI.gameObject.SetActive(false);

        if(gameOverPanel.gameObject.activeInHierarchy)
            gameOverPanel.gameObject.SetActive(false);
    }
    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.I) && !canPressKey) || TradeMenu.instance.startTrading || TradeMenu.instance.endTrading) 
        {
            canPressKey = true;
            ToggleInventory();
            TradeMenu.instance.startTrading = false;
            TradeMenu.instance.endTrading = false;
        }
        MoveInventoryPanel(); 

        if(Input.GetKeyDown(KeyCode.M))
        {
            ToggleSkillTree();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleCharStats();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            ToggleQuestLog();
        }

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (isChestUiVisible)
        {
            StartCoroutine(WaitForCurrentChest());
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            SaveGame();
        }
        if(chestUI.gameObject.activeInHierarchy)
        {
            chestUI.Show();
        }
        else
        {
            chestUI.Hide();
        }
        if ((DialogueMenu.instance.currentManager != null && DialogueMenu.instance.currentManager.isTalking))
        {
            playerInfo.gameObject.SetActive(false);
        }
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            if(!player.isAlive)
                DisplayGameOverScreen();
        }

        SV_Cheats();
    }
    public IEnumerator WaitForEnemies()
    {
        yield return new WaitForSeconds(1f);
        enemies = new List<EnemyController>(FindObjectsOfType<EnemyController>());

        if(!MenuController.instance.isCreatingNewFile && SceneManager.GetActiveScene().buildIndex == 0)
            StartCoroutine(SaveManager.instance.SetEnemyPositions());
    }

    public void RespawnPlayer()
    {
        var playerController = FindObjectOfType<PlayerController>();
        
        if(playerController == null)
        {
            GameObject tempPlayer = Instantiate(playerPrefab);
            player = tempPlayer.GetComponent<PlayerController>();
        }
        else
        {
            player = playerController;
        }
        playerInfo.RegisterPlayer(player);
        charStats.RegisterPlayer(player);
        staminaBar.RegisterPlayer(player);
    }

    private IEnumerator WaitForCurrentChest()
    {
        while(ChestUI.instance.currentChest == null)
        {
            yield return null;
        }
        ChestController.instance.onChestChanged?.Invoke();
    }

    public void InteractWithChest(ChestController chest)
    {
        if (!isChestUiVisible)
        {
            chestUI.gameObject.SetActive(true);
            isChestUiVisible = true;
        }

        chest.isOpen = true;
        chest.anim.SetBool("isOpen", true);

        if (chest.isOpen)
        {
            player.anim.SetTrigger("OpenChest");
        }
    }

    private void ToggleCharStats()
    {
        if (!isCharStatsVisible)
        {
            charStatsUI.SetActive(true);
            isCharStatsVisible = true;
        }
        else
        {
            charStatsUI.SetActive(false);
            isCharStatsVisible = false;
        }
    }
   
    public void MoveInventoryPanel()
    {
        if (!isAnimating && isInventoryVisible && canPressKey && panelAnimTimer > 0f)
         {
            panelAnimTimer -= Time.deltaTime;
            inventoryPanel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(650f, 19.96f), panelAnimTimer);
            inventoryPanel.GetComponent<Image>().DOFade(1f, 5f);

            if(panelAnimTimer <= 0)
            {
                isAnimating = true;
                canPressKey = false;
                panelAnimTimer = 1f;
            }
         }
         else if(isAnimating && isInventoryVisible && canPressKey && panelAnimTimer > 0f)
         {
            panelAnimTimer -= Time.deltaTime;
            inventoryPanel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(transform.position.x + 235f, 19.96f), panelAnimTimer);
            inventoryPanel.GetComponent<Image>().DOFade(0f, panelAnimTimer);

            if (panelAnimTimer <= 0)
            {
                isAnimating = false;
                canPressKey = false;
                panelAnimTimer = 1f;
                ToggleInventory();
            }
         }
    }
    public void ToggleInventory()
    {
        if (!isInventoryVisible && !isAnimating)
        {
            inventoryUI.Show();
            isInventoryVisible = true;
        }
        else if(isInventoryVisible && !isAnimating)
        {
            inventoryUI.Hide();
            isInventoryVisible = false;
        }
    }
    private void ToggleQuestLog()
    {
        if (!isQuestLogVisible)
        {
            questLog.SetActive(true);
            isQuestLogVisible = true;
        }
        else
        {
            questLog.SetActive(false);
            isQuestLogVisible = false;
        }
    }
    private void ToggleSkillTree()
    {
        if (!isSkillTreeVisible)
        {
            skillTreePanel.SetActive(true);
            isSkillTreeVisible = true;
        }
        else
        {
            skillTreePanel.SetActive(false);
            isSkillTreeVisible = false;
        }
    }
    public void AddExperience(float value)
    {
        bool hasLeveledUp = player.stats.AddExp(value);
    }
    public void SaveGame()
    {
        saveManager.SaveGame();
    }

    public void ShowGUI()
    {
        GUI.SetActive(true);
    }
    public void HideGUI()
    {
        GUI.SetActive(false);
    }
    public void LevelTooLowInfo()
    {
        levelTooLowText.DOFade(1f, 1f).OnComplete(() =>
        {
            levelTooLowText.DOFade(0f, 1f).OnComplete(() =>
            {

            });
        });
    }
    public void DisplayGameOverScreen()
    {
        gameOverPanel.gameObject.SetActive(true);
        isGameOverPanelVisible = true;
        gameOverPanel.isAnimating = true;
        gameOverPanel.FadeAnimation();
    }
    public List<float> GetEnemyXpositions()
    {
        List<float> enemyPosX = new List<float>();

        foreach (EnemyController enemy in enemies)
        {
            enemyPosX.Add(enemy.transform.position.x);
        }

        return enemyPosX;
    }
    public List<float> GetEnemyYpositions()
    {
        List<float> enemyPosY = new List<float>();

        foreach (EnemyController enemy in enemies)
        {
            enemyPosY.Add(enemy.transform.position.y);
        }

        return enemyPosY;
    }
    public List<float> GetEnemyZpositions()
    {
        List<float> enemyPosZ = new List<float>();

        foreach (EnemyController enemy in enemies)
        {
            enemyPosZ.Add(enemy.transform.position.z);
        }

        return enemyPosZ;
    }
    public List<float> GetEnemyXrotations()
    {
        List<float> enemyRotX = new List<float>();

        foreach (EnemyController enemy in enemies)
        {
            enemyRotX.Add(enemy.transform.eulerAngles.x);
        }

        return enemyRotX;
    }
    public List<float> GetEnemyYrotations()
    {
        List<float> enemyRotY = new List<float>();

        foreach (EnemyController enemy in enemies)
        {
            enemyRotY.Add(enemy.transform.eulerAngles.y);
        }

        return enemyRotY;
    }
    public List<float> GetEnemyZrotations()
    {
        List<float> enemyRotZ = new List<float>();

        foreach (EnemyController enemy in enemies)
        {
            enemyRotZ.Add(enemy.transform.eulerAngles.z);
        }

        return enemyRotZ;
    }
    public void SV_Cheats()
    {
        float defenceCheat = 50f;
        float walkCheat = 10f;
        float jumpCheat = 1.0f;
        float attackCheat = 30.0f;
        int levelCheat = 30;

        if (Input.GetKeyDown(KeyCode.Alpha9) && !isSvCheatsOn)
        {
            player.stats.Defence += defenceCheat;
            player.stats.Level += levelCheat;
            player.movement.walkSpeed += walkCheat;
            player.movement.jumpHeight += jumpCheat;
            player.stats.Attack += attackCheat;
            isSvCheatsOn = true;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha9) && isSvCheatsOn)
        {
            player.stats.Defence -= defenceCheat;
            player.stats.Level -= levelCheat;
            player.movement.walkSpeed -= walkCheat;
            player.movement.jumpHeight -= jumpCheat;
            player.stats.Attack -= attackCheat;
            isSvCheatsOn = false;
        }

        if(Input.GetKeyDown(KeyCode.Alpha0) && isSvCheatsOn)
        {
            player.stats.Health = player.stats.MaxHealth;
            player.stats.Stamina = player.stats.MaxStamina;
            player.stats.Mana = player.stats.MaxMana;
        }
    }
}
