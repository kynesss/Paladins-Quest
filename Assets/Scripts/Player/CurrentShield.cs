using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentShield : MonoBehaviour
{
    public static CurrentShield instance;
    public BoxCollider boxCollider;
    public BoxCollider[] shieldColliders;
    public Item currentShield;
    public bool hitBlocked = false;

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
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;
    }
    private void Update()
    {
        SetShieldCollider();
    }

    public void SetShieldCollider()
    {
       if(currentShield != null)
        {
            foreach (var collider in shieldColliders)
            {
                if(currentShield.name == collider.name)
                {
                    boxCollider.size = collider.size;
                    boxCollider.center = collider.center;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "EnemyWeapon")
        {
            StartCoroutine(hitInformation());
        }
    }

    private IEnumerator hitInformation()
    {
        hitBlocked = true;

        if (hitBlocked)
        {
            yield return new WaitForSeconds(1f);
            hitBlocked = false;
        }
    }
}
