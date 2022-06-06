using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    private Image screenImage;

    [SerializeField]
    private Image mainMenuBtnImage;

    [SerializeField]
    private Text mainMenuBtnTxt;

    [SerializeField]
    private Text caption;

    private PlayerController player;
    private CameraController cameraController;
    public bool isAnimating = false;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        cameraController = FindObjectOfType<CameraController>(); 
    }

    public void FadeAnimation()
    {
        if (isAnimating)
        {
            screenImage.DOFade(1f, 3f);
            mainMenuBtnImage.DOFade(1f, 3f);
            mainMenuBtnTxt.DOFade(1f, 3f);

            caption.DOFade(1f, 3f).OnComplete(() =>
            {
                isAnimating = false;
            });
        }
    }
    public void OpenMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void ResetPlayerSettings()
    {
        gameObject.SetActive(false);
        player.stats.Health = player.stats.MaxHealth;
        player.stats.Stamina = player.stats.MaxStamina;
        player.stats.Mana = player.stats.MaxMana;
        player.isAlive = true;
        player.anim.SetBool("isAlive", player.isAlive);
        player.anim.SetFloat("PlayerEquipped", 0.0f);
        
        if(Equipment.instance.rightHandSlot.gameObject.activeInHierarchy)
            Equipment.instance.UnEquip();

        if(Equipment.instance.isShieldEquipped)
            Equipment.instance.UnEquipShield();
        
        GameController.instance.isGameOverPanelVisible = false;
    }
}
