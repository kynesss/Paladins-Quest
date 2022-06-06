using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTouch : MonoBehaviour
{
    public GameObject gameObject;

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
