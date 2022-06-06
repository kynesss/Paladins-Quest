using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    private PlayerController player;

    [SerializeField] private Text levelTooLowText;
    
    public Skill[] skills;
    public SkillButton[] skillButtons;

    [SerializeField] public Skill activateSkill;

    public UnityEvent onSkillActivated;

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
        player = FindObjectOfType<PlayerController>();
    }
    public void UnlockSkill()
    {
        if(activateSkill.requiredLvl <= player.stats.Level)
        {
            activateSkill.isActivated = true;

            UnlockPoisonSkill(activateSkill);
        }
        else
        {
            DisplayLevelTooLowInfo();
        }
    }
    private void DisplayLevelTooLowInfo()
    {
        levelTooLowText.DOFade(1f, 1f).OnComplete(() =>
        {
            levelTooLowText.DOFade(0f, 1f).OnComplete(() =>
            {

            });
        });
    }

    private void UnlockPoisonSkill(Skill skill)
    {
        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i] == skill)
            {
                onSkillActivated.Invoke();
            }
        }
    }
}

