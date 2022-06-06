using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceTrigger : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;
    
    private void OnTriggerEnter(Collider other)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
