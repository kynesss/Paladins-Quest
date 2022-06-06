using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class CurrentWeapon : MonoBehaviour
{
    public static CurrentWeapon instance;
    public Item item;
    private PlayerController player;
    public BoxCollider[] weaponColliders;
    public BoxCollider boxCollider;
    [SerializeField] private GameObject swordPoisonTrail;
    public bool isWeaponInfected = false;

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
        player = FindObjectOfType<PlayerController>();
        boxCollider = GetComponent<BoxCollider>();
        disableWeapon();
    }

    private void Update()
    {
        GetBoxCollider(item);
    }

    private void OnEnable()
    {
        Equipment.instance.onStatsChanged -= DeductWeaponAttack;
        Equipment.instance.onStatsChanged += GetWeaponAttack;
        StartCoroutine(WaitForCurrentWeapon());
    }
    private void OnDisable()
    {
        Equipment.instance.onStatsChanged -= GetWeaponAttack;
        Equipment.instance.onStatsChanged += DeductWeaponAttack;
        player.onPlayerAttacked.RemoveListener(DeductWeaponConditionModifier);
        SkillManager.instance.onSkillActivated.RemoveListener(SetWeaponPoisonEffect);
    }

    public void GetWeaponAttack(Item item)
    {
        if (item != null && gameObject.activeInHierarchy)
        {
            player.stats.Attack += item.damageModifier;
        }
    }

    public void DeductWeaponAttack(Item item)
    {
        player.stats.Attack = player.stats.baseAttack;
    }

    public void GetBoxCollider(Item item)
    {
        if (item != null)
        {
            for (int i = 0; i < weaponColliders.Length; i++)
            {
                if (weaponColliders[i].name == item.Name)
                {
                    boxCollider.size = weaponColliders[i].size;
                    boxCollider.center = weaponColliders[i].center;
                }
            }
        }
    }

    public void enableWeapon()
    {
        boxCollider.enabled = true;
    }

    public void disableWeapon()
    {
        boxCollider.enabled = false;
    }
    public void CheckWeaponType(Item item)
    {
        if(item.weaponType == WeaponType.OneHanded)
        {
            item.weaponType = WeaponType.OneHanded;
        }
        else if(item.weaponType == WeaponType.TwoHanded)
        {
            item.weaponType = WeaponType.TwoHanded;
        }

        SetPoisonEffectPosition(item.weaponType);
    }
    public void DeductWeaponConditionModifier()
    {
        if(item != null && item.itemType == ItemType.Weapon && item.weaponConditionModifier > 0)
        {
            item.weaponConditionModifier -= 0.1f;

            if(item.weaponConditionModifier <= 0)
            {
                Equipment.instance.Remove(item);
                item = null;
                gameObject.SetActive(false);
                player.isEquipped = false;
                player.stats.Attack = player.stats.baseAttack;
            }
        }
    }
    private IEnumerator WaitForCurrentWeapon()
    {
        while(item == null)
        {
            yield return null;
        }
        player.onPlayerAttacked.AddListener(DeductWeaponConditionModifier);
        SkillManager.instance.onSkillActivated.AddListener(SetWeaponPoisonEffect);

        if (SkillManager.instance.skills[2].isActivated && item.itemType == ItemType.Weapon)
        {
            SkillManager.instance.onSkillActivated.Invoke();
        }
    }
    public void SetWeaponPoisonEffect()
    {
        if(item.itemType == ItemType.Weapon && SkillManager.instance.skills[2].isActivated)
        { 
            swordPoisonTrail.SetActive(true);
            isWeaponInfected = true;
        }
        else
        { 
            swordPoisonTrail.SetActive(false);
            isWeaponInfected = false;
        }
    }
    private void SetPoisonEffectPosition(WeaponType type)
    {
        if(type == WeaponType.OneHanded)
        {
            swordPoisonTrail.transform.localPosition = new Vector3(0.038f, -0.126f, 0.802f);
        }
        else
        {
            swordPoisonTrail.transform.localPosition = new Vector3(0.079f, -0.091f, 0.485f);
        }
    }
}
