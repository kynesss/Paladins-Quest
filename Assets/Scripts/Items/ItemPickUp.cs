using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : Interactable
{
    public Item item;
    PlayerController player;
    public bool CanPickUp = false;
    public MeshCollider meshCollider;

    private void Start()
    {
        meshFilter.sharedMesh = item.mesh.sharedMesh;
        meshRenderer.sharedMaterials = item.mesh.sharedMaterials;
        player = FindObjectOfType<PlayerController>();
        meshCollider = GetComponent<MeshCollider>();
    }

    public override void Interact()
    {
        base.Interact();
        
        if(CanPickUp && Inventory.instance.Add(item))
        {
           Destroy(gameObject);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            CanPickUp = true;
            player.itemTarget = this;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanPickUp = false;
        }
    }
}
