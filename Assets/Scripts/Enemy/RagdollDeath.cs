using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollDeath : MonoBehaviour
{
    public static RagdollDeath instance;

    [Header("References")]
    [SerializeField] private Animator anim = null;

    public Rigidbody bodyRb;
    private Rigidbody[] ragdollBodies;
    private Collider[] ragdollColliders;
    public List<Collider> turnOnColliders;

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
        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();
        anim = GetComponent<Animator>();
        bodyRb = GetComponent<Rigidbody>();

        ToggleRagdoll(false);
        foreach (Collider collider in turnOnColliders)
        {
            collider.enabled = true;
        }
        bodyRb.isKinematic = false;
    }

    public void ToggleRagdoll(bool state)
    {
        anim.enabled = !state;

        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.isKinematic = !state;
        }
        foreach (Collider collider in ragdollColliders)
        {
            collider.enabled = state;
        }
    }
}
