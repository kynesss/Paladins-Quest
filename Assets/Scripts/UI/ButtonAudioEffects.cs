using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonAudioEffects : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;
    private Button button;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        button = GetComponent<Button>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(button.interactable)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(button.interactable)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }
}
