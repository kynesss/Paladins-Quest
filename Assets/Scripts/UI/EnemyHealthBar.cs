using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyHealthBar : MonoBehaviour
{
    public static EnemyHealthBar instance;
    public EnemyController enemy;
    public Image fillingHealthBar;
    public Image healthBar;

    private void Awake()
    {
        if(instance != null)
        {
            return;
        }
        instance = this;
    }

    void Update()
    {
        fillingHealthBar.fillAmount = enemy.stats.Health / enemy.stats.MaxHealth;

        if(SceneManager.GetActiveScene().buildIndex == 1 && Camera.main != null)
            FollowCamera();
    }

    private void FollowCamera()
    {
        healthBar.transform.LookAt(Camera.main.transform.position);
        healthBar.transform.Rotate(0f, 180, 0f);

        if(!enemy.enabled)
        {
            healthBar.enabled = false;
        }
    }
}
