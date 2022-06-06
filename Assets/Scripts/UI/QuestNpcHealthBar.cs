using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestNpcHealthBar : MonoBehaviour
{
    public Image healthImage;

    public QuestNpcController questNpcController;

    private void Update()
    {
        healthImage.fillAmount = questNpcController.stats.Health / questNpcController.stats.MaxHealth;
    }
}
